using Akavache;
using DiceRoller.Models;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DiceRoller.Storage
{
    public class EntityRepository : IEntityRepository
    {
        public EntityRepository()
        {
            BlobCache.ApplicationName = "DiceRoller";
        }

        public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
            where TEntity : IEntity
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;

            var entityCollection = (await this.GetAllAsync<TEntity>())
                .ToList();

            entityCollection.Add(entity);

            await this.WriteEntities(entityCollection);

            return entity;  
        }

        public async Task UpdateAsync<TEntity>(TEntity entity)
            where TEntity : IEntity
        {
            entity.UpdatedDate = DateTime.UtcNow;

            var entityCollection = (await this.GetAllAsync<TEntity>())
                .ToList();
            var storedEntity = entityCollection.First(e => e.Id == entity.Id);
            entityCollection.Remove(storedEntity);
            entityCollection.Add(entity);

            await this.WriteEntities(entityCollection);
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(Guid id)
            where TEntity : IEntity
        {
            return (await this.GetAllAsync<TEntity>())
                .FirstOrDefault(e => e.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : IEntity
        {
            try
            {
                return await BlobCache.UserAccount.GetObject<List<TEntity>>(typeof(TEntity).Name);
            }
            catch(KeyNotFoundException)
            {
                return Enumerable.Empty<TEntity>();
            }
        }

        public async Task RemoveAsync<TEntity>(TEntity entity)
            where TEntity : IEntity
        {
            var entityCollection = (await this.GetAllAsync<TEntity>())
                .Where(e => e.Id != entity.Id);

            await this.WriteEntities(entityCollection);

            if(!entityCollection.Any())
                await BlobCache.UserAccount.Invalidate(entity.Id.ToString());
        }

        private async Task WriteEntities<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : IEntity
        {
            await BlobCache.UserAccount.InsertObject(typeof(TEntity).Name, entities.ToList());
        }
    }
}
