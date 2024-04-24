using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UnZipOnline.Models
{
    public class Archives
    {
        public int Id { get; set; }
        public string? Name { get; set; } 
        public string? ContentType { get; set; }
        public DateTime LastModified { get; set; }

        public List<Files>? Files { get; set; }
    }
}
