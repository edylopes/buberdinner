namespace BuberDinner.Api.Helpers;

internal static class HeaderSanitizer
{
    public static string Sanitizer(string value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;

        // Remove caracteres de controle e quebras de linha
        var sanitized = value
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty);

        // Opcional: codificar caracteres especiais (HTML/XSS)
        sanitized = System.Net.WebUtility.HtmlEncode(sanitized);

        return sanitized;
    }
}

