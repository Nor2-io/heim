using DemoGeolocationWorld.wit.imports.heim.demoGeolocation.v1_0_0;

namespace DemoGeolocationWorld.wit.exports.heim.demoGeolocation.v1_0_0;

public partial class GeoLocationImpl : IGeoLocation
{

    // Extract Geo-Location from Image
    public static async Task<Result<ITypes.ExtractGeoLocationOk,ITypes.ExtractGeoLocationError>> ExtractGeoLocationAsync(string Authorization, ITypes.ExtractGeoLocationRequestBody extractGeoLocationRequestBody)
    {
        // REPLACE WITH IMPLEMENTATION
        var tcs = new TaskCompletionSource<Result<ITypes.ExtractGeoLocationOk,ITypes.ExtractGeoLocationError>>();
        tcs.SetException(new NotImplementedException());
        return await tcs.Task;
    }

    // Retrieve Country Name from Coordinates
    public static async Task<Result<ITypes.GetCountryFromCoordinatesOk,ITypes.GetCountryFromCoordinatesError>> GetCountryFromCoordinatesAsync(string Authorization, ITypes.Coordinates getCountryFromCoordinatesRequestBody)
    {
        // REPLACE WITH IMPLEMENTATION
        var tcs = new TaskCompletionSource<Result<ITypes.GetCountryFromCoordinatesOk,ITypes.GetCountryFromCoordinatesError>>();
        tcs.SetException(new NotImplementedException());
        return await tcs.Task;
    }

    // Retrieve Sun Rise and Set Times
    public static async Task<Result<ITypes.GetSunTimesOk,ITypes.GetSunTimesError>> GetSunTimesAsync(string Authorization, ITypes.SunTimesRequest getSunTimesRequestBody)
    {
        // REPLACE WITH IMPLEMENTATION
        var tcs = new TaskCompletionSource<Result<ITypes.GetSunTimesOk,ITypes.GetSunTimesError>>();
        tcs.SetException(new NotImplementedException());
        return await tcs.Task;
    }

}
