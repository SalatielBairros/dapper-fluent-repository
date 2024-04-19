using Example.Multiverse.Application.Helpers;
using System;
using System.Text.Json.Serialization;

namespace Dapper.Fluent.Application;

public class UsePeak
{
    [JsonPropertyName("mdmId")]
    public string Id { get; set; }

    [JsonPropertyName("tcode")]
    public string TCode { get; set; }

    [JsonPropertyName("tenantid")]
    public string TenantId { get; set; }

    [JsonPropertyName("peakdate")]
    public DateTime PeakDate { get; set; }

    [JsonPropertyName("slotid")]
    public int SlotId { get; set; }

    [JsonPropertyName("eventprocessed")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime EventProcessedDate { get; set; }

    [JsonPropertyName("peak_uso")]
    public int UsePeakTotal { get; set; }

    [JsonPropertyName("totallicenses")]
    public int TotalLicenses { get; set; }

    [JsonPropertyName("usedall")]
    public bool UsedAllLicenses { get; set; }

    [JsonIgnore]
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
}
