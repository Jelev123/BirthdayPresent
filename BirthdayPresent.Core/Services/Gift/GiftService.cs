namespace BirthdayPresent.Core.Services.Gift
{
    using BirthdayPresent.Core.Interfaces.Gift;
    using BirthdayPresent.Core.ViewModels.Gift;
    using BirthdayPresent.Core.ViewModels.Vote;
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GiftService : IGiftService
    {
        private readonly ApplicationDbContext data;

        public GiftService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<AllGiftsViewModel>> GetAllGiftsAsync()
        {
            return await data.Gifts.Select(x => new AllGiftsViewModel
            {
                GiftId = x.Id,
                GiftName = x.Name,
            }).ToListAsync();
        }

        public async Task<GiftByIdViewModel> GiftById(int id)
        {
            return await data.Gifts.
                Where(x => x.Id == id).
                Select(s => new GiftByIdViewModel 
                { 
                    Name = s.Name,
                    Description = s.Description,
                    IsActive = s.IsActive,
                    Votes = s.Votes.Select(s => new VoteViewModel 
                    {
                        GiftId = id,
                        VotedAt = DateTime.UtcNow,
                        VoterId = s.VoterId,
                        VoteSessionId = s.VoteSessionId,
                    }).ToList()
                }).
                FirstOrDefaultAsync();
        }

        public async Task VoteAsync(int id, string userId)
        {
            var gift = await data.Gifts.FirstOrDefaultAsync(x => x.Id == id);

            if (gift != null)
            {
                var votes = new Vote
                {
                    CreatedAt = DateTime.UtcNow,
                    GiftId = id,
                    UpdatedAt = DateTime.UtcNow,
                    VotedAt = DateTime.UtcNow,
                    VoterId = userId,
                };
            }
        }
    }
}


//{
//    // Refactoring is needed 
//    var tool = await _dbContext.Tools
//        .Include(r => r.Votes)
//        .FirstOrDefaultAsync(r => r.Id == toolId && !r.Deleted)
//        ?? throw new ResourceNotFoundException(string.Format(
//            ErrorMessages.EntityDoesNotExist, typeof(Tool).Name));

//    var existingVote = tool.Votes.FirstOrDefault(v => v.UserId == userId);

//    if (existingVote != null)
//    {
//        if (existingVote.Type == type)
//        {
//            // User clicked on the same vote type again, so remove their vote
//            tool.Votes.Remove(existingVote);
//        }
//        else
//        {
//            // User is changing their vote type
//            existingVote.Type = type;
//        }
//    }
//    else
//    {
//        // The user hasn't voted before, so create a new vote
//        var newVote = new Vote
//        {
//            Id = Guid.NewGuid().ToString(),
//            Type = type,
//            ToolId = tool.Id,
//            UserId = userId,
//        };

//        tool.Votes.Add(newVote);
//    }

//    await _dbContext.SaveChangesAsync();

//    var upVotes = tool.Votes.Count(v => v.Type && !v.Deleted);
//    var downVotes = tool.Votes.Count(v => !v.Type && !v.Deleted);

//    return new VoteViewModel
//    {
//        UpVotes = upVotes,
//        DownVotes = downVotes,
//    };
//}