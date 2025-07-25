import asyncio
import hashlib
import poll_loop
import json
import os

from proxy import exports
from proxy.types import Ok
from proxy.imports import types
from proxy.imports.types import (
    Method_Get,
    Method_Post,
    Scheme,
    Scheme_Http,
    Scheme_Https,
    Scheme_Other,
    IncomingRequest,
    ResponseOutparam,
    OutgoingResponse,
    Fields,
    OutgoingBody,
    OutgoingRequest,
)
from poll_loop import Stream, Sink, PollLoop
from typing import Tuple
from urllib import parse


class IncomingHandler(exports.IncomingHandler):
    def handle(self, request: IncomingRequest, response_out: ResponseOutparam) -> None:
        loop = PollLoop()
        asyncio.set_event_loop(loop)
        loop.run_until_complete(handle_async(request, response_out))


async def handle_async(
    request: IncomingRequest, response_out: ResponseOutparam
) -> None:
    request_stream = Stream(request.consume())
    chunks = []
    while True:
        chunk = await request_stream.next()
        if chunk is None:
            break
        chunks.append(chunk)
    body_bytes = b"".join(chunks)

    try:
        data = json.loads(body_bytes.decode("utf-8"))
        message_value = data.get("message", "")
    except Exception:
        message_value = ""

    env_var = os.getenv("HELLO_MESSAGE")
    
    response_data = json.dumps(f"{message_value}, {env_var}!").encode("utf-8")

    headers = Fields.from_list([("content-type", b"application/json")])

    response = OutgoingResponse(headers)

    response_body = response.body()

    ResponseOutparam.set(response_out, Ok(response))

    sink = Sink(response_body)
    await sink.send(response_data)
    sink.close()