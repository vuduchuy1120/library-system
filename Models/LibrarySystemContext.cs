using Library_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_System
{
    public class LibrarySystemContext : DbContext
    {
        public LibrarySystemContext() { }

        public LibrarySystemContext(DbContextOptions<LibrarySystemContext> options) : base(options) { }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BorrowDetail> BorrowDetails { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
        .HasOne(b => b.Author)
        .WithMany(a => a.Books)
        .HasForeignKey(b => b.AuthorId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
        .HasIndex(b => b.IsbCode)
        .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.UserName)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Phone)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(Category => Category.CategoryName)
                .IsUnique();

            modelBuilder.Entity<Publisher>()
                .HasIndex(Publisher => Publisher.PublisherName)
                .IsUnique();
        }
    }
}
