using System.Threading.Tasks;
using CandidateAPI.Data;
using CandidateAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateAPI.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly DataContext _context;

        public CandidateRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Candidate> AddOrUpdateCandidate(Candidate candidate)
        {
            var existingCandidate = await _context.Candidates
                .FirstOrDefaultAsync(x => x.Email == candidate.Email);

            if (existingCandidate != null)
            {
                existingCandidate.FirstName = candidate.FirstName;
                existingCandidate.LastName = candidate.LastName;
                existingCandidate.PhoneNumber = candidate.PhoneNumber;
                existingCandidate.BestTimeToCall = candidate.BestTimeToCall;
                existingCandidate.LinkedInProfileUrl = candidate.LinkedInProfileUrl;
                existingCandidate.GitHubProfileUrl = candidate.GitHubProfileUrl;
                existingCandidate.FreeTextComment = candidate.FreeTextComment;
                _context.Candidates.Update(existingCandidate);
            }
            else
            {
                _context.Candidates.Add(candidate);
            }

            await _context.SaveChangesAsync();
            return candidate;
        }

        public async Task<Candidate?> GetCandidateByEmail(string email)
        {
            return await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
