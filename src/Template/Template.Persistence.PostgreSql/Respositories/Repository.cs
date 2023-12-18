using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Template.Application.Contracts.Repositories;
using Template.Domain.Entities.Abastractions;
using Template.Persistence.PosgreSql.Database;

namespace Template.Persistence.PosgreSql.Respositories
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
        where TKey : IEquatable<TKey>
    {

        protected readonly Context context;

        public Repository(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual void Add(TEntity entity) =>
            context.Set<TEntity>().Add(entity);

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken) =>
            await context.Set<TEntity>().AddAsync(entity, cancellationToken);

        public virtual void AddRange(List<TEntity> entities) =>
            context.Set<TEntity>().AddRange(entities);

        public virtual async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken) =>
            await context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        public virtual bool Any() =>
            context.Set<TEntity>().Any();

        public virtual bool Any(Expression<Func<TEntity, bool>> expression) =>
            context.Set<TEntity>().Any(expression);

        public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken) =>
            await context.Set<TEntity>().AnyAsync(cancellationToken);

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken) =>
            await context.Set<TEntity>().AnyAsync(expression, cancellationToken);

        public virtual void Delete(TEntity entity) =>
            context.Set<TEntity>().Remove(entity);

        public virtual TEntity FirstOrDefault() =>
             context.Set<TEntity>().FirstOrDefault();

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression) =>
             context.Set<TEntity>().FirstOrDefault(expression);

        public virtual async Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken) =>
            await context.Set<TEntity>().FirstOrDefaultAsync(cancellationToken);

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken) =>
            await context.Set<TEntity>().FirstOrDefaultAsync(expression, cancellationToken);

        public virtual TEntity GetById(TKey id) =>
            context.Set<TEntity>().Find(id);

        public virtual TEntity GetByIdNoTraking(TKey id) =>
            context.Set<TEntity>().AsNoTracking().FirstOrDefault(e => e.Id.Equals(id));

        public virtual async Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken) =>
            await context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);

        public virtual async Task<TEntity> GetByIdNoTrackingAsync(TKey id, CancellationToken cancellationToken) =>
            await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);

        public virtual List<TEntity> List() =>
            context.Set<TEntity>().ToList();

        public virtual List<TEntity> List(Expression<Func<TEntity, bool>> expression) =>
            context.Set<TEntity>().Where(expression).ToList();

        public virtual async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken) =>
            await context.Set<TEntity>().ToListAsync(cancellationToken);

        public virtual async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken) =>
            await context.Set<TEntity>().Where(expression).ToListAsync(cancellationToken);

        public virtual int Count() =>
             context.Set<TEntity>().Count();

        public virtual int Count(Expression<Func<TEntity, bool>> expression) =>
             context.Set<TEntity>().Where(expression).Count();

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken) =>
             await context.Set<TEntity>().CountAsync(cancellationToken);

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken) =>
            await context.Set<TEntity>().Where(expression).CountAsync(cancellationToken);

    }
}
