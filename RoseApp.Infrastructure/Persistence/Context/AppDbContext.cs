using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Admin;
using RoseApp.Domain.Entities.Catalog;
using RoseApp.Domain.Entities.Commerce;
using RoseApp.Domain.Entities.Content;
using RoseApp.Domain.Entities.Identity;
using RoseApp.Domain.Entities.Notifications;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Identity
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<OtpVerification> OtpVerifications { get; set; }

        // Catalog
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Occasion> Occasions { get; set; }
        public DbSet<Product> Products { get; set; }

        // Commerce
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }

        // Content
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }

        // Notifications
        public DbSet<Notification> Notifications { get; set; }

        // UserDomain
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

        // Admin
        public DbSet<AdminAuditLog> AdminAuditLogs { get; set; }

        // Implementation of IDbContext
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
            => base.Set<TEntity>();

        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
            => base.Entry(entity);

        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            // Configure composite keys
        }
    }
}