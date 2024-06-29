using System.Threading.Tasks;
using CandidateAPI.Models;
using CandidateAPI.Repositories;
using CandidateAPI.Services;
using Moq;
using Xunit;

namespace CandidateAPI.Tests.Services
{
    public class CandidateServiceTests
    {
        private readonly Mock<ICandidateRepository> _repositoryMock;
        private readonly CandidateService _service;

        public CandidateServiceTests()
        {
            _repositoryMock = new Mock<ICandidateRepository>();
            _service = new CandidateService(_repositoryMock.Object);
        }

        [Fact]
        public async Task AddOrUpdateCandidate_CallsRepositoryMethod()
        {
            // Arrange
            var candidate = new Candidate { Id = 1, Email = "test@example.com", FirstName = "John", LastName = "Doe" };
            _repositoryMock.Setup(repo => repo.AddOrUpdateCandidate(candidate)).ReturnsAsync(candidate);

            // Act
            var result = await _service.AddOrUpdateCandidate(candidate);

            // Assert
            _repositoryMock.Verify(repo => repo.AddOrUpdateCandidate(candidate), Times.Once);
            Assert.Equal(candidate, result);
        }

    }
}