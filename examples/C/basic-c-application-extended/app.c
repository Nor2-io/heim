#pragma warning(                                                               \
    disable : 4996) // getenv is unsafe but getenv_s is msvc specific
#include "bindings/proxy.h"
#include "jsmn.h"
#include <stdbool.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define MAX_TOKENS 128

void set_string(proxy_string_t *ret, const char *str) {
ret->ptr = (uint8_t *)str;
ret->len = strlen(str);
}
void set_string_field(wasi_http_types_field_value_t *ret, const char *str) {
ret->ptr = (uint8_t *)str;
ret->len = strlen(str);
}
void set_bytes(proxy_list_u8_t *ret, const char *data) {
ret->ptr = (uint8_t *)data;
ret->len = strlen(data);
}

// Sends a response with the supplied status code, content type and message
// CAUTION! Do not malloc() the message, if it is malloc():ed it needs to be
// freed here which it is not
void send_response(wasi_http_types_own_response_outparam_t *response_out,
                int response_status_code, const char *content_type,
                const char *message) {

wasi_http_types_own_fields_t headers;
wasi_http_types_header_error_t headers_err;

proxy_list_tuple2_field_name_field_value_t entries;
entries.ptr = malloc(1 * sizeof(proxy_tuple2_field_name_field_value_t));
entries.len = 1;
set_string(&entries.ptr[0].f0, "Content-Type");
set_string_field(&entries.ptr[0].f1, content_type);

if (!wasi_http_types_static_fields_from_list(&entries, &headers,
                                            &headers_err)) {
    wasi_http_types_result_own_outgoing_response_error_code_t result = {
        .is_err = true, .val.err = headers_err.tag};
    wasi_http_types_static_response_outparam_set(*response_out, &result);
    wasi_http_types_fields_drop_own(headers);
    free(entries.ptr);
    return;
}

wasi_http_types_own_outgoing_response_t res =
    wasi_http_types_constructor_outgoing_response(headers);

wasi_http_types_status_code_t status_code = response_status_code;
wasi_http_types_method_outgoing_response_set_status_code(
    wasi_http_types_borrow_outgoing_response(res), status_code);

wasi_http_types_own_outgoing_body_t body;
if (!wasi_http_types_method_outgoing_response_body(
        wasi_http_types_borrow_outgoing_response(res), &body)) {
    wasi_http_types_result_own_outgoing_response_error_code_t result = {
        .is_err = true, .val.err = WASI_HTTP_TYPES_ERROR_CODE_INTERNAL_ERROR};
    wasi_http_types_static_response_outparam_set(*response_out, &result);
    wasi_http_types_outgoing_response_drop_own(res);
    free(entries.ptr);
    return;
}

wasi_http_types_own_output_stream_t out_body_stream;
if (!wasi_http_types_method_outgoing_body_write(
        wasi_http_types_borrow_outgoing_body(body), &out_body_stream)) {
    wasi_http_types_result_own_outgoing_response_error_code_t result = {
        .is_err = true, .val.err = WASI_HTTP_TYPES_ERROR_CODE_INTERNAL_ERROR};
    wasi_http_types_static_response_outparam_set(*response_out, &result);
    wasi_http_types_outgoing_body_drop_own(body);
    wasi_http_types_outgoing_response_drop_own(res);
    free(entries.ptr);
    return;
}

wasi_io_streams_stream_error_t stream_err;
proxy_list_u8_t body_contents;
set_bytes(&body_contents, message);

if (!wasi_io_streams_method_output_stream_blocking_write_and_flush(
        wasi_io_streams_borrow_output_stream(out_body_stream), &body_contents,
        &stream_err)) {
    wasi_http_types_result_own_outgoing_response_error_code_t result = {
        .is_err = true, .val.err = stream_err.tag};
    wasi_http_types_static_response_outparam_set(*response_out, &result);
    wasi_io_streams_output_stream_drop_own(out_body_stream);
    wasi_http_types_outgoing_body_drop_own(body);
    wasi_http_types_outgoing_response_drop_own(res);
    free(entries.ptr);
    return;
}

wasi_io_streams_output_stream_drop_own(out_body_stream);

wasi_http_types_error_code_t err;
if (!wasi_http_types_static_outgoing_body_finish(body, NULL, &err)) {
    wasi_http_types_result_own_outgoing_response_error_code_t result = {
        .is_err = true, .val.err = err.tag};
    wasi_http_types_static_response_outparam_set(*response_out, &result);
    wasi_http_types_outgoing_body_drop_own(body);
    wasi_http_types_outgoing_response_drop_own(res);
    free(entries.ptr);
    return;
}

wasi_http_types_result_own_outgoing_response_error_code_t result = {
    .is_err = false, .val.ok = res};
wasi_http_types_static_response_outparam_set(*response_out, &result);

free(entries.ptr);
return;
}

