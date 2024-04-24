using Microsoft.EntityFrameworkCore;

namespace UnZipOnline.Models
{
    public class UnZipContext:DbContext
    {
        public DbSet<Files> Files { get; set; }
        public DbSet<Archives> Archives { get; set; }

        public UnZipContext(DbContextOptions<UnZipContext> options)
            :base(options) 
        {
        }
    }
}
