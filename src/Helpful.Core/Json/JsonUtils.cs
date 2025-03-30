namespace Helpful.Core.Json;

using Newtonsoft.Json;

/// <summary>
/// Утилита для настройки форматировщиков.
/// </summary>
public static class JsonUtils
{
    /// <summary>
    /// Получить параметры сериализации JSON в <see cref="JsonSerializerSettings" />
    /// Указываем: автоматическое распознавание типов, сохраняем ссылки на объекты,
    /// сериализуем цикличные ссылки (зависимости).
    /// </summary>
    /// <returns> Настройки Json форматировщика. </returns>
    public static JsonSerializerSettings GetReferencedSettings()
    {
        var jsonSerializerSettings = new JsonSerializerSettings();
        SetReferencedSettings(jsonSerializerSettings);

        return jsonSerializerSettings;
    }

    /// <summary>
    /// Попытка десериализации объекта.
    /// </summary>
    /// <typeparam name="T"> В какой тип десериализуется. </typeparam>
    /// <param name="value"> Значение, которе десериализуется. </param>
    /// <param name="result"> Результат десериализации. </param>
    /// <returns> True - удалось десериализовать, иначе false. </returns>
    public static bool TryDeserialize<T>(string value, out T result) where T : class, new()
    {
        try
        {
            var deserializeObject = JsonConvert.DeserializeObject<T>(value);
            result = deserializeObject ?? new T();
            return deserializeObject != null;
        }
        catch
        {
            result = new T();
            return false;
        }
    }

    /// <summary>
    /// Установить параметры сериализации JSON в <see cref="JsonSerializerSettings" />
    /// Указываем: автоматическое распознавание типов, сохраняем ссылки на объекты,
    /// сериализуем цикличные ссылки (зависимости).
    /// </summary>
    /// <param name="serializerSettings"> Настройки Json форматировщика. </param>
    public static void SetReferencedSettings(this JsonSerializerSettings serializerSettings)
    {
        // serializerSettings.TypeNameHandling = TypeNameHandling.Auto;
        // serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        // serializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
    }
}