namespace BirthdayPresent.Core.Services.Base
{
    using BirthdayPresent.Core.Constants;
    using BirthdayPresent.Core.Handlers;
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models.Interfaces.Base;
    using Microsoft.EntityFrameworkCore;

    public class BaseService<T> where T : class, IBaseEntity
    {
        protected readonly ApplicationDbContext _data;

        public BaseService(ApplicationDbContext data)
        {
            this._data = data;
        }

        protected async Task CreateEntityAsync(T entity, CancellationToken _cancellationToken)
        {
            await AddEntityAsync(entity, _cancellationToken);
            await SaveModificationAsync(entity, _cancellationToken);
        }

        protected async Task CreateEntityAsync<TEntity>(TEntity entity, CancellationToken _cancellationToken) where TEntity : class, IBaseEntity
        {
            await AddEntityAsync(entity, _cancellationToken);
            await SaveModificationAsync(entity, _cancellationToken);
        }

        protected async Task SaveModificationAsync(T entity, CancellationToken _cancellationToken)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            await _data.SaveChangesAsync(_cancellationToken);
        }

        protected async Task SaveModificationAsync<TEntity>(TEntity entity, CancellationToken _cancellationToken) where TEntity : class, IBaseEntity
        {
            entity.UpdatedAt = DateTime.UtcNow;
            await _data.SaveChangesAsync(_cancellationToken);
        }

        protected async Task<bool> AnyByIdAsync(int id, CancellationToken _cancellationToken)
        {
            var any = await _data.Set<T>()
                .Where(e => e.Id == id && !e.Deleted)
                .AsNoTracking()
                .AnyAsync(_cancellationToken);
            return any;
        }

        protected async Task<bool> AnyByIdAsync<TEntity>(int id, CancellationToken _cancellationToken) where TEntity : class, IBaseEntity
        {
            var any = await _data.Set<TEntity>()
                .Where(e => e.Id == id && !e.Deleted)
                .AsNoTracking()
                .AnyAsync(_cancellationToken);

            return any;
        }

        protected async Task<T> GetEntityByIdAsync(int Id, CancellationToken _cancellationToken)
        {
            var entity = await this.FindByIdAsync(Id, _cancellationToken);

            if (entity != null)
                return entity;

            throw new ResourceNotFoundException(string.Format(
                ErrorMessages.EntityDoesNotExist, typeof(T).Name));
        }

        protected async Task<TEntity> FindIdByIdOrDefaultAsync<TEntity>(int id, CancellationToken _cancellationToken) where TEntity : class, IBaseEntity
        {
            var entity = await _data.Set<TEntity>()
                .Where(e => e.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(_cancellationToken);

            return entity;
        }

        private async Task AddEntityAsync(T entity, CancellationToken _cancellationToken)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _data.AddAsync(entity, _cancellationToken);
        }

        private async Task AddEntityAsync<TEntity>(TEntity entity, CancellationToken _cancellationToken) where TEntity : class, IBaseEntity
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _data.AddAsync(entity, _cancellationToken);
        }

        private async Task<T> FindByIdOrDefaultAsync(int id, CancellationToken _cancellationToken)
        {
            var entity = await _data.Set<T>()
                .Where(e => e.Id == id && !e.Deleted)
                .AsNoTracking()
                .FirstOrDefaultAsync(_cancellationToken);

            return entity;
        }

        private async Task<T> FindByIdAsync(int id, CancellationToken _cancellationToken)
        {
            var entity = await _data.Set<T>()
                .Where(e => e.Id == id && !e.Deleted)
                .FirstOrDefaultAsync(_cancellationToken);

            return entity;
        }
    }
}
