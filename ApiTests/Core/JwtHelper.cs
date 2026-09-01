using System.Text;
using System.Text.Json;

namespace ApiTests.Core;

public static class JwtHelper
{
    public static DateTime GetExpiry(string token)
    {
        string payloadSegment = token.Split('.')[1];
        string json = Encoding.UTF8.GetString(Base64UrlDecode(payloadSegment));

        using var doc = JsonDocument.Parse(json);
        long exp = doc.RootElement.GetProperty("exp").GetInt64();

        return DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
    }

    private static byte[] Base64UrlDecode(string input)
    {
        string padded = input.Replace('-', '+').Replace('_', '/');
        switch (padded.Length % 4)
        {
            case 2: padded += "=="; break;
            case 3: padded += "="; break;
        }
        return Convert.FromBase64String(padded);
    }
}