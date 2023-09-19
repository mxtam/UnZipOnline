﻿using Microsoft.EntityFrameworkCore;

namespace UnZipOnline.Models
{
    public class UnZipContext:DbContext
    {
        public DbSet<FileModel> Files { get; set; }
        public DbSet<ExtractedModel> Extracted { get; set; }

        public UnZipContext(DbContextOptions<UnZipContext> options)
            :base(options) 
        {
        }
    }
}
