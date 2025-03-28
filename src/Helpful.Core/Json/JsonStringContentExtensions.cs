namespace Helpful.Api.Json;

using System.Text;

using Newtonsoft.Json;

/// <summary>
/// Расширение для преобразования из объекта в Json обертнутый в StringContent, и обратно.
/// </summary>
public static class JsonStringContentExtensions
{
    public static string ToJsonString(this object value)
    {
        return JsonConvert.SerializeObject(value);
    }
    
    public static StringContent ToJsonStringContent(this object value)
    {
        return new StringContent(JsonConvert.SerializeObject(value),
            Encoding.UTF8,
            "application/json");
    }

    public static async Task<T> FromJsonStringContent<T>(this HttpContent content)
    {
        var json = await content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json, JsonUtils.GetReferencedSettings())!;
    }
    
    public static async Task<T> FromJsonStringContent<T>(this Stream stream)
    {
        var json = await new StreamReader(stream).ReadToEndAsync();
        return JsonConvert.DeserializeObject<T>(json, JsonUtils.GetReferencedSettings())!;
    }
}