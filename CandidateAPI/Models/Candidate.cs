namespace CandidateAPI.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; // Unique identifier
        public string BestTimeToCall { get; set; } = string.Empty;
        public string LinkedInProfileUrl { get; set; } = string.Empty;
        public string GitHubProfileUrl { get; set; } = string.Empty;
        public string FreeTextComment { get; set; } = string.Empty;
    }
}