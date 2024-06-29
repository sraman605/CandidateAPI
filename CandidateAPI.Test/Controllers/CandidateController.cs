using System.Threading.Tasks;
using CandidateAPI.Controllers;
using CandidateAPI.Models;
using CandidateAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;


public class CandidateControllerTests
{
    private readonly Mock<ICandidateService> _serviceMock;
    private readonly CandidateController _controller;

    public CandidateControllerTests()
    {
        _serviceMock = new Mock<ICandidateService>();
        _controller = new CandidateController(_serviceMock.Object);
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
        var candidate = new Candidate { Email = "john.doe@example.com", FirstName = "John", FreeTextComment = "Some comment" };

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
        var candidate = new Candidate { Email = "john.doe@example.com", FirstName = "John", LastName = "Doe" };

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
        var candidate = new Candidate { Email = "john.doe@example.com", FirstName = "John", LastName = "Doe", FreeTextComment = "Some comment" };
        _serviceMock.Setup(s => s.AddOrUpdateCandidate(candidate)).ReturnsAsync(candidate);

        // Act
        var result = await _controller.AddOrUpdateCandidate(candidate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(candidate, okResult.Value);
    }
}
