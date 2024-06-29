using System.Threading.Tasks;
using CandidateAPI.Models;
using CandidateAPI.Repositories;

namespace CandidateAPI.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _repository;

        public CandidateService(ICandidateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Candidate> AddOrUpdateCandidate(Candidate candidate)
        {
            return await _repository.AddOrUpdateCandidate(candidate);
        }

        //to show in the UI If required in the future.
        public async Task<Candidate?> GetCandidateByEmail(string email)
        {
            return await _repository.GetCandidateByEmail(email);
        }
    }
}