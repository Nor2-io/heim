// Code generated by wit-bindgen-go. DO NOT EDIT.

// Package terminaloutput represents the imported interface "wasi:cli/terminal-output@0.2.6".
package terminaloutput

import (
	"go.bytecodealliance.org/cm"
)

// TerminalOutput represents the imported resource "wasi:cli/terminal-output@0.2.6#terminal-output".
//
// The output side of a terminal.
//
//	resource terminal-output
type TerminalOutput cm.Resource

// ResourceDrop represents the imported resource-drop for resource "terminal-output".
//
// Drops a resource handle.
//
//go:nosplit
func (self TerminalOutput) ResourceDrop() {
	self0 := cm.Reinterpret[uint32](self)
	wasmimport_TerminalOutputResourceDrop((uint32)(self0))
	return
}
