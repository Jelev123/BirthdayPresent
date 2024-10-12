namespace BirthdayPresent.Infrastructure.Data.Models.Interfaces.Base
{
    public interface IBaseEntity
    {
         int Id { get; set; }
         DateTime CreatedAt { get; set; }
         DateTime UpdatedAt { get; set; }
    }
}
