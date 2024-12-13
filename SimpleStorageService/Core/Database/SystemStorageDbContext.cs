using Microsoft.EntityFrameworkCore;

namespace Core.Database
{
    public class SystemStorageDbContext : DbContext
    {
        public SystemStorageDbContext(DbContextOptions<SystemStorageDbContext> options)
         : base(options)
        { }
        public DbSet<ObjectContent> ObjectContent { get; set; }
        public DbSet<ObjectMetadata> ObjectMetadata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObjectMetadata>()
                .HasOne(fm => fm.Content)
                .WithOne()
                .HasForeignKey<ObjectContent>(fc => fc.Id);
        }
    }
}
