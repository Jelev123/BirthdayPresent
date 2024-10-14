namespace BirthdayPresent.Core.Handlers
{
    using System;

    public class ResourceAlreadyExistsException : Exception
    {
        public ResourceAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
