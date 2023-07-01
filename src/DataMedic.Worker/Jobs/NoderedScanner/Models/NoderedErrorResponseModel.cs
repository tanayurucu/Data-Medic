using Newtonsoft.Json;

namespace DataMedic.Worker.Models;

public class NoderedErrorResponseModel
{
    [JsonProperty("errorList")]
    public List<FlowErrorDetail> ErrorList { get; set; }
}

public class FlowErrorDetail
{
    [JsonProperty("error")]
    public string Error { get; set; }
    [JsonProperty("date")]
    public DateTime Date { get; set; }
    [JsonProperty("nodeId")]
    public string NodeId { get; set; }
}