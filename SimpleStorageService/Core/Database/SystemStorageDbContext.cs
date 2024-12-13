using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Core.Database
{
    public class SystemStorageDbContext : DbContext
    {
        public SystemStorageDbContext(DbContextOptions<SystemStorageDbContext> options)
         : base(options)
        { }
        public DbSet<FileMetadata> FileMetadatas { get; set; }
        public DbSet<FileContent> FileContents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileMetadata>()
                .HasOne(fm => fm.FileContent)
                .WithOne()
                .HasForeignKey<FileContent>(fc => fc.Id);
        }
    }
}
