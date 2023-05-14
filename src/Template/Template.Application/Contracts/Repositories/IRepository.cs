using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Template.Application.Contracts.Repositories
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        void Add(TEntity entity);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        void AddRange(List<TEntity> entities);
        Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken);
        void Delete(TEntity entity);
        bool Any();
        bool Any(Expression<Func<TEntity, bool>> expression);
        Task<bool> AnyAsync(CancellationToken cancellationToken);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
        TEntity FirstOrDefault();
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
        TEntity GetById(TKey id);
        Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken);
        List<TEntity> List();
        List<TEntity> List(Expression<Func<TEntity, bool>> expression);
        Task<List<TEntity>> ListAsync(CancellationToken cancellationToken);
        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
        int Count();
        int Count(Expression<Func<TEntity, bool>> expression);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
    }
}
