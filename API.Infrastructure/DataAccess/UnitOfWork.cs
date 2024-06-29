using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using API.Infrastructure.Interfaces;
using API.Infrastructure.Data;
using API.Shared.Extensions;
using API.Infrastructure.Entities;
using API.Shared.Exceptions;

namespace API.Infrastructure.DataAcess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext db;

        private readonly IHttpContextAccessor _accessor;

        private string _ErrorMassage = string.Empty;

        private readonly Dictionary<Type, object> PrivateRepository;

        public Task<IDbContextTransaction> GetDBTransaction { get => db.Database.BeginTransactionAsync(); }

        public UnitOfWork(ApplicationDBContext _db, IHttpContextAccessor accessor)
        {
            db = _db;
            _accessor = accessor;
            PrivateRepository = new Dictionary<Type, object>();
        }

        #region Repository

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, new()
        {
            object repository;

            PrivateRepository.TryGetValue(typeof(TEntity), out repository);
            if (repository == null)
            {
                repository = new Repository<TEntity>(db, _accessor);
                PrivateRepository.Add(typeof(TEntity), repository);
            }

            return (Repository<TEntity>)repository;
        }

        #endregion

        #region DB Managment


        public async Task<bool> Save()
        {
            Guid userId = _accessor.HttpContext.GetClaimsUserID();

            return await Save(userId);
        }

        public async Task<bool> Save(Guid UserId)
        {
            try
            {
                foreach (var item in db.ChangeTracker.Entries())
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            if (item.Entity is BaseEntity)
                            {
                                ((BaseEntity)item.Entity).ID = Guid.NewGuid();
                                ((BaseEntity)item.Entity).CreateDate = DateTime.UtcNow;
                            }

                            if (item.Entity is IUserEntity)
                                if (((IUserEntity)item.Entity).UserId == Guid.Empty)
                                    ((IUserEntity)item.Entity).UserId = UserId;

                            break;
                        case EntityState.Modified:
                            if (item.Entity is BaseEntity)
                            {
                                if (item.Entity is ISoftDeleteEntity)
                                {
                                    if (!((ISoftDeleteEntity)item.Entity).IsDeleted)
                                        ((BaseEntity)item.Entity).UpdateDate = DateTime.UtcNow;
                                }
                                else
                                    ((BaseEntity)item.Entity).UpdateDate = DateTime.UtcNow;

                                item.Property(nameof(BaseEntity.CreateDate)).IsModified = false;
                            }

                            break;
                        case EntityState.Detached:
                            break;
                    }
                }

                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var handledException = DataBaseExceptionNormalizer.Check(ex);
                if (!String.IsNullOrEmpty(handledException))
                    throw new DataBasehandledException(handledException);

                throw new DataBaseUnhandledException(ex.Message);
            }
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = db.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        #endregion

        public async ValueTask DisposeAsync()
        {
            await db.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
