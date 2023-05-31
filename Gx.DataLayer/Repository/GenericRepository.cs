using System;
using System.Linq;
using System.Threading.Tasks;
using Gx.DataLayer.Context;
using Gx.DataLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Gx.DataLayer.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        #region constructor

        private GxDbContext context;
        private DbSet<TEntity> dbSet;

        public GenericRepository(GxDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }

        #endregion


        public IQueryable<TEntity> GetEntitiesQuery()
        {
            return dbSet.AsQueryable();
        }

        public async Task<TEntity> GetEntityById(long entityId)
        {
            return await dbSet.SingleOrDefaultAsync(e => e.Id == entityId);
        }

        public  Int64 GetMaxEntityId()
        {
            return  dbSet.Max(r => r.Id);
        }

        public async Task AddEntity(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.LastUpdateDate = entity.CreateDate;
            await dbSet.AddAsync(entity);
        }

        public  long AddEntityForGetId(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.LastUpdateDate = entity.CreateDate;
            dbSet.AddAsync(entity);
            return entity.Id;
        }


        public void UpdateEntity(TEntity entity)
        {
            entity.LastUpdateDate = DateTime.Now;
            dbSet.Update(entity);
        }

        public void RemoveEntity(TEntity entity)
        {
            entity.Status = true;
            UpdateEntity(entity);
        }

        public async Task RemoveEntity(long entityId)
        {
            var entity = await GetEntityById(entityId);
            RemoveEntity(entity);
        }

        public async Task DeleteEntity(long entityId)
        {
            var entity = await GetEntityById(entityId);
            dbSet.Remove(entity);
            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context?.Dispose();
        }

       
    }
}
