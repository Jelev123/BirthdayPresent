namespace BirthdayPresent.Core.Services.Base
{
    using BirthdayPresent.Core.Constants;
    using BirthdayPresent.Core.Handlers;
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models.Interfaces.Base;
    using Microsoft.EntityFrameworkCore;

    public abstract class BaseService<T> where T : class, IBaseEntity
    {
        protected readonly ApplicationDbContext _data;

        public BaseService(ApplicationDbContext data)
        {
            this._data = data;
        }

        protected async Task CreateEntityAsync(T entity, CancellationToken _cancellationToken)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            await _data.AddAsync(entity, _cancellationToken);
            await _data.SaveChangesAsync(_cancellationToken);
        }

        protected async Task SaveModificationAsync(T entity, CancellationToken _cancellationToken)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            await _data.SaveChangesAsync(_cancellationToken);
        }

        protected async Task<bool> AnyByIdAsync(int id, CancellationToken _cancellationToken)
        {
            return await _data.Set<T>()
                .Where(e => e.Id == id && !e.Deleted)
                .AsNoTracking()
                .AnyAsync(_cancellationToken);
        }

        protected async Task<T> GetEntityByIdAsync(int id, CancellationToken _cancellationToken)
        {
            var entity = await _data.Set<T>()
                .Where(e => e.Id == id && !e.Deleted)
                .AsNoTracking()
                .FirstOrDefaultAsync(_cancellationToken);

            if (entity == null)
            {
                throw new ResourceNotFoundException(string.Format(ErrorMessages.EntityDoesNotExist, typeof(T).Name));
            }

            return entity;
        }

        protected async Task<TEntity> FindByIdOrDefaultAsync<TEntity>(int id, CancellationToken _cancellationToken) where TEntity : class, IBaseEntity
        {
            return await _data.Set<TEntity>()
                .Where(e => e.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(_cancellationToken);
        }
    }
}
