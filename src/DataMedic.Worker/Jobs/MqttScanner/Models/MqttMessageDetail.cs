using Newtonsoft.Json;

namespace DataMedic.Worker.Jobs.MqttScanner.Models;

public class MqttMessageDetail
{
    [JsonProperty("date")]
    public DateTime Date { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
    [JsonProperty("timeToLiveInSeconds")]
    public TimeSpan TimeToLiveInSeconds { get; set; }
    public bool Status { get; set; }
    public string Topic { get; set; }
}