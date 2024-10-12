namespace BirthdayPresent.Infrastructure.Data.Models.Base
{
    using BirthdayPresent.Infrastructure.Data.Models.Interfaces.Base;

    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
