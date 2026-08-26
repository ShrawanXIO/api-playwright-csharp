using System.Text.Json;

namespace ApiTests.Core;

public static class ConfigLoader
{
    public static ApiSettings Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<ApiSettings>(json)!;
    }
}