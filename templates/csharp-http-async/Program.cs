using System.Text;
using ProxyWorld;
using ProxyWorld.wit.exports.wasi.http.v0_2_0;
using static ProxyWorld.wit.imports.wasi.http.v0_2_0.ITypes;

namespace ProxyWorld.wit.exports.wasi.http.v0_2_0;

public class IncomingHandlerImpl : IIncomingHandler
{
    public static void Handle(IncomingRequest request, ResponseOutparam responseOut)
    {
        Console.WriteLine("IncomingHandlerImpl.Handle");

        Func<Task> task = async () =>
        {
            await HandleAsync(request, responseOut);
        };

        WasiEventLoop.PollWasiEventLoopUntilResolved(task());

        Console.WriteLine("IncomingHandlerImpl.Handle done");
    }

    public static async Task HandleAsync(IncomingRequest request, ResponseOutparam responseOut)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync("https://dummyjson.com/recipes/1");
            var responseHeaders = new List<(string, byte[])>
            {
                ("content-type", Encoding.UTF8.GetBytes("application/json")),
                ("accept", Encoding.UTF8.GetBytes("*"))
            };
            SendResponse(responseOut, responseHeaders, Encoding.UTF8.GetBytes(await response.Content.ReadAsStringAsync()));
        }
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
