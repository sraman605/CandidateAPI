using System.Threading.Tasks;
using CandidateAPI.Models;

namespace CandidateAPI.Repositories
{
    public interface ICandidateRepository
    {
        Task<Candidate> AddOrUpdateCandidate(Candidate candidate);
        Task<Candidate?> GetCandidateByEmail(string email);
    }
}