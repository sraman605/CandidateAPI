using System.Threading.Tasks;
using CandidateAPI.Models;
using CandidateAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CandidateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _service;

        public CandidateController(ICandidateService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] Candidate candidate)
        {
            if (candidate == null)
            {
                return BadRequest("Candidate information is null.");
            }

            switch (string.IsNullOrWhiteSpace(candidate.Email), string.IsNullOrWhiteSpace(candidate.FirstName), string.IsNullOrWhiteSpace(candidate.LastName), string.IsNullOrWhiteSpace(candidate.FreeTextComment))
            {
                case (true, _, _, _):
                    return BadRequest("Email is required.");
                case (_, true, _, _):
                    return BadRequest("First name is required.");
                case (_, _, true, _):
                    return BadRequest("Last name is required.");
                case (_, _, _, true):
                    return BadRequest("Free text comment is required.");
                default:
                    return Ok(await _service.AddOrUpdateCandidate(candidate));
                 break;
            }
           
        }
    }
}