using System.Collections.Generic;
using System.Threading.Tasks;

using Moq;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Infrastructure.Services;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Infrastructure
{
    public class TeamTunerUserServiceTests
    {
        public TeamTunerUserServiceTests()
        {
            _testUser = new TeamTunerUser();

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var emailConfigProviderMock = new Mock<IConfigProvider<EmailConfig>>();

            // Repositories
            var teamTunerRepositoryMock = new Mock<ITeamTunerUserRepository>();
            teamTunerRepositoryMock.Setup(r => r.GetAsync(_testUser.Id, It.IsAny<IEnumerable<string>>()))
                                   .ReturnsAsync(_testUser);
            var registrationRequestRepositoryMock = new Mock<IRegistrationRequestRepository>();
            var teamMembershipRequestRepositoryMock = new Mock<ITeamMembershipRequestRepository>();
            var cardLevelRepositoryMock = new Mock<ICardLevelRepository>();

            // Services
            var emailServiceMock = new Mock<IEmailVerificationService>();

            // Providers
            var userProviderMock = new Mock<ITeamTunerUserProvider>();

            _userService = new TeamTunerUserService(teamTunerRepositoryMock.Object, cardLevelRepositoryMock.Object, registrationRequestRepositoryMock.Object,
                emailServiceMock.Object, teamMembershipRequestRepositoryMock.Object, unitOfWorkMock.Object, userProviderMock.Object, emailConfigProviderMock.Object);
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