namespace BirthdayPresent.Core.Services.Gift
{
    using BirthdayPresent.Core.Interfaces.Gift;
    using BirthdayPresent.Core.Services.Base;
    using BirthdayPresent.Core.ViewModels.Gift;
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GiftService : BaseService<Gift>, IGiftService
    {
        public GiftService(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<AllGiftsViewModel>> GetAllGiftsAsync(CancellationToken cancellationToken)
        {
            return await _data.Gifts.AsNoTracking()
                .Select(x => new AllGiftsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToListAsync();
        }
    }
}
