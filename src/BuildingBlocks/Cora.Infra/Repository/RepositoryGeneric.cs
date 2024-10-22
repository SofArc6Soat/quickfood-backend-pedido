using Cora.Infra.Context;
using Core.Domain.Data;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Cora.Infra.Repository
{
    [ExcludeFromCodeCoverage]
    public abstract class RepositoryGeneric<TEntity>(IContext context) : IRepositoryGeneric<TEntity>
        where TEntity : Entity
    {
        protected readonly IContext Context = context;

        public IUnitOfWork UnitOfWork => Context;

        public DbConnection GetDbConnection() => Context.GetDbConnection();

        public virtual Task<TEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken) =>
            Context.GetDbSet<TEntity>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        public virtual Task<List<TEntity>> FindAllAsync(CancellationToken cancellationToken) =>
            Context.GetDbSet<TEntity>().AsNoTracking().ToListAsync(cancellationToken);

        public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken) =>
            await Context.GetDbSet<TEntity>().AddAsync(entity, cancellationToken);

        public virtual async Task InsertRangeAsync(List<TEntity> entityList, CancellationToken cancellationToken) =>
            await Context.GetDbSet<TEntity>().AddRangeAsync(entityList, cancellationToken);

        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            Context.GetDbSet<TEntity>().Update(entity);

            return Task.CompletedTask;
        }

        public virtual Task UpdateRangeAsync(List<TEntity> entityList, CancellationToken cancellationToken)
        {
            Context.GetDbSet<TEntity>().UpdateRange(entityList);

            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = Context.GetDbSet<TEntity>().Find(id);
            if (entity != null)
            {
                Context.GetDbSet<TEntity>().Remove(entity);
            }

            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            Context.GetDbSet<TEntity>().Remove(entity);

            return Task.CompletedTask;
        }

        public virtual Task DeleteRangeAsync(List<TEntity> entityList, CancellationToken cancellationToken)
        {
            Context.GetDbSet<TEntity>().RemoveRange(entityList);

            return Task.CompletedTask;
        }

        public virtual IQueryable<TEntity> Query() => Context.GetDbSet<TEntity>().AsQueryable();

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) =>
            Context.GetDbSet<TEntity>().AsNoTracking().Where(predicate);
    }
}