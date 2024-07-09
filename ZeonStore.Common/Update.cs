using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ZeonStore.Common
{
    public record Update
    {
        [Key]
        public required int Id { get; init; }

        public required string Version { get; init; }

        public required DateTime ReleaseDate { get; init; }

        [ForeignKey("InstallId")]
        public required IEnumerable<DownloadFileInfo> InstallFiles { get; init; } = new List<DownloadFileInfo>();

        [ForeignKey("UpdateId")]
        public required IEnumerable<DownloadFileInfo> UpdateFiles { get; init; } = new List<DownloadFileInfo>();
    }
}
