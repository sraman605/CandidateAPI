using System.Threading.Tasks;
using CandidateAPI.Models;

namespace CandidateAPI.Services
{
    public interface ICandidateService
    {
        Task<Candidate> AddOrUpdateCandidate(Candidate candidate);
        Task<Candidate?> GetCandidateByEmail(string email);
    }
}