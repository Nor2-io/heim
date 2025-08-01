import {
  IncomingRequest,
  ResponseOutparam,
  OutgoingBody,
  OutgoingResponse,
  OutputStream,
  Fields,
} from "wasi:http/types@0.2.1";

// @ts-ignore
import { fileServer } from './embed.js';

export const incomingHandler = {
  async handle(
    incomingRequest: IncomingRequest,
    responseOutparam: ResponseOutparam
  ) {
    const basePrefix = '{{http_trigger_path}}'.replace("*", "");
    const rawPath = incomingRequest.pathWithQuery?.() ?? '/';
    let path = rawPath.startsWith(basePrefix) ? rawPath.slice(basePrefix.length) : rawPath;
    path = (!path || path === '/') ? 'index.html' : path.replace(/^\/+/, '');

    const content = getFileContent(fileServer, path);

    if (!content) {
      const outgoingResponse = new OutgoingResponse(new Fields());

      let outgoingBody = outgoingResponse.body();
      {
        let outputStream = outgoingBody.write();
        outputStream.blockingWriteAndFlush(
          new Uint8Array(new TextEncoder().encode("404 No content found"))
        );
        // @ts-ignore: This is required in order to dispose the stream before we return
        outputStream[Symbol.dispose]();
      }
      outgoingResponse.setStatusCode(404);
      ResponseOutparam.set(outgoingResponse, {
        tag: "ok",
        val: outgoingResponse,
      });
    }
    else {
      const headers = new Fields();
      headers.set("Content-Type", [new TextEncoder().encode(guessMimeType(path))]);

      const outgoingResponse = new OutgoingResponse(headers);
      const outgoingBody = outgoingResponse.body();
      const stream = outgoingBody.write();
      await writeStreamBody(stream, content);

      outgoingResponse.setStatusCode(200);

      outgoingResponse.headers

      ResponseOutparam.set(outgoingResponse, {
        tag: "ok",
        val: outgoingResponse,
      });
    }
  }
}

function guessMimeType(path: string): string {
  if (path.endsWith('.html') || path.endsWith('.htm')) return 'text/html';
  if (path.endsWith('.js')) return 'application/javascript';
  if (path.endsWith('.css')) return 'text/css';
  if (path.endsWith('.json')) return 'application/json';
  if (path.endsWith('.svg')) return 'image/svg+xml';
  if (path.endsWith('.png')) return 'image/png';
  if (path.endsWith('.jpg') || path.endsWith('.jpeg')) return 'image/jpeg';
  if (path.endsWith('.wasm')) return 'application/wasm';
  if (path.endsWith('.xml')) return 'application/xml';
  if (path.endsWith('.pdf')) return 'application/pdf';
  return 'application/octet-stream';
}

async function writeStreamBody(stream: OutputStream, body: Uint8Array) {
  const pollable = stream.subscribe();
  let offset = 0;

  while (offset < body.length) {
    await pollable.block();

    const permitted = Number(stream.checkWrite());
    if (permitted === 0) continue;

    const remaining = body.length - offset;
    const len = Math.min(permitted, remaining);
    const chunk = body.slice(offset, offset + len);

    stream.write(chunk);

    offset += len;
  }

  stream.flush();
  await pollable.block();
  stream.checkWrite();
  //@ts-ignore
  pollable[Symbol.dispose]?.();
  //@ts-ignore
  stream[Symbol.dispose]?.();
}

type EmbeddedFile =
  | { type: 'text'; content: string }
  | { type: 'binary'; content: string };

export function getFileContent(
  files: Record<string, EmbeddedFile>,
  path: string
): Uint8Array {
  const file = files[path];
  if (!file) throw new Error(`File not found: ${path}`);

  if (file.type === 'text') {
    return new TextEncoder().encode(file.content);
  }
  if (file.type === 'binary') {
    return new Uint8Array(Array.from(atob(file.content), c => c.charCodeAt(0)))
  }

  throw new Error(`Unsupported file type for ${path}`);
}