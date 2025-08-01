package main

import (
	incominghandler "go-http/wasi/http/incoming-handler"
	"go-http/wasi/http/types"
	"net/http"

	"go.bytecodealliance.org/cm"
)

func init() {
	incominghandler.Exports.Handle = Handle
}

func main() {}

func Handle(request types.IncomingRequest, responseOutParam types.ResponseOutparam) {
	// Set headers
	headers := types.NewFields()
	header := []byte("text/plain")
	_, _, isErr := headers.Append("Content-Type", types.FieldValue(cm.NewList(&header[0], len(header)))).Result()
	if isErr {
		headers.ResourceDrop()
		types.ResponseOutparamSet(responseOutParam, cm.Err[cm.Result[types.ErrorCodeShape, types.OutgoingResponse, types.ErrorCode]](types.ErrorCodeInternalError(cm.Some("Failed to append headers"))))
		return
	}

	// Create response
	response := types.NewOutgoingResponse(headers)
	response.SetStatusCode(types.StatusCode(http.StatusOK))

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

	bodyContent := []byte("Hello world!")
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
