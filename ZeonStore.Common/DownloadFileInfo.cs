using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZeonStore.Common
{
    public record DownloadFileInfo
    {
        [Key]
        public required int Id { get; init; }

        public required string DownloadUrl { get; init; }

        public required string Platform { get; init; }

        public required string ExebutableName { get; init; }
    }
}
