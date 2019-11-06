using System.Collections.Generic;
using System.Threading.Tasks;

using Moq;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Infrastructure.Services;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit
{
    public class TeamTunerUserServiceTests
    {
        public TeamTunerUserServiceTests()
        {
            _testUser = new TeamTunerUser();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var teamTunerRepositoryMock = new Mock<ITeamTunerUserRepository>();
            teamTunerRepositoryMock.Setup(r => r.GetAsync(_testUser.Id, It.IsAny<IEnumerable<string>>()))
                                   .ReturnsAsync(_testUser);
            var registrationRequestRepositoryMock = new Mock<IRegistrationRequestRepository>();
            var emailServiceMock = new Mock<IEmailVerificationService>();
            var cardLevelRepositoryMock = new Mock<ICardLevelRepository>();

            _userService = new TeamTunerUserService(teamTunerRepositoryMock.Object, cardLevelRepositoryMock.Object, registrationRequestRepositoryMock.Object,
                emailServiceMock.Object, unitOfWorkMock.Object);
        }

        private readonly TeamTunerUser _testUser;
        private readonly TeamTunerUserService _userService;

        [Fact]
        public async Task UpdateTest()
        {
            // Arrange
            var updateUser = new TeamTunerUser
                             {
                                 Id = _testUser.Id,
                                 Email = "a"
                             };

            // Act
            var updatedUser = await _userService.UpdateAsync(updateUser, new[] {nameof(TeamTunerUser.Email)});

            // Assert
            Assert.Equal(updatedUser.Email, updateUser.Email);
            Assert.Equal(updatedUser.CreatedById, _testUser.CreatedById);
            Assert.Equal(updatedUser.CreatedOnUtc, _testUser.CreatedOnUtc);
        }
    }
}