using System.ComponentModel.DataAnnotations;

namespace ZeonStore.WebApi.Models
{
    public record Publisher
    {
        [Key]
        public required int Id { get; init; }

        public required string Name { get; init; }
    }
}
