namespace UnZipOnline.Models
{
    public class Files
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ExtractedTo { get; set; }
        public string? FullPath { get; set; }
        public DateTime? LastModified { get; set; }

        public int ArchivesId { get; set; }
        public Archives? Archives { get; set; }
    }
}
