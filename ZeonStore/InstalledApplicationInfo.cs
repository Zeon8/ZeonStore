using System;

namespace ZeonStore
{
    public record InstalledApplicationInfo
    {
        public InstalledApplicationInfo(int id, string version, DateTime releaseDate, string? exebutableFile)
        {
            Id = id;
            Version = version;
            ReleaseDate = releaseDate;
            ExebutableFile = exebutableFile;
        }

        public int Id { get; init; }

        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string? ExebutableFile { get; set; }
    }
}
