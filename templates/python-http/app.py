import asyncio
import hashlib
import poll_loop

from nor2_world import exports
from nor2_world.types import Ok
from nor2_world.imports import types
from nor2_world.imports.types import (
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
    response = OutgoingResponse(Fields.from_list([("content-type", b"application/json")]))

    response_body = response.body()

    ResponseOutparam.set(response_out, Ok(response))

    req = OutgoingRequest(Fields.from_list([]))
    req.set_scheme(Scheme_Https())
    req.set_authority('dummyjson.com')
    req.set_path_with_query('/recipes/1')

    resp = await poll_loop.send(req)

    stream = Stream(resp.consume())
    sink = Sink(response_body)
    while True:
        chunk = await stream.next()
        if chunk is None:
            break
        else:
            await sink.send(chunk)

    sink.close()