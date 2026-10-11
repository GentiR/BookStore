using Microsoft.EntityFrameworkCore;
using ProCredit.BookStore.Domain;

namespace ProCredit.BookStore.Infrastructure.DbContexts;

public class BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors => Set<Author>();

    public DbSet<Book> Books => Set<Book>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("bookstore");

        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Authors");

            entity.HasKey(x => x.AuthorId);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books");

            entity.HasKey(x => x.BookId);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.SubTitle)
                .HasMaxLength(250);

            entity.HasOne<Author>()
                .WithMany()
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}