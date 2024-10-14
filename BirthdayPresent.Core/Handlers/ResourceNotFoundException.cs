namespace BirthdayPresent.Core.Handlers
{
    using System;

    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message)
        {
        }
    }
}
