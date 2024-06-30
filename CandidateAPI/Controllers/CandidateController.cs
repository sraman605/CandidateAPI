using CandidateAPI.Models;
using CandidateAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

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
        private readonly IMemoryCache _cache;
        /// <summary>
        /// Constructor for CandidateController.
        /// </summary>
        /// <param name="service">The candidate service implementation.</param>
        public CandidateController(ICandidateService service, IMemoryCache cache)
        {
            _service = service;
            _cache = cache;
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
                        var cacheKey = $"Candidate_{candidate.Email}";

                        /// <summary>
                        /*Before calling the service to add or update a candidate,
                       the cache is checked to see if the candidate with the same email exists.
                       If not found in the cache, the candidate is processed and then added to the cache.*/
                        /// </summary>

                        if (!_cache.TryGetValue(cacheKey, out Candidate cachedCandidate))
                        {
                            // If not in cache, add or update the candidate and cache the result
                            var result = await _service.AddOrUpdateCandidate(candidate);
                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Cache for 5 minutes
                            _cache.Set(cacheKey, result, cacheEntryOptions);

                            return Ok(result);
                        }
                        // Return cached candidate if found
                        return Ok(cachedCandidate);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, "An error occurred while processing your request.");
                    }
            }
        }

    }
}