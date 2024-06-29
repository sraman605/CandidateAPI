using CandidateAPI.Models;
using CandidateAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CandidateAPI.Controllers
{
    /// <summary>
    /// API controller for managing candidates.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {

        private readonly ICandidateService _service;
        /// <summary>
        /// Constructor for CandidateController.
        /// </summary>
        /// <param name="service">The candidate service implementation.</param>
        public CandidateController(ICandidateService service)
        {
            _service = service;
        }

        /// <summary>
        /// Adds or updates a candidate.
        /// </summary>
        /// <param name="candidate">The candidate object to add or update.</param>
        /// <returns>Returns IActionResult indicating success or failure.</returns>

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
                    try
                    {
                        var result = await _service.AddOrUpdateCandidate(candidate);
                        return Ok(result);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, "An error occurred while processing your request.");
                    }
            }
        }

    }
}