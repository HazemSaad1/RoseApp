using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace RoseApp.Application.Contracts
{
    /// <summary>
    /// Abstraction over the EF Core DbContext, exposed from the Application layer.
    /// The Infrastructure layer's AppDbContext implements this interface directly,
    /// so Handlers depend on IDbContext instead of a Repository/UnitOfWork pattern.
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Generic accessor for entities.
        /// </summary>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets an EntityEntry for the given entity.
        /// </summary>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}