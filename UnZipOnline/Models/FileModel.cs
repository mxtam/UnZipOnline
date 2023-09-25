namespace UnZipOnline.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ExtractedFile { get; set; }
        public string? ExtractedFileName { get; set; }
        public string? Path { get; set; }
        public string? ContentType { get; set; }
        public string? ExtractedTo { get; set; }
        public DateTime? LastModified { get; set; }

    }
}
