
namespace BuberDinner.Infrastructure.Services.SMTP.Model;

public class WelcomeEmail
{
    public string UserName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = "Buber Dinner"!;
    public string ConfirmLink { get; set; } = string.Empty;
    public string SupportEmail { get; set; } = "suporte@buberdinner.com";
    public string SupportPhone { get; set; } = "+55 (47) 99999-9999";
    public string CompanyAddress { get; set; } = "Av. das Flores, 1000 - Balneário  Camboriú, SP";
    public string TermsUrl { get; set; } = "#";
    public string PrivacyUrl { get; set; } = "#";
    public string HelpUrl { get; set; } = "#";
    public string SupportUrl { get; set; } = "#";
}
