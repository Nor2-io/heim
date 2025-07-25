#pragma warning disable CS8603
#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8629
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Import = DemoGeolocationWorld.wit.imports.heim.demoGeolocation.v1_0_0;

namespace DemoGeolocationWorld.wit.exports.heim.demoGeolocation.v1_0_0;

[JsonSerializable(typeof(Import.ITypes.ExtractGeoLocationRequestBody))]
[JsonSerializable(typeof(Import.ITypes.GeoLocationResponse))]
[JsonSerializable(typeof(Import.ITypes.Coordinates))]
[JsonSerializable(typeof(Import.ITypes.CountryResponse))]
[JsonSerializable(typeof(Import.ITypes.SunTimesRequest))]
[JsonSerializable(typeof(Import.ITypes.SunTimesResponse))]
internal partial class MyJsonContext : JsonSerializerContext
{
}
public class ExtractGeoLocationRequestBodyConverter : JsonConverter<Import.ITypes.ExtractGeoLocationRequestBody>
{
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override Import.ITypes.ExtractGeoLocationRequestBody Read(ref Utf8JsonReader reader, Type typeToConverter, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
        string image = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Import.ITypes.ExtractGeoLocationRequestBody(image);
            }
            else
            {
                string propertyName = reader.GetString();
                reader.Read(); 
                switch (propertyName)
                {
                    case "image":
                        image = reader.GetString();
                        break;
                    default:
                        reader.Skip();
                        break;

                }
            }
        }
        throw new JsonException();

    }
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override void Write(Utf8JsonWriter writer, Import.ITypes.ExtractGeoLocationRequestBody value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("image", value.image);

        writer.WriteEndObject();

    }
}
public class GeoLocationResponseConverter : JsonConverter<Import.ITypes.GeoLocationResponse>
{
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override Import.ITypes.GeoLocationResponse Read(ref Utf8JsonReader reader, Type typeToConverter, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
        double? latitude = null;
        double? longitude = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Import.ITypes.GeoLocationResponse(latitude, longitude);
            }
            else
            {
                string propertyName = reader.GetString();
                reader.Read(); 
                switch (propertyName)
                {
                    case "latitude":
                        latitude = reader.GetDouble();
                        break;
                    case "longitude":
                        longitude = reader.GetDouble();
                        break;
                    default:
                        reader.Skip();
                        break;

                }
            }
        }
        throw new JsonException();

    }
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override void Write(Utf8JsonWriter writer, Import.ITypes.GeoLocationResponse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("latitude", (double)value.latitude);


        writer.WriteNumber("longitude", (double)value.longitude);


        writer.WriteEndObject();

    }
}
public class CoordinatesConverter : JsonConverter<Import.ITypes.Coordinates>
{
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override Import.ITypes.Coordinates Read(ref Utf8JsonReader reader, Type typeToConverter, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
        double latitude = 0;
        double longitude = 0;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Import.ITypes.Coordinates(latitude, longitude);
            }
            else
            {
                string propertyName = reader.GetString();
                reader.Read(); 
                switch (propertyName)
                {
                    case "latitude":
                        latitude = reader.GetDouble();
                        break;
                    case "longitude":
                        longitude = reader.GetDouble();
                        break;
                    default:
                        reader.Skip();
                        break;

                }
            }
        }
        throw new JsonException();

    }
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override void Write(Utf8JsonWriter writer, Import.ITypes.Coordinates value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("latitude", (double)value.latitude);

        writer.WriteNumber("longitude", (double)value.longitude);

        writer.WriteEndObject();

    }
}
public class CountryResponseConverter : JsonConverter<Import.ITypes.CountryResponse>
{
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override Import.ITypes.CountryResponse Read(ref Utf8JsonReader reader, Type typeToConverter, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
        string? country = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Import.ITypes.CountryResponse(country);
            }
            else
            {
                string propertyName = reader.GetString();
                reader.Read(); 
                switch (propertyName)
                {
                    case "country":
                        country = reader.GetString();
                        break;
                    default:
                        reader.Skip();
                        break;

                }
            }
        }
        throw new JsonException();

    }
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override void Write(Utf8JsonWriter writer, Import.ITypes.CountryResponse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("country", value.country);


        writer.WriteEndObject();

    }
}
public class SunTimesRequestConverter : JsonConverter<Import.ITypes.SunTimesRequest>
{
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override Import.ITypes.SunTimesRequest Read(ref Utf8JsonReader reader, Type typeToConverter, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
        Import.ITypes.Coordinates? coordinates = null;
        string? date = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Import.ITypes.SunTimesRequest(coordinates, date);
            }
            else
            {
                string propertyName = reader.GetString();
                reader.Read(); 
                switch (propertyName)
                {
                    case "coordinates":
                        coordinates = JsonSerializer.Deserialize<Import.ITypes.Coordinates>(ref reader, options);
                        break;
                    case "date":
                        date = reader.GetString();
                        break;
                    default:
                        reader.Skip();
                        break;

                }
            }
        }
        throw new JsonException();

    }
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override void Write(Utf8JsonWriter writer, Import.ITypes.SunTimesRequest value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("coordinates");
        JsonSerializer.Serialize(writer, value.coordinates, options);


        writer.WriteString("date", value.date);


        writer.WriteEndObject();

    }
}
public class SunTimesResponseConverter : JsonConverter<Import.ITypes.SunTimesResponse>
{
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override Import.ITypes.SunTimesResponse Read(ref Utf8JsonReader reader, Type typeToConverter, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
        string? sunrise = null;
        string? sunset = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Import.ITypes.SunTimesResponse(sunrise, sunset);
            }
            else
            {
                string propertyName = reader.GetString();
                reader.Read(); 
                switch (propertyName)
                {
                    case "sunrise":
                        sunrise = reader.GetString();
                        break;
                    case "sunset":
                        sunset = reader.GetString();
                        break;
                    default:
                        reader.Skip();
                        break;

                }
            }
        }
        throw new JsonException();

    }
    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public override void Write(Utf8JsonWriter writer, Import.ITypes.SunTimesResponse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("sunrise", value.sunrise);


        writer.WriteString("sunset", value.sunset);


        writer.WriteEndObject();

    }
}
public static class JsonHelper
{
    public static JsonSerializerOptions GetJsonOptions()
    {
        return new JsonSerializerOptions 
            {
                Converters = 
                {
                    new ExtractGeoLocationRequestBodyConverter(),
                    new GeoLocationResponseConverter(),
                    new CoordinatesConverter(),
                    new CountryResponseConverter(),
                    new SunTimesRequestConverter(),
                    new SunTimesResponseConverter(),
                },
                TypeInfoResolver = new MyJsonContext(),
            };
    }

    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, GetJsonOptions());
    }

    [UnconditionalSuppressMessage("AOT analysis", "IL3050::RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode",
    Justification = "Everything referenced in the loaded assembly is manually preserved, so it's safe")]
    public static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, GetJsonOptions());
    }
}

#pragma warning restore CS8629
#pragma warning restore CS8603 
#pragma warning restore CS8600 
#pragma warning restore CS8604 
