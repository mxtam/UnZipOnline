namespace UnZipOnline.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public string? ExtractedTo { get; set; }
        public DateTime? LastModified { get; set; }
        public string? ExtractedFiles { get; set; }

    }
}
