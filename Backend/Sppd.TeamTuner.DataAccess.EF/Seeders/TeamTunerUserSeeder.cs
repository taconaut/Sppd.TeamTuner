using System;
using System.Text;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Utils.Extensions;
using Sppd.TeamTuner.Shared;

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
                                    ApplicationRole = Core.CoreConstants.Auth.Roles.SYSTEM,
                                    PasswordHash = Encoding.ASCII.GetBytes("A"),
                                    PasswordSalt = Encoding.ASCII.GetBytes("A"),
                                    Name = systemUser.Name,
                                    SppdName = systemUser.Name
                                });

            // Application admin user
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Name = "admin",
                                            SppdName = "App-Admin",
                                            Email = "admin@sppdteamtuner.com",
                                            ApplicationRole = Core.CoreConstants.Auth.Roles.ADMIN
                                        }, "admin".Md5Hash());

            // Team Holy Cow users
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Name = "holyCowLeader",
                                            SppdName = "Team-HolyCow-Leader",
                                            Email = "holyCowLeader@sppdteamtuner.com",
                                            ApplicationRole = Core.CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW),
                                            TeamRole = Core.CoreConstants.Auth.Roles.LEADER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            CardLevels = new[]
                                                         {
                                                             new CardLevel
                                                             {
                                                                 CardId = Guid.Parse(TestingConstants.Card.TERRANCE_AND_PHILLIP_ID),
                                                                 Level = 44
                                                             }
                                                         }
                                        }, "holyCowLeader".Md5Hash());
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Name = "holyCowCoLeader",
                                            SppdName = "Team-HolyCow-CoLeader",
                                            Email = "holyCowCoLeader@sppdteamtuner.com",
                                            ApplicationRole = Core.CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW),
                                            TeamRole = Core.CoreConstants.Auth.Roles.CO_LEADER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID)
                                        }, "holyCowCoLeader".Md5Hash());
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Name = "holyCowMember",
                                            SppdName = "Team-HolyCow-Member",
                                            Email = "holyCowMember@sppdteamtuner.com",
                                            ApplicationRole = Core.CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW),
                                            TeamRole = Core.CoreConstants.Auth.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID)
                                        }, "holyCowMember".Md5Hash());

            // Federation Holy users
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Name = "holyLeader",
                                            SppdName = "Federation-Holy-Leader",
                                            Email = "holyLeader@sppdteamtuner.com",
                                            ApplicationRole = Core.CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW),
                                            TeamRole = Core.CoreConstants.Auth.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            FederationRole = Core.CoreConstants.Auth.Roles.LEADER
                                        }, "holyLeader".Md5Hash());
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Name = "holyCoLeader",
                                            SppdName = "Federation-Holy-CoLeader",
                                            Email = "holyCoLeader@sppdteamtuner.com",
                                            ApplicationRole = Core.CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW),
                                            TeamRole = Core.CoreConstants.Auth.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            FederationRole = Core.CoreConstants.Auth.Roles.CO_LEADER
                                        }, "holyCoLeader".Md5Hash());
            await _userService.AddAsync(new TeamTunerUser
                                        {
                                            Name = "holyMember",
                                            SppdName = "Federation-Holy-Member",
                                            Email = "holyMember@sppdteamtuner.com",
                                            ApplicationRole = Core.CoreConstants.Auth.Roles.USER,
                                            TeamId = new Guid(TestingConstants.Team.HOLY_COW),
                                            TeamRole = Core.CoreConstants.Auth.Roles.MEMBER,
                                            FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                            FederationRole = Core.CoreConstants.Auth.Roles.MEMBER
                                        }, "holyMember".Md5Hash());
        }
    }
}