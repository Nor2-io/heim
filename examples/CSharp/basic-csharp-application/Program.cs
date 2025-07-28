using System.Text;
using static ProxyWorld.wit.imports.wasi.http.v0_2_0.ITypes;

namespace ProxyWorld.wit.exports.wasi.http.v0_2_0;

public class IncomingHandlerImpl : IIncomingHandler
{
    public static void Handle(IncomingRequest request, ResponseOutparam responseOut)
    {
        var content = Encoding.ASCII.GetBytes("Hello, World!");
        var headers = new List<(string, byte[])> {
            ("content-type", Encoding.ASCII.GetBytes("text/plain")),
            ("content-length", Encoding.ASCII.GetBytes(content.Length.ToString()))
        };

        SendResponse(responseOut, headers, content);
    }

    public static void SendResponse(ResponseOutparam responseOut, List<(string, byte[])> headers, byte[] bodyBytes)
    {
        // This handling is neeeded, because of an bug in Wit-bindgen for dotnet.
        // https://github.com/bytecodealliance/wit-bindgen/pull/1215
        var responseHeaders = Fields.FromList(new List<(string, byte[])>()).AsOk;
        try {
            responseHeaders = Fields.FromList(headers).AsOk;
        } catch(Exception) {}

        var response = new OutgoingResponse(responseHeaders);
        response.SetStatusCode(200);
        var body = response.Body().AsOk;

        ResponseOutparam.Set(responseOut, Result<OutgoingResponse, ErrorCode>.Ok(response));
        using (var stream = body.Write().AsOk) {
            stream.BlockingWriteAndFlush(bodyBytes);
        }

        OutgoingBody.Finish(body, null);
    }
}

