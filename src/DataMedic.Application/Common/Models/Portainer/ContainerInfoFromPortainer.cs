using Newtonsoft.Json;

namespace DataMedic.Application.Common.Models.Portainer;
public class ContainerInfoFromPortainer
{
    [JsonProperty("Id")]
    public string Id { get; set; }
    [JsonProperty("State")]
    public string State { get; set; }
    [JsonProperty("Names")]
    public List<string> Names { get; set; }
}