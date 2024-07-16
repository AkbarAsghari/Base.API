using API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<IDbContextTransaction> GetDBTransaction { get; }

        IRepository<TEntity> Repository<TEntity>() where TEntity : class, new();

        #region DB Managment

        Task<bool> Save();

        Task<bool> Save(Guid UserId);

        void DetachAllEntities();

        #endregion

    }
}
