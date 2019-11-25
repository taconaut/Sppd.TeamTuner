using System;
using System.Text;
using System.Threading.Tasks;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Enumerations;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;

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

        public async Task SeedAsync(SeedMode seedMode)
        {
            if (seedMode == SeedMode.None)
            {
                return;
            }

            // Application admin user
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(InitializationConstants.User.ADMIN_ID),
                                            Name = InitializationConstants.User.ADMIN_NAME,
                                            SppdName = "App-Admin",
                                            Email = "admin@sppdteamtuner.com",
                                            IsEmailVerified = true,
                                            ApplicationRole = CoreConstants.Authorization.Roles.ADMIN
                                        }, InitializationConstants.User.ADMIN_PASSWORD_MD5);

            if (seedMode != SeedMode.Test)
            {
                return;
            }

            // System user
            // Add it using the repository so we are able to set fake password and hash so that login becomes impossible with this user
            var systemUser = new SystemUser();
            _userRepository.Add(new TeamTunerUser
                                {
                                    Id = systemUser.Id,
                                    Email = systemUser.Email,
                                    IsEmailVerified = true,
                                    ApplicationRole = CoreConstants.Authorization.Roles.SYSTEM,
                                    PasswordHash = Encoding.ASCII.GetBytes("A"),
                                    PasswordSalt = Encoding.ASCII.GetBytes("A"),
                                    Name = systemUser.Name,
                                    SppdName = systemUser.Name
                                });

            // Team Holy Cow users
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_LEADER_ID),
                                            Name = TestingConstants.User.HOLY_COW_TEAM_LEADER_NAME,
                                            SppdName = "Team-HolyCow-Leader",
                                            Email = "holyCowLeader@sppdteamtuner.com",
                                            IsEmailVerified = true,
                                            ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Authorization.Roles.LEADER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            ProfileVisibility = UserProfileVisibility.User,
                                            CardLevels = new[]
                                                         {
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.TERRANCE_AND_PHILLIP_ID),
                                                                 Level = 6,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_LEADER_ID)
                                                             },
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.POISON_ID),
                                                                 Level = 5,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_LEADER_ID)
                                                             },
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.BLOOD_ELF_BEBE_ID),
                                                                 Level = 5,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_LEADER_ID)
                                                             },
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.AWESOMO_ID),
                                                                 Level = 4,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_LEADER_ID)
                                                             }
                                                         }
                                        }, TestingConstants.User.HOLY_COW_TEAM_LEADER_PASSWORD_MD5);
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_ID),
                                            Name = TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_NAME,
                                            SppdName = "Team-HolyCow-CoLeader",
                                            Email = "holyCowCoLeader@sppdteamtuner.com",
                                            IsEmailVerified = true,
                                            ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Authorization.Roles.CO_LEADER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            ProfileVisibility = UserProfileVisibility.Team,
                                            CardLevels = new[]
                                                         {
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.TERRANCE_AND_PHILLIP_ID),
                                                                 Level = 3,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_ID)
                                                             },
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.POISON_ID),
                                                                 Level = 1,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_ID)
                                                             },
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.BLOOD_ELF_BEBE_ID),
                                                                 Level = 6,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_ID)
                                                             },
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.AWESOMO_ID),
                                                                 Level = 3,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_ID)
                                                             }
                                                         }
                                        }, TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_PASSWORD_MD5);
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_MEMBER_ID),
                                            Name = TestingConstants.User.HOLY_COW_TEAM_MEMBER_NAME,
                                            SppdName = "Team-HolyCow-Member",
                                            Email = "holyCowMember@sppdteamtuner.com",
                                            IsEmailVerified = true,
                                            ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Authorization.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            ProfileVisibility = UserProfileVisibility.Federation,
                                            CardLevels = new[]
                                                         {
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.TERRANCE_AND_PHILLIP_ID),
                                                                 Level = 6,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_MEMBER_ID)
                                                             },
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.POISON_ID),
                                                                 Level = 4,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_MEMBER_ID)
                                                             },
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.BLOOD_ELF_BEBE_ID),
                                                                 Level = 1,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_MEMBER_ID)
                                                             },
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.AWESOMO_ID),
                                                                 Level = 1,
                                                                 UserId = Guid.Parse(TestingConstants.User.HOLY_COW_TEAM_MEMBER_ID)
                                                             }
                                                         }
                                        }, TestingConstants.User.HOLY_COW_TEAM_MEMBER_PASSWORD_MD5);

            // Federation Holy users
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_FEDERATION_TEAM_LEADER_ID),
                                            Name = TestingConstants.User.HOLY_FEDERATION_LEADER_NAME,
                                            SppdName = "Federation-Holy-Leader",
                                            Email = "holyLeader@sppdteamtuner.com",
                                            IsEmailVerified = true,
                                            ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Authorization.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            FederationRole = CoreConstants.Authorization.Roles.LEADER
                                        }, TestingConstants.User.HOLY_FEDERATION_LEADER_PASSWORD_MD5);
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_FEDERATION_CO_LEADER_ID),
                                            Name = TestingConstants.User.HOLY_FEDERATION_CO_LEADER_NAME,
                                            SppdName = "Federation-Holy-CoLeader",
                                            Email = "holyCoLeader@sppdteamtuner.com",
                                            IsEmailVerified = true,
                                            ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Authorization.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            FederationRole = CoreConstants.Authorization.Roles.CO_LEADER
                                        }, TestingConstants.User.HOLY_FEDERATION_CO_LEADER_PASSWORD_MD5);
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Id = Guid.Parse(TestingConstants.User.HOLY_FEDERATION_MEMBER_ID),
                                            Name = TestingConstants.User.HOLY_FEDERATION_MEMBER_NAME,
                                            SppdName = "Federation-Holy-Member",
                                            Email = "holyMember@sppdteamtuner.com",
                                            IsEmailVerified = true,
                                            ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW_ID),
                                            TeamRole = CoreConstants.Authorization.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            FederationRole = CoreConstants.Authorization.Roles.MEMBER
                                        }, TestingConstants.User.HOLY_FEDERATION_MEMBER_PASSWORD_MD5);
        }
    }
}