using System;
using System.Text;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Common;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders
{
    internal class TeamTunerUserSeeder : IDbSeeder
    {
        /// <summary>
        ///     Use the service rather then the repository to have password hashing functionality out of the box.
        /// </summary>
        private readonly ITeamTunerUserService _userService;

        private readonly IRepository<TeamTunerUser> _userRepository;

        public TeamTunerUserSeeder(ITeamTunerUserService userService, IRepository<TeamTunerUser> userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        public int Priority => SeederConstants.Priority.TEST_DATA;

        public async Task SeedAsync()
        {
            // System user
            // Add it using the repository so we are able to set fake password and hash so that login becomes impossible with this user
            var systemUser = new SystemUser();
            _userRepository.Add(new TeamTunerUser
                                {
                                    Id = systemUser.Id,
                                    Email = systemUser.Email,
                                    ApplicationRole = CoreConstants.Auth.Roles.SYSTEM,
                                    PasswordHash = Encoding.ASCII.GetBytes("A"),
                                    PasswordSalt = Encoding.ASCII.GetBytes("A"),
                                    Name = systemUser.Name,
                                    SppdName = systemUser.Name
                                });

            // Application admin user
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.ADMIN_ID),
                                            Name = TestingConstants.User.ADMIN_NAME,
                                            SppdName = "App-Admin",
                                            Email = "admin@sppdteamtuner.com",
                                            ApplicationRole = CoreConstants.Auth.Roles.ADMIN
                                        }, TestingConstants.User.ADMIN_PASSWORD_MD5);

            // Team Holy Cow users
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_LEADER_ID),
                                            Name = TestingConstants.User.HOLY_COW_TEAM_LEADER_NAME,
                                            SppdName = "Team-HolyCow-Leader",
                                            Email = "holyCowLeader@sppdteamtuner.com",
                                            ApplicationRole = CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Auth.Roles.LEADER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            CardLevels = new[]
                                                         {
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.TERRANCE_AND_PHILLIP_ID),
                                                                 Level = 44
                                                             }
                                                         }
                                        }, TestingConstants.User.HOLY_COW_TEAM_LEADER_PASSWORD_MD5);
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_ID),
                                            Name = TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_NAME,
                                            SppdName = "Team-HolyCow-CoLeader",
                                            Email = "holyCowCoLeader@sppdteamtuner.com",
                                            ApplicationRole = CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Auth.Roles.CO_LEADER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID)
                                        }, TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_PASSWORD_MD5);
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_MEMBER_ID),
                                            Name = TestingConstants.User.HOLY_COW_TEAM_MEMBER_NAME,
                                            SppdName = "Team-HolyCow-Member",
                                            Email = "holyCowMember@sppdteamtuner.com",
                                            ApplicationRole = CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Auth.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID)
                                        }, TestingConstants.User.HOLY_COW_TEAM_MEMBER_PASSWORD_MD5);

            // Federation Holy users
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_FEDERATION_TEAM_LEADER_ID),
                                            Name = TestingConstants.User.HOLY_FEDERATION_LEADER_NAME,
                                            SppdName = "Federation-Holy-Leader",
                                            Email = "holyLeader@sppdteamtuner.com",
                                            ApplicationRole = CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Auth.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            FederationRole = CoreConstants.Auth.Roles.LEADER
                                        }, TestingConstants.User.HOLY_FEDERATION_LEADER_PASSWORD_MD5);
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_FEDERATION_CO_LEADER_ID),
                                            Name = TestingConstants.User.HOLY_FEDERATION_CO_LEADER_NAME,
                                            SppdName = "Federation-Holy-CoLeader",
                                            Email = "holyCoLeader@sppdteamtuner.com",
                                            ApplicationRole = CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Auth.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            FederationRole = CoreConstants.Auth.Roles.CO_LEADER
                                        }, TestingConstants.User.HOLY_FEDERATION_CO_LEADER_PASSWORD_MD5);
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_FEDERATION_MEMBER_ID),
                                            Name = TestingConstants.User.HOLY_FEDERATION_MEMBER_NAME,
                                            SppdName = "Federation-Holy-Member",
                                            Email = "holyMember@sppdteamtuner.com",
                                            ApplicationRole = CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Auth.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            FederationRole = CoreConstants.Auth.Roles.MEMBER
                                        }, TestingConstants.User.HOLY_FEDERATION_MEMBER_PASSWORD_MD5);
        }
    }
}