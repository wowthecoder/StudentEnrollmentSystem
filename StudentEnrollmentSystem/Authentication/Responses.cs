namespace StudentEnrollmentSystem.Authentication
{
    public class StatusResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    public class TokenResponse
    {
        public string? Token { get; set; }
        public string? Role { get; set; }
        public DateTime Expiration { get; set; }
    }
}
