/// <reference path="./wasi-io-streams.d.ts" />
declare module 'wasi:cli/stdout@0.2.1' {
  export function getStdout(): OutputStream;
  export type OutputStream = import('wasi:io/streams@0.2.1').OutputStream;
}
