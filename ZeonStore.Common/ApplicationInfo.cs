using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZeonStore.Common
{
    public record ApplicationInfo
    {
        [Key]
        public required int Id { get; init; }

        public required string Name { get; init; }

        public required string IconUrl { get; init; }

        public required string ShortDescription { get; init; }

        public required string[] Categories { get; init; } 

        [NotMapped]
        public required string PublisherName { get; set; }
    }

    public record ApplicationDetailedInfo : ApplicationInfo
    {
        public required string FullDescription { get; set; }

        public required string[] ImageUrls { get; set; }

        public required Update LatestUpdate { get; init; }

        public required IEnumerable<Update> Updates { get; init; }

    }
}
