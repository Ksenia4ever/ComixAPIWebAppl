using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Models;

public partial class ComixAPIContext : DbContext
{
    public ComixAPIContext()
    {
    }

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Roles_Name");
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Accounts_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.IsBlocked)
                .HasDefaultValue(false);

            entity.Property(e => e.Created)
                .HasColumnType("datetime2");

            entity.Property(e => e.Modified)
                .HasColumnType("datetime2");

            entity.HasIndex(e => e.Email)
                .IsUnique()
                .HasDatabaseName("UQ_Accounts_Email");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Accounts_Roles");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Authors_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Biography);

            entity.Property(e => e.Created)
                .HasColumnType("datetime2");

            entity.Property(e => e.Modified)
                .HasColumnType("datetime2");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Genres_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Genres_Name");

            entity.Property(e => e.Created)
                .HasColumnType("datetime2");

            entity.Property(e => e.Modified)
                .HasColumnType("datetime2");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Publishers_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Country)
                .HasMaxLength(100);

            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Publishers_Name");

            entity.Property(e => e.Created)
                .HasColumnType("datetime2");

            entity.Property(e => e.Modified)
                .HasColumnType("datetime2");
        });

        modelBuilder.Entity<Comic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Comics_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .IsRequired();

            entity.Property(e => e.Price)
                .HasColumnType("decimal(10,2)");

            entity.Property(e => e.StockQuantity);

            entity.Property(e => e.ReleaseYear);

            entity.Property(e => e.CoverImagePath)
                .HasMaxLength(255);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.Created)
                .HasColumnType("datetime2");

            entity.Property(e => e.Modified)
                .HasColumnType("datetime2");

            entity.HasOne(d => d.Genre).WithMany(p => p.Comics)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Comics_Genres");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Comics)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Comics_Publishers");
        });

        modelBuilder.Entity<ComicAuthor>(entity =>
        {
            entity.HasKey(e => new { e.ComicId, e.AuthorId }).HasName("ComicAuthors_pkey");

            entity.ToTable("ComicAuthors");

            entity.Property(e => e.ComicId).HasColumnName("ComicId");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorId");

            entity.HasOne(d => d.Comic).WithMany(p => p.ComicAuthors)
                .HasForeignKey(d => d.ComicId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ComicAuthors_Comics");

            entity.HasOne(d => d.Author).WithMany(p => p.ComicAuthors)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ComicAuthors_Authors");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Carts_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.HasIndex(e => e.AccountId)
                .IsUnique()
                .HasDatabaseName("UQ_Carts_AccountId");

            entity.Property(e => e.Created)
                .HasColumnType("datetime2");

            entity.Property(e => e.Modified)
                .HasColumnType("datetime2");

            entity.HasOne(d => d.Account).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Carts_Accounts");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CartItems_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Quantity);

            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10,2)");

            entity.HasIndex(e => new { e.CartId, e.ComicId })
                .IsUnique()
                .HasDatabaseName("UQ_CartItems_Cart_Comic");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CartItems_Carts");

            entity.HasOne(d => d.Comic).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ComicId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CartItems_Comics");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Orders_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10,2)");

            entity.Property(e => e.Created)
                .HasColumnType("datetime2");

            entity.Property(e => e.Modified)
                .HasColumnType("datetime2");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_Accounts");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OrderItems_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Quantity);

            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10,2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(d => d.Comic).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ComicId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderItems_Comics");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Reviews_pkey");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Rating);

            entity.Property(e => e.Comment);

            entity.Property(e => e.Created)
                .HasColumnType("datetime2");

            entity.Property(e => e.Modified)
                .HasColumnType("datetime2");

            entity.HasIndex(e => new { e.ComicId, e.AccountId })
                .IsUnique()
                .HasDatabaseName("UQ_Reviews_Comic_Account");

            entity.HasOne(d => d.Account).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Reviews_Accounts");

            entity.HasOne(d => d.Comic).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ComicId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Reviews_Comics");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


//using Microsoft.EntityFrameworkCore;

