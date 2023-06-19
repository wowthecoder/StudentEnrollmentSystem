namespace StudentEnrollmentSystem.Services
{
    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
    }

    public class BadRequestException : Exception 
    {
        public BadRequestException() { }
        public BadRequestException(string message) : base(message) { }
    }

    public class ProblemException : Exception
    {
        public ProblemException() { }
        public ProblemException(string message) : base(message) { }
    }
}
