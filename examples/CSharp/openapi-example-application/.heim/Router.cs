using System.Text;
using ProxyWorld.wit.imports.wasi.http.v0_2_0;
using ProxyWorld.wit.imports.wasi.io.v0_2_0;
using DemoGeolocationWorld.wit.exports.heim.demoGeolocation.v1_0_0;
using Import = DemoGeolocationWorld.wit.imports.heim.demoGeolocation.v1_0_0;
namespace ProxyWorld.wit.exports.wasi.http.v0_2_0;

public class IncomingHandlerImpl: IIncomingHandler
{
    public static void Handle(ITypes.IncomingRequest request, ITypes.ResponseOutparam responseOut)
    {
        var (path_without_query, method_string, queryParams) = GetPathAndMethod(ref request);

        var content = Encoding.ASCII.GetBytes($"Not found, path => {path_without_query} method => {method_string}");
        var content_type = "text/plain";
        ushort response_code = 404;

        switch((path_without_query, method_string))
        {
            case ("/MyProjectPath/extract-geo-location", "post"): 
                ExecuteHandler(() => {
                    string Authorization = Encoding.UTF8.GetString(request.Headers().Get("Authorization").SelectMany(b => b).ToArray());
                    var extractGeoLocationRequestBody = JsonHelper.Deserialize<Import.ITypes.ExtractGeoLocationRequestBody>(ParseBodyAsString(request));

                    var res = GeoLocationImpl.ExtractGeoLocation(Authorization, extractGeoLocationRequestBody);
                    if (res.IsOk) {
                        switch (res.Tag) {
                            case Import.ITypes.ExtractGeoLocationOk.Tags.Ok:
                                response_code = 200;
                                content_type = "application/json";
                                content = Encoding.ASCII.GetBytes(JsonHelper.Serialize(res.AsOk.AsOk));
                                break;

                        }
                    } else {
                        switch (res.Tag) {
                            case Import.ITypes.ExtractGeoLocationError.Tags.InternalServerError:
                                response_code = 500;
                                content_type = "text/plain";
                                content = Encoding.ASCII.GetBytes("500 Internal server error");
                                break;

                        }
                    }
                }, ref response_code, ref content, ref content_type, ref responseOut);
                break;
            case ("/MyProjectPath/get-country", "post"): 
                ExecuteHandler(() => {
                    string Authorization = Encoding.UTF8.GetString(request.Headers().Get("Authorization").SelectMany(b => b).ToArray());
                    var getCountryFromCoordinatesRequestBody = JsonHelper.Deserialize<Import.ITypes.Coordinates>(ParseBodyAsString(request));

                    var res = GeoLocationImpl.GetCountryFromCoordinates(Authorization, getCountryFromCoordinatesRequestBody);
                    if (res.IsOk) {
                        switch (res.Tag) {
                            case Import.ITypes.GetCountryFromCoordinatesOk.Tags.Ok:
                                response_code = 200;
                                content_type = "application/json";
                                content = Encoding.ASCII.GetBytes(JsonHelper.Serialize(res.AsOk.AsOk));
                                break;

                        }
                    } else {
                        switch (res.Tag) {
                            case Import.ITypes.GetCountryFromCoordinatesError.Tags.InternalServerError:
                                response_code = 500;
                                content_type = "text/plain";
                                content = Encoding.ASCII.GetBytes("500 Internal server error");
                                break;

                        }
                    }
                }, ref response_code, ref content, ref content_type, ref responseOut);
                break;
            case ("/MyProjectPath/sun-times", "post"): 
                ExecuteHandler(() => {
                    string Authorization = Encoding.UTF8.GetString(request.Headers().Get("Authorization").SelectMany(b => b).ToArray());
                    var getSunTimesRequestBody = JsonHelper.Deserialize<Import.ITypes.SunTimesRequest>(ParseBodyAsString(request));

                    var res = GeoLocationImpl.GetSunTimes(Authorization, getSunTimesRequestBody);
                    if (res.IsOk) {
                        switch (res.Tag) {
                            case Import.ITypes.GetSunTimesOk.Tags.Ok:
                                response_code = 200;
                                content_type = "application/json";
                                content = Encoding.ASCII.GetBytes(JsonHelper.Serialize(res.AsOk.AsOk));
                                break;

                        }
                    } else {
                        switch (res.Tag) {
                            case Import.ITypes.GetSunTimesError.Tags.InternalServerError:
                                response_code = 500;
                                content_type = "text/plain";
                                content = Encoding.ASCII.GetBytes("500 Internal server error");
                                break;

                        }
                    }
                }, ref response_code, ref content, ref content_type, ref responseOut);
                break;

            default:
                ExecuteHandler(() => {
                    // Not found
                }, ref response_code, ref content, ref content_type, ref responseOut); 
                break;
        }
    }

