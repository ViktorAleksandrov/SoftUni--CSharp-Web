using System;

namespace SIS.HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        private const string ErrorMessage = "The Server has encountered an error.";

        public InternalServerErrorException()
            : base(ErrorMessage)
        {
        }
    }
}
