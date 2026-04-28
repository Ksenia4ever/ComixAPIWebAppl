using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Models
{
    public class ComixAPIContext : DbContext
    {
        public ComixAPIContext(DbContextOptions<ComixAPIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Comic> Comics { get; set; }
        public virtual DbSet<ComicAuthor> ComicAuthors { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ComicAuthor>()
                .HasKey(ca => new { ca.ComicId, ca.AuthorId });

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<Cart>()
                .HasIndex(c => c.AccountId)
                .IsUnique();

            modelBuilder.Entity<CartItem>()
                .HasIndex(ci => new { ci.CartId, ci.ComicId })
                .IsUnique();

            modelBuilder.Entity<Review>()
                .HasIndex(r => new { r.ComicId, r.AccountId })
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(a => a.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Cart)
                .WithOne(c => c.Account)
                .HasForeignKey<Cart>(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comic>()
                .HasOne(c => c.Genre)
                .WithMany(g => g.Comics)
                .HasForeignKey(c => c.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comic>()
                .HasOne(c => c.Publisher)
                .WithMany(p => p.Comics)
                .HasForeignKey(c => c.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComicAuthor>()
                .HasOne(ca => ca.Comic)
                .WithMany(c => c.ComicAuthors)
                .HasForeignKey(ca => ca.ComicId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ComicAuthor>()
                .HasOne(ca => ca.Author)
                .WithMany(a => a.ComicAuthors)
                .HasForeignKey(ca => ca.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Comic)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.ComicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Account)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Comic)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(oi => oi.ComicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Account)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Comic)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.ComicId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comic>()
                .Property(c => c.Price)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.UnitPrice)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(10,2)");
        }
    }
}