// Code generated by wit-bindgen-go. DO NOT EDIT.

// Package run represents the imported interface "wasi:cli/run@0.2.6".
package run

import (
	"go.bytecodealliance.org/cm"
)

// Run represents the imported function "run".
//
// Run the program.
//
//	run: func() -> result
//
//go:nosplit
func Run() (result cm.BoolResult) {
	result0 := wasmimport_Run()
	result = (cm.BoolResult)((uint8)((uint32)(result0)))
	return
}