//namespace ComixAPIWebApp.Models
//{
//    public class ComixAPIContext : DbContext
//    {
//        public ComixAPIContext(DbContextOptions<ComixAPIContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<Account> Accounts { get; set; }
//        public virtual DbSet<Author> Authors { get; set; }
//        public virtual DbSet<Cart> Carts { get; set; }
//        public virtual DbSet<CartItem> CartItems { get; set; }
//        public virtual DbSet<Comic> Comics { get; set; }
//        public virtual DbSet<ComicAuthor> ComicAuthors { get; set; }
//        public virtual DbSet<Genre> Genres { get; set; }
//        public virtual DbSet<Order> Orders { get; set; }
//        public virtual DbSet<OrderItem> OrderItems { get; set; }
//        public virtual DbSet<Publisher> Publishers { get; set; }
//        public virtual DbSet<Review> Reviews { get; set; }
//        public virtual DbSet<Role> Roles { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.Entity<ComicAuthor>()
//                .HasKey(ca => new { ca.ComicId, ca.AuthorId });

//            modelBuilder.Entity<Account>()
//                .HasIndex(a => a.Email)
//                .IsUnique();

//            modelBuilder.Entity<Cart>()
//                .HasIndex(c => c.AccountId)
//                .IsUnique();

//            modelBuilder.Entity<CartItem>()
//                .HasIndex(ci => new { ci.CartId, ci.ComicId })
//                .IsUnique();

//            modelBuilder.Entity<Review>()
//                .HasIndex(r => new { r.ComicId, r.AccountId })
//                .IsUnique();

//            modelBuilder.Entity<Account>()
//                .HasOne(a => a.Role)
//                .WithMany(r => r.Accounts)
//                .HasForeignKey(a => a.RoleId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<Account>()
//                .HasOne(a => a.Cart)
//                .WithOne(c => c.Account)
//                .HasForeignKey<Cart>(c => c.AccountId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<Comic>()
//                .HasOne(c => c.Genre)
//                .WithMany(g => g.Comics)
//                .HasForeignKey(c => c.GenreId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<Comic>()
//                .HasOne(c => c.Publisher)
//                .WithMany(p => p.Comics)
//                .HasForeignKey(c => c.PublisherId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<ComicAuthor>()
//                .HasOne(ca => ca.Comic)
//                .WithMany(c => c.ComicAuthors)
//                .HasForeignKey(ca => ca.ComicId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<ComicAuthor>()
//                .HasOne(ca => ca.Author)
//                .WithMany(a => a.ComicAuthors)
//                .HasForeignKey(ca => ca.AuthorId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<CartItem>()
//                .HasOne(ci => ci.Cart)
//                .WithMany(c => c.CartItems)
//                .HasForeignKey(ci => ci.CartId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<CartItem>()
//                .HasOne(ci => ci.Comic)
//                .WithMany(c => c.CartItems)
//                .HasForeignKey(ci => ci.ComicId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<Order>()
//                .HasOne(o => o.Account)
//                .WithMany(a => a.Orders)
//                .HasForeignKey(o => o.AccountId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<OrderItem>()
//                .HasOne(oi => oi.Order)
//                .WithMany(o => o.OrderItems)
//                .HasForeignKey(oi => oi.OrderId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<OrderItem>()
//                .HasOne(oi => oi.Comic)
//                .WithMany(c => c.OrderItems)
//                .HasForeignKey(oi => oi.ComicId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<Review>()
//                .HasOne(r => r.Account)
//                .WithMany(a => a.Reviews)
//                .HasForeignKey(r => r.AccountId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<Review>()
//                .HasOne(r => r.Comic)
//                .WithMany(c => c.Reviews)
//                .HasForeignKey(r => r.ComicId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<Comic>()
//                .Property(c => c.Price)
//                .HasColumnType("decimal(10,2)");

//            modelBuilder.Entity<CartItem>()
//                .Property(ci => ci.UnitPrice)
//                .HasColumnType("decimal(10,2)");

//            modelBuilder.Entity<Order>()
//                .Property(o => o.TotalAmount)
//                .HasColumnType("decimal(10,2)");

//            modelBuilder.Entity<OrderItem>()
//                .Property(oi => oi.UnitPrice)
//                .HasColumnType("decimal(10,2)");
//        }
//    }
//}

