#include "bindings/proxy.h"
#include <stdbool.h>
#include <stdint.h>
#include <stdlib.h>
#include <string.h>

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

void exports_wasi_http_incoming_handler_handle(
    wasi_http_types_own_incoming_request_t request,
    wasi_http_types_own_response_outparam_t response_out) {

  wasi_http_types_own_fields_t headers;
  wasi_http_types_header_error_t headers_err;

  proxy_list_tuple2_field_name_field_value_t entries;
  entries.ptr = malloc(1 * sizeof(proxy_tuple2_field_name_field_value_t));
  entries.len = 1;
  set_string(&entries.ptr[0].f0, "Content-Type");
  set_string_field(&entries.ptr[0].f1, "text/plain");

  if (!wasi_http_types_static_fields_from_list(&entries, &headers,
                                               &headers_err)) {
    wasi_http_types_result_own_outgoing_response_error_code_t result = {
        .is_err = true, .val.err = headers_err.tag};
    wasi_http_types_static_response_outparam_set(response_out, &result);
    wasi_http_types_fields_drop_own(headers);
    free(entries.ptr);
    return;
  }

  wasi_http_types_own_outgoing_response_t res =
      wasi_http_types_constructor_outgoing_response(headers);

  wasi_http_types_status_code_t status_code = 200;
  wasi_http_types_method_outgoing_response_set_status_code(
      wasi_http_types_borrow_outgoing_response(res), status_code);

  wasi_http_types_own_outgoing_body_t body;
  if (!wasi_http_types_method_outgoing_response_body(
          wasi_http_types_borrow_outgoing_response(res), &body)) {
    wasi_http_types_result_own_outgoing_response_error_code_t result = {
        .is_err = true, .val.err = WASI_HTTP_TYPES_ERROR_CODE_INTERNAL_ERROR};
    wasi_http_types_static_response_outparam_set(response_out, &result);
    wasi_http_types_outgoing_response_drop_own(res);
    free(entries.ptr);
    return;
  }

  wasi_http_types_own_output_stream_t out_body_stream;
  if (!wasi_http_types_method_outgoing_body_write(
          wasi_http_types_borrow_outgoing_body(body), &out_body_stream)) {
    wasi_http_types_result_own_outgoing_response_error_code_t result = {
        .is_err = true, .val.err = WASI_HTTP_TYPES_ERROR_CODE_INTERNAL_ERROR};
    wasi_http_types_static_response_outparam_set(response_out, &result);
    wasi_http_types_outgoing_body_drop_own(body);
    wasi_http_types_outgoing_response_drop_own(res);
    free(entries.ptr);
    return;
  }

  wasi_io_streams_stream_error_t stream_err;
  proxy_list_u8_t body_contents;
  set_bytes(&body_contents, "Hello world!");

  if (!wasi_io_streams_method_output_stream_blocking_write_and_flush(
          wasi_io_streams_borrow_output_stream(out_body_stream), &body_contents,
          &stream_err)) {
    wasi_http_types_result_own_outgoing_response_error_code_t result = {
        .is_err = true, .val.err = stream_err.tag};
    wasi_http_types_static_response_outparam_set(response_out, &result);
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
    wasi_http_types_static_response_outparam_set(response_out, &result);
    wasi_http_types_outgoing_body_drop_own(body);
    wasi_http_types_outgoing_response_drop_own(res);
    free(entries.ptr);
    return;
  }

  wasi_http_types_result_own_outgoing_response_error_code_t result = {
      .is_err = false, .val.ok = res};
  wasi_http_types_static_response_outparam_set(response_out, &result);

  free(entries.ptr);
  return;
}