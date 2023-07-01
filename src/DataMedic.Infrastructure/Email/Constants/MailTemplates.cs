namespace DataMedic.Infrastructure.Email.Constants;

public static class MailTemplates
{
    private const string BasePath = "../DataMedic.Infrastructure/Email/Templates";

    public static string Welcome => File.ReadAllText(Path.Combine(BasePath, "welcome.mjml"));
}