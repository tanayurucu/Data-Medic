namespace DataMedic.Application.Sensors.Models;

public sealed class PrtgSettings
{
    public const string SectionName = nameof(PrtgSettings);
    public string PRTG_HOST { get; set; } = "";
    public string PRTG_USERNAME { get; set; } = "";
    public string PRTG_PASSWORD { get; set; } = ".";
    public string PROXY_USERNAME { get; set; } = "";
    public string PROXY_PASSWORD { get; set; } = ".";
    public string PROXY_HOST { get; set; } = "";
    public string PROXY_PORT { get; set; } = "8080";
}