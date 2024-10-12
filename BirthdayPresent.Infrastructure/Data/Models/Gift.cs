namespace BirthdayPresent.Infrastructure.Data.Models
{
    using BirthdayPresent.Infrastructure.Data.Models.Base;

    public class Gift : BaseEntity
    {
        public Gift()
        {
            this.Votes = new HashSet<Vote>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Vote> Votes { get; set; }
    }
}
