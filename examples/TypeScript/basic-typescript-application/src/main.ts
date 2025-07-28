import {
  IncomingRequest,
  ResponseOutparam,
  OutgoingBody,
  OutgoingResponse,
  Fields,
} from "wasi:http/types@0.2.1";

export const incomingHandler = {
  async handle(
    incomingRequest: IncomingRequest,
    responseOutparam: ResponseOutparam
  ) {
    // Start building an outgoing response
    const outgoingResponse = new OutgoingResponse(new Fields());

    // Access the outgoing response body
    let outgoingBody = outgoingResponse.body();
    {
      // Create a stream for the response body
      let outputStream = outgoingBody.write();
      // Write hello world to the response stream
      const json = await (
        await fetch(`https://dummyjson.com/recipes/1`)
      ).json();

      outputStream.blockingWriteAndFlush(
        new Uint8Array(new TextEncoder().encode(JSON.stringify(json)))
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
