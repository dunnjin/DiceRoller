using DiceRoller.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiceRoller.Storage
{
    public interface IEntityRepository
    {
        Task<TEntity> AddAsync<TEntity>(TEntity entity)
            where TEntity : IEntity;

        Task UpdateAsync<TEntity>(TEntity entity)
            where TEntity : IEntity;

        Task<TEntity> GetByIdAsync<TEntity>(Guid id)
            where TEntity : IEntity;

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : IEntity;

        Task RemoveAsync<TEntity>(TEntity entity)
            where TEntity : IEntity;
    }
}
