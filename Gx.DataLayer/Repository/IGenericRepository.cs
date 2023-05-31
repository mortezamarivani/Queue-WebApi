using System;
using System.Linq;
using System.Threading.Tasks;
using Gx.DataLayer.Entities.Common;

namespace Gx.DataLayer.Repository
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetEntitiesQuery();

        Task<TEntity> GetEntityById(long entityId);

        Int64 GetMaxEntityId();

        Task AddEntity(TEntity entity);
        long AddEntityForGetId(TEntity entity);

        void UpdateEntity(TEntity entity);

        void RemoveEntity(TEntity entity);

        Task DeleteEntity(long entityId);

        Task RemoveEntity(long entityId);

        Task SaveChanges();
    }
}