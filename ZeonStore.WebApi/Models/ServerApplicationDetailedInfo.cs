using System.Text.Json.Serialization;
using ZeonStore.Common;

namespace ZeonStore.WebApi.Models
{
    public record ServerApplicationDetailedInfo : ApplicationDetailedInfo
    {
        [JsonIgnore]
        public Publisher Publisher { get; init; }
    }
}
