// Code generated by wit-bindgen-go. DO NOT EDIT.

package ioerror

// This file contains wasmimport and wasmexport declarations for "wasi:io@0.2.6".

//go:wasmimport wasi:io/error@0.2.6 [resource-drop]error
//go:noescape
func wasmimport_ErrorResourceDrop(self0 uint32)

//go:wasmimport wasi:io/error@0.2.6 [method]error.to-debug-string
//go:noescape
func wasmimport_ErrorToDebugString(self0 uint32, result *string)
