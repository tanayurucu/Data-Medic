using Newtonsoft.Json;

namespace DataMedic.Worker.Jobs.KafkaScanner.Models;

public class KafkaMessageDetail
{
    public string Topic { get; set; }
    public long Offset { get; set; }
    [JsonProperty("receiveTime")]
    public DateTime RecieveTime { get; set; }
    [JsonProperty("timeToLiveInSeconds")]
    public TimeSpan TimeToLiveInSeconds { get; set; }
    public string? LastMessageRecieved { get; set; }
    public bool Status { get; set; }
}