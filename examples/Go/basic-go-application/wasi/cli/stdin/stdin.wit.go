// Code generated by wit-bindgen-go. DO NOT EDIT.

// Package stdin represents the imported interface "wasi:cli/stdin@0.2.6".
package stdin

import (
	"go-http/wasi/io/streams"
	"go.bytecodealliance.org/cm"
)

// InputStream represents the imported type alias "wasi:cli/stdin@0.2.6#input-stream".
//
// See [streams.InputStream] for more information.
type InputStream = streams.InputStream

// GetStdin represents the imported function "get-stdin".
//
//	get-stdin: func() -> input-stream
//
//go:nosplit
func GetStdin() (result InputStream) {
	result0 := wasmimport_GetStdin()
	result = cm.Reinterpret[InputStream]((uint32)(result0))
	return
}
