using CandidateAPI.Controllers;
using CandidateAPI.Models;
using CandidateAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;


public class CandidateControllerTests
{
    private readonly Mock<ICandidateService> _serviceMock;
    private readonly CandidateController _controller;
    private readonly IMemoryCache _memoryCache;

    public CandidateControllerTests()
    {
        _serviceMock = new Mock<ICandidateService>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _controller = new CandidateController(_serviceMock.Object, _memoryCache);

    }

    [Fact]
    public async Task AddOrUpdateCandidate_ReturnsBadRequest_WhenCandidateIsNull()
    {
        // Act
        var result = await _controller.AddOrUpdateCandidate(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Candidate information is null.", badRequestResult.Value);
    }

    [Fact]
    public async Task AddOrUpdateCandidate_ReturnsBadRequest_WhenEmailIsNullOrWhiteSpace()
    {
        // Arrange
        var candidate = new Candidate { FirstName = "John", LastName = "Doe", FreeTextComment = "Some comment" };

        // Act
        var result = await _controller.AddOrUpdateCandidate(candidate);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Email is required.", badRequestResult.Value);
    }

    [Fact]
    public async Task AddOrUpdateCandidate_ReturnsBadRequest_WhenFirstNameIsNullOrWhiteSpace()
    {
        // Arrange
        var candidate = new Candidate { Email = "john.doe@example.com", LastName = "Doe", FreeTextComment = "Some comment" };

        // Act
        var result = await _controller.AddOrUpdateCandidate(candidate);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("First name is required.", badRequestResult.Value);
    }

    [Fact]
    public async Task AddOrUpdateCandidate_ReturnsBadRequest_WhenLastNameIsNullOrWhiteSpace()
    {
        // Arrange
        var candidate = new Candidate { Email = "sraman605@gmail.com", FirstName = "Shraman", FreeTextComment = "Some comment" };

        // Act
        var result = await _controller.AddOrUpdateCandidate(candidate);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Last name is required.", badRequestResult.Value);
    }

    [Fact]
    public async Task AddOrUpdateCandidate_ReturnsBadRequest_WhenFreeTextCommentIsNullOrWhiteSpace()
    {
        // Arrange
        var candidate = new Candidate { Email = "sraman605@gmail.com", FirstName = "Shraman", LastName = "Bajracharya" };

        // Act
        var result = await _controller.AddOrUpdateCandidate(candidate);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Free text comment is required.", badRequestResult.Value);
    }

    [Fact]
    public async Task AddOrUpdateCandidate_ReturnsOk_WhenCandidateIsValid()
    {
        // Arrange
        var candidate = new Candidate { Email = "sraman605@gmail.com", FirstName = "Shraman", LastName = "Bajracharya", FreeTextComment = "Some comment" };
        _serviceMock.Setup(s => s.AddOrUpdateCandidate(candidate)).ReturnsAsync(candidate);

        // Act
        var result = await _controller.AddOrUpdateCandidate(candidate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(candidate, okResult.Value);
    }

    /// <summary>
    /// This test checks if the controller uses the cached candidate instead of calling the service. 
    /// It verifies that the service method is not called when the candidate is in the cache.
    /// </summary>

    [Fact]
    public async Task AddOrUpdateCandidate_UsesCache_WhenCandidateExistsInCache()
    {
        // Arrange
        var candidate = new Candidate { Email = "cached@example.com", FirstName = "Cached", LastName = "User", FreeTextComment = "Cached comment" };
        var cacheKey = $"Candidate_{candidate.Email}";
        _memoryCache.Set(cacheKey, candidate, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)));

        // Act
        var result = await _controller.AddOrUpdateCandidate(candidate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(candidate, okResult.Value);

        // Verify that the service method was not called
        _serviceMock.Verify(s => s.AddOrUpdateCandidate(It.IsAny<Candidate>()), Times.Never);
    }

    /// <summary>
    /// This test checks if the controller updates the cache when a new candidate is added.
    /// It verifies that the cache is updated with the new candidate.
    /// </summary>
    [Fact]
    public async Task AddOrUpdateCandidate_UpdatesCache_WhenNewCandidateIsAdded()
    {
        // Arrange
        var candidate = new Candidate { Email = "newuser@example.com", FirstName = "New", LastName = "User", FreeTextComment = "New comment" };
        _serviceMock.Setup(s => s.AddOrUpdateCandidate(candidate)).ReturnsAsync(candidate);
        var cacheKey = $"Candidate_{candidate.Email}";

        // Act
        var result = await _controller.AddOrUpdateCandidate(candidate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(candidate, okResult.Value);

        // Verify that the cache is updated
        var cachedCandidate = _memoryCache.Get<Candidate>(cacheKey);
        Assert.NotNull(cachedCandidate);
        Assert.Equal(candidate.Email, cachedCandidate.Email);
        Assert.Equal(candidate.FirstName, cachedCandidate.FirstName);
        Assert.Equal(candidate.LastName, cachedCandidate.LastName);
        Assert.Equal(candidate.FreeTextComment, cachedCandidate.FreeTextComment);
    }

}