    private static string ParseBodyAsString(ITypes.IncomingRequest request) 
    {
        ITypes.IncomingBody body = request.Consume().AsOk;
        IStreams.InputStream stream = body.Stream().AsOk; 
        
        ulong len = 4096;
        var result = new List<byte>();
        byte[] buffer;

        do
        {
            buffer = stream.BlockingRead(len).AsOk;
            result.AddRange(buffer);
        } while (buffer.Length == (int)len);

        if (result.Count == 0) {
            return "";
        }
        return Encoding.UTF8.GetString(result.ToArray());
    }

    private static void ExecuteHandler(
        Action codeBlock, 
        ref ushort response_code, 
        ref byte[] content, 
        ref string content_type,
        ref ITypes.ResponseOutparam responseOut)
    {
        try 
        {
            codeBlock();
        }
        catch(Exception ex)
        {
            Console.Error.WriteLine($"Internal server error: {ex.Message}");
            response_code = 500;
            content = Encoding.UTF8.GetBytes("Internal server error");
            content_type = "text/plain";
        }
        finally {
            var headers = new List<(string, byte[])> {
                ("Content-Type", Encoding.UTF8.GetBytes(content_type)),
                ("Content-Length", Encoding.UTF8.GetBytes(content.Count().ToString()))
            };
            var responseHeaders = ITypes.Fields.FromList(new List<(string, byte[])>()).AsOk;
            try {
                responseHeaders = ITypes.Fields.FromList(headers).AsOk;
            } catch(Exception) {}

            var response = new ITypes.OutgoingResponse(responseHeaders);
            response.SetStatusCode(response_code);
            var body = response.Body().AsOk;
            ITypes.ResponseOutparam.Set(responseOut, Result<ITypes.OutgoingResponse, ITypes.ErrorCode>.Ok(response));
            using (var stream = body.Write().AsOk) {
                stream.BlockingWriteAndFlush(content);
            }
            ITypes.OutgoingBody.Finish(body, null);
        }
    }

    private static (string, string, Dictionary<string, string?>?) GetPathAndMethod(ref ITypes.IncomingRequest request)
    {
        ITypes.Method method = request.Method();
        #pragma warning disable CS8602 // Possible null reference argument.
        var schema = request.Scheme().Tag switch {
            ITypes.Scheme.Tags.Http => "http://",
            ITypes.Scheme.Tags.Https => "https://",
            _ => "",
        };
        #pragma warning restore CS8602 // Possible null reference argument.
        var uri = new Uri(schema + request.Authority()+request.PathWithQuery());
        
        var path_without_query = uri.GetLeftPart(UriPartial.Path).Replace(schema + request.Authority(), "");
        var queryParams = uri.Query.TrimStart('?').Split('&').Select(q => q.Split('=')).ToDictionary(kv => kv[0], kv => kv.Length > 1 ? kv[1] : null);

        var method_string = method.Tag switch
            {
                ITypes.Method.Tags.Connect => "connect",
                ITypes.Method.Tags.Delete => "delete",
                ITypes.Method.Tags.Get => "get",
                ITypes.Method.Tags.Head => "head",
                ITypes.Method.Tags.Options => "options",
                ITypes.Method.Tags.Patch => "patch",
                ITypes.Method.Tags.Post => "post",
                ITypes.Method.Tags.Put => "put",
                ITypes.Method.Tags.Trace => "trace",
                _ => ""
            };

        return (path_without_query,method_string, queryParams);
    }

    static T? TryCatch<T>(Func<T> func) where T : class
    {
        try
        {
            return func();
        }
        catch
        {
            return null;
        }
    }
}

public static class TypeParser
{
    public static T? Parse<T>(Func<string> getValue) where T : struct
    {
        if (getValue == null) throw new ArgumentNullException(nameof(getValue));

        try
        {
            string input = getValue.Invoke();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            return (T)Convert.ChangeType(input, typeof(T));
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static T ParseNonNullable<T>(Func<string> getValue) where T : struct
    {
        if (getValue == null) throw new ArgumentNullException(nameof(getValue));

        try
        {
            string input = getValue.Invoke();

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input), $"Input cannot be null or empty for type {typeof(T).Name}");
            }

            return (T)Convert.ChangeType(input, typeof(T));
        }
        catch (Exception)
        {
            throw new FormatException($"Unable to parse the type {typeof(T).Name}");
        }
    }
}
