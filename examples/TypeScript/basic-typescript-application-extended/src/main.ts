import {
  IncomingRequest,
  ResponseOutparam,
  OutgoingBody,
  OutgoingResponse,
  Fields,
} from "wasi:http/types@0.2.1";


import { getEnvironment } from "wasi:cli/environment@0.2.1";

export const incomingHandler = {
  async handle(
    incomingRequest: IncomingRequest,
    responseOutparam: ResponseOutparam
  ) {
    // Get a readable stream from the incoming request
    const request = incomingRequest.consume();
    const requestStream = request.stream();
    const pollable = requestStream.subscribe();

    // Read the stream as chunks
    let chunks: Uint8Array[] = [];
    while (true) {
      await pollable.block();
      try {
        const chunk = requestStream.read(BigInt(1024));
        if (chunk.length === 0) {
          continue;
        }
        chunks.push(chunk);
      } catch (err) {
        // EOF is an error
        break;
      }
    }

    // Combine the chunks to a Uint8Array
    const totalLength = chunks.reduce((sum, chunk) => sum + chunk.length, 0);
    const fullBody = new Uint8Array(totalLength);

    let offset = 0;
    for (const chunk of chunks) {
      fullBody.set(chunk, offset);
      offset += chunk.length;
    }

    // Parse the body as a string
    const text = new TextDecoder().decode(fullBody);
    const json = JSON.parse(text);

    // Retrieve the environment variable we specified in component.toml and set the value for in application.toml
    const env_var = getEnvironment().find(([key]) => key == "HELLO_MESSAGE")?.[1];

    // Start building an outgoing response
    const outgoingResponse = new OutgoingResponse(Fields.fromList([["Content-Type", new TextEncoder().encode("application/json")]]));

    // Access the outgoing response body
    let outgoingBody = outgoingResponse.body();
    {
      // Create a stream for the response body
      let outputStream = outgoingBody.write();

      // Read the body and get env variable

      outputStream.blockingWriteAndFlush(
        new Uint8Array(new TextEncoder().encode(JSON.stringify(`${json.message}, ${env_var}!`)))
      );
      // @ts-ignore: This is required in order to dispose the stream before we return
      outputStream[Symbol.dispose]();
    }

    // Set the status code for the response
    outgoingResponse.setStatusCode(200);
    // Finish the response body
    OutgoingBody.finish(outgoingBody, undefined);
    // Set the created response to an "OK" Result<T> value
    ResponseOutparam.set(outgoingResponse, {
      tag: "ok",
      val: outgoingResponse,
    });
  },
};