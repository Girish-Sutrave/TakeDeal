using Microsoft.EntityFrameworkCore;

namespace TakeDeal.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ListingImage> ListingImages { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Category>().HasOne(c=>c.ParentCategory).WithMany(c=>c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);

            // Listing → User
            modelBuilder.Entity<Listing>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)   // ✅ CORRECT
                .OnDelete(DeleteBehavior.Cascade);


            // Listing → Category
            modelBuilder.Entity<Listing>()
                .HasOne(l => l.Category)
                .WithMany()
                .HasForeignKey(l => l.CategoryId)   // ✅ CORRECT
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