void exports_wasi_http_incoming_handler_handle(
    wasi_http_types_own_incoming_request_t request,
    wasi_http_types_own_response_outparam_t response_out) {

wasi_http_types_own_headers_t incoming_header =
    wasi_http_types_method_incoming_request_headers(
        wasi_http_types_borrow_incoming_request(request));
wasi_http_types_borrow_fields_t borrowed_headers =
    wasi_http_types_borrow_fields(incoming_header);

proxy_list_field_value_t content_type_header;
wasi_http_types_field_name_t content_type;
set_string(&content_type, "Content-Type");

wasi_http_types_method_fields_get(borrowed_headers, &content_type,
                                    &content_type_header);

const char *json_mime = "application/json";
size_t json_mime_len = strlen(json_mime);

bool found_application_json = false;

for (size_t i = 0; i < content_type_header.len; i++) {
    wasi_http_types_field_value_t val = content_type_header.ptr[i];
    if (val.len == json_mime_len &&
        memcmp(val.ptr, json_mime, json_mime_len) == 0) {
    found_application_json = true;
    break;
    }
}

if (!found_application_json) {
    send_response(&response_out, 500, "text/plain",
                "Missing header Content-Type: application/json");
    return;
}

wasi_http_types_own_incoming_body_t incoming_raw_body;
if (!wasi_http_types_method_incoming_request_consume(
        wasi_http_types_borrow_incoming_request(request),
        &incoming_raw_body)) {
    send_response(&response_out, 500, "text/plain",
                "wasi_http_types_method_incoming_request_consume was called "
                "multiple times");
    return;
}

wasi_http_types_own_input_stream_t incoming_body_stream;
if (!wasi_http_types_method_incoming_body_stream(
        wasi_http_types_borrow_incoming_body(incoming_raw_body),
        &incoming_body_stream)) {

    send_response(&response_out, 500, "text/plain",
                "wasi_http_types_method_incoming_body_stream was called "
                "multiple times");
    return;
}

bool eof = false;
uint8_t *incoming_body = NULL;
uint64_t len = 0;
size_t capacity = 1024;

while (!eof) {
    if (len >= capacity) {
    capacity *= 2;
    incoming_body = realloc(incoming_body, capacity);
    if (!incoming_body) {
        send_response(&response_out, 500, "text/plain", "realloc failed");
        return;
    }
    }
    proxy_list_u8_t chunk;
    wasi_io_streams_stream_error_t stream_err;
    if (wasi_io_streams_method_input_stream_blocking_read(
            wasi_io_streams_borrow_input_stream(incoming_body_stream), 1024,
            &chunk, &stream_err)) {
    len += chunk.len;
    uint8_t *buf = realloc(incoming_body, len);

    if (!buf) {
        proxy_list_u8_free(&chunk);
        wasi_io_streams_input_stream_drop_own(incoming_body_stream);
        wasi_http_types_incoming_body_drop_own(incoming_raw_body);
        send_response(&response_out, 500, "text/plain", "realloc failed");
        return;
    }

    incoming_body = buf;

    memcpy(incoming_body + len - chunk.len, chunk.ptr, chunk.len);
    } else {
    if (stream_err.tag == WASI_IO_STREAMS_STREAM_ERROR_CLOSED) {
        eof = true;
    } else {
        wasi_io_streams_input_stream_drop_own(incoming_body_stream);
        wasi_http_types_incoming_body_drop_own(incoming_raw_body);
        send_response(&response_out, 500, "text/plain",
                    "failed to read body stream");

        return;
    }
    }
    proxy_list_u8_free(&chunk);
}

if (len == 0) {
    wasi_io_streams_input_stream_drop_own(incoming_body_stream);
    wasi_http_types_incoming_body_drop_own(incoming_raw_body);
    send_response(&response_out, 400, "text/plain", "Empty body received");
    free(incoming_body);
    return;
}

char *json_string = malloc(len + 1);
memcpy(json_string, incoming_body, len);
json_string[len] = '\0';

jsmn_parser parser;
jsmn_init(&parser);

jsmntok_t tokens[MAX_TOKENS];

// Parse JSON
int token_count =
    jsmn_parse(&parser, json_string, strlen(json_string), tokens, MAX_TOKENS);

if (token_count <= 0) {
    wasi_io_streams_input_stream_drop_own(incoming_body_stream);
    wasi_http_types_incoming_body_drop_own(incoming_raw_body);
    send_response(&response_out, 500, "text/plain",
                "No json was found in body");
    free(incoming_body);
    free(json_string);
    return;
}

char name[64] = {0};
for (int i = 1; i < token_count - 1; i++) {
    if (tokens[i].type == JSMN_STRING && tokens[i].end - tokens[i].start == 4 &&
        strncmp(json_string + tokens[i].start, "name", 4) == 0) {

    jsmntok_t val = tokens[i + 1];
    int len = val.end - val.start;

    if (len >= sizeof(name))
        len = sizeof(name) - 1;

    memcpy(name, json_string + val.start, len);
    name[len] = '\0';
    break;
    }

    i++;
}

char dest[64];
const char *raw = getenv("HELLO_MESSAGE");
if (raw) {
    strncpy(dest, raw, sizeof(dest) - 1);
    dest[sizeof(dest) - 1] = '\0';
} else {
    send_response(&response_out, 400, "text/plain",
                "No environment variable 'HELLO_MESSAGE' was found");
    return;
}

size_t response_length = strlen(dest) + 1 + strlen(name) + 1;
char *response = malloc(response_length);
snprintf(response, response_length, "%s %s", dest, name);

size_t dest_len = strlen(dest);
char *dest_compacted = malloc(dest_len + 1);

if (dest_compacted) {
    memcpy(dest_compacted, dest, dest_len + 1);
}

send_response(&response_out, 200, "text/plain", response);
free(response);
free(dest_compacted);
free(json_string);
return;
}