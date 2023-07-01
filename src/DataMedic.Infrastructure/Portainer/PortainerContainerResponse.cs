// Generated by https://quicktype.io

namespace DataMedic.Infrastructure.Portainer;

using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class PortainerContainerResponse
{
    [JsonProperty("Command")]
    public string Command { get; set; }

    [JsonProperty("Created")]
    public long Created { get; set; }

    [JsonProperty("HostConfig")]
    public HostConfig HostConfig { get; set; }

    [JsonProperty("Id")]
    public string Id { get; set; }

    [JsonProperty("Image")]
    public string Image { get; set; }

    [JsonProperty("ImageID")]
    public string ImageId { get; set; }

    [JsonProperty("Labels")]
    public Dictionary<string, string> Labels { get; set; }

    [JsonProperty("Names")]
    public string[] Names { get; set; }

    [JsonProperty("NetworkSettings")]
    public NetworkSettings NetworkSettings { get; set; }

    [JsonProperty("Ports")]
    public Port[] Ports { get; set; }
}

public partial class Port
{
    [JsonProperty("IP", NullValueHandling = NullValueHandling.Ignore)]
    public string Ip { get; set; }

    [JsonProperty("PrivatePort")]
    public long PrivatePort { get; set; }

    [JsonProperty("PublicPort", NullValueHandling = NullValueHandling.Ignore)]
    public long? PublicPort { get; set; }

    [JsonProperty("Type")]
    public string Type { get; set; }
}

public partial class PortainerContainerResponse
{
    public static PortainerContainerResponse[]? FromJson(string json) =>
        JsonConvert.DeserializeObject<PortainerContainerResponse[]>(
            json,
            PortainerContainerResponseConverter.Settings
        );
}

internal static class PortainerContainerResponseConverter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}
