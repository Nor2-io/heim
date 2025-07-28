package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	incominghandler "go-http/wasi/http/incoming-handler"
	"go-http/wasi/http/types"
	"os"

	"go.bytecodealliance.org/cm"
)

func init() {
	incominghandler.Exports.Handle = Handle
}

func main() {}

type Data struct {
	Name string `json:"name"`
}

func Handle(request types.IncomingRequest, responseOutParam types.ResponseOutparam) {
	incomingContentType := request.Headers().Get("Content-Type").Slice()
	contentTypeFound := false
	for _, val := range incomingContentType {
		if string(val.Slice()) == "application/json" {
			contentTypeFound = true
			break
		}
	}

	if !contentTypeFound {
		sendResponse(responseOutParam, 415, "text/plain", "Missing header Content-Type: application/json")
		return
	}

	incomingBody, _, isErr := request.Consume().Result()
	if isErr {
		types.ResponseOutparamSet(responseOutParam, cm.Err[cm.Result[types.ErrorCodeShape, types.OutgoingResponse, types.ErrorCode]](types.ErrorCodeInternalError(cm.Some("Request body was read multiple times"))))
		return
	}

	incomingBodyStream, _, isErr := incomingBody.Stream().Result()
	if isErr {
		incomingBody.ResourceDrop()
		types.ResponseOutparamSet(responseOutParam, cm.Err[cm.Result[types.ErrorCodeShape, types.OutgoingResponse, types.ErrorCode]](types.ErrorCodeInternalError(cm.Some("Request body stream was read multiple times"))))
		return
	}

	var result bytes.Buffer
	const chunkSize = 1024
	for {
		readResult, _, _ := incomingBodyStream.BlockingRead(chunkSize).Result()

		if readResult.Len() == 0 {
			break
		}

		result.Write(readResult.Slice())
	}

	incomingBodyStream.ResourceDrop()
	incomingBody.ResourceDrop()

	var data Data
	if err := json.Unmarshal(result.Bytes(), &data); err != nil {
		sendResponse(responseOutParam, 400, "text/plain", "Incoming request body was malformed or missing")
		return
	}

	env, exists := os.LookupEnv("HELLO_MESSAGE")
	if !exists {
		sendResponse(responseOutParam, 400, "text/plain", "No environment variable with the name 'HELLO_MESSAGE' was found")
		return
	}

	sendResponse(responseOutParam, 200, "text/plain", fmt.Sprintf("%s %s", env, data.Name))
}

func sendResponse(responseOutParam types.ResponseOutparam, statuscode int, contentType string, message string) {
	// Set headers
	headers := types.NewFields()
	header := []byte(contentType)
	_, _, isErr := headers.Append("Content-Type", types.FieldValue(cm.NewList(&header[0], len(header)))).Result()
	if isErr {
		headers.ResourceDrop()
		types.ResponseOutparamSet(responseOutParam, cm.Err[cm.Result[types.ErrorCodeShape, types.OutgoingResponse, types.ErrorCode]](types.ErrorCodeInternalError(cm.Some("Failed to append headers"))))
		return
	}

	// Create response
	response := types.NewOutgoingResponse(headers)
	response.SetStatusCode(types.StatusCode(statuscode))

	// Write and finalize body
	body, _, isErr := response.Body().Result()
	if isErr {
		response.ResourceDrop()
		headers.ResourceDrop()
		types.ResponseOutparamSet(responseOutParam, cm.Err[cm.Result[types.ErrorCodeShape, types.OutgoingResponse, types.ErrorCode]](types.ErrorCodeInternalError(cm.Some("Body was read multiple times"))))
		return
	}

	bodyStream, _, isErr := body.Write().Result()
	if isErr {
		body.ResourceDrop()
		response.ResourceDrop()
		headers.ResourceDrop()
		types.ResponseOutparamSet(responseOutParam, cm.Err[cm.Result[types.ErrorCodeShape, types.OutgoingResponse, types.ErrorCode]](types.ErrorCodeInternalError(cm.Some("Body stream was read multiple times"))))
		return
	}

	bodyContent := []byte(message)
	_, _, isErr = bodyStream.Write(cm.NewList(&bodyContent[0], len(bodyContent))).Result()
	if isErr {
		body.ResourceDrop()
		response.ResourceDrop()
		headers.ResourceDrop()
		types.ResponseOutparamSet(responseOutParam, cm.Err[cm.Result[types.ErrorCodeShape, types.OutgoingResponse, types.ErrorCode]](types.ErrorCodeInternalError(cm.Some("Attempted to write to a closed stream"))))
		return
	}

	bodyStream.ResourceDrop()

	_, _, isErr = types.OutgoingBodyFinish(body, cm.None[types.Trailers]()).Result()
	if isErr {
		body.ResourceDrop()
		response.ResourceDrop()
		headers.ResourceDrop()
		types.ResponseOutparamSet(responseOutParam, cm.Err[cm.Result[types.ErrorCodeShape, types.OutgoingResponse, types.ErrorCode]](types.ErrorCodeInternalError(cm.Some("Attempted to write to a closed stream"))))
		return
	}

	// Send the response
	types.ResponseOutparamSet(responseOutParam, cm.OK[cm.Result[types.ErrorCodeShape, types.OutgoingResponse, types.ErrorCode]](response))
}
