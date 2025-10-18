

namespace BuberDinner.Infrastructure.Services.SMTP;

public class EmailRenderer
{
    public string Render(string templatePath, Dictionary<string, string> values)
    {
        var html = File.ReadAllText(templatePath);
        foreach (var kv in values)
            html = html.Replace($"{{{{{kv.Key}}}}}", kv.Value);
        return html;
    }
}
