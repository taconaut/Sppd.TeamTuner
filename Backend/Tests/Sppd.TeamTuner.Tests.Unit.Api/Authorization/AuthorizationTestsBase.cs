using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Enumerations;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Abstract base class for authorization tests. It sets up
    /// </summary>
    public abstract class AuthorizationTestsBase
    {
        #region Constructors

        protected AuthorizationTestsBase()
        {
            InitializeUsers();
            BuildServiceProvider();
        }

        #endregion

        #region  Private Classes

        private class TeamTunerUserProvider : ITeamTunerUserProvider
        {
            public ITeamTunerUser CurrentUser { get; set; }
        }

        #endregion

        #region Protected Properties

        protected IAuthorizationService AuthorizationService => _authorizationService ?? (_authorizationService = _serviceProvider.GetRequiredService<IAuthorizationService>());

        /// <summary>
        ///     Gets the user who is Member of Team1 with <see cref="ITeamTunerUser.ProfileVisibility" /> =
        ///     <see cref="UserProfileVisibility.User" />.
        /// </summary>
        protected ITeamTunerUser MemberTeam1 => _users[s_memberTeam1Id];

        /// <summary>
        ///     Gets the user who is Co-Leader of Team1 with <see cref="ITeamTunerUser.ProfileVisibility" /> =
        ///     <see cref="UserProfileVisibility.Team" />.
        /// </summary>
        protected ITeamTunerUser CoLeaderTeam1 => _users[s_coLeaderTeam1Id];

        /// <summary>
        ///     Gets the second user who is Co-Leader of Team1 with <see cref="ITeamTunerUser.ProfileVisibility" /> =
        ///     <see cref="UserProfileVisibility.Team" />.
        /// </summary>
        protected ITeamTunerUser CoLeader2Team1 => _users[s_coLeader2Team1Id];

        /// <summary>
        ///     Gets the user who is Leader of Team1 with <see cref="ITeamTunerUser.ProfileVisibility" /> =
        ///     <see cref="UserProfileVisibility.Team" />.
        /// </summary>
        protected ITeamTunerUser LeaderTeam1 => _users[s_leaderTeam1Id];

        /// <summary>
        ///     Gets the user who is Member of Team2 with <see cref="ITeamTunerUser.ProfileVisibility" /> =
        ///     <see cref="UserProfileVisibility.User" />.
        /// </summary>
        protected ITeamTunerUser MemberTeam2 => _users[s_memberTeam2Id];

        /// <summary>
        ///     Gets the user who is Co-Leader of Team2 with <see cref="ITeamTunerUser.ProfileVisibility" /> =
        ///     <see cref="UserProfileVisibility.Team" />.
        /// </summary>
        protected ITeamTunerUser CoLeaderTeam2 => _users[s_coLeaderTeam2Id];

        /// <summary>
        ///     Gets the Leader being part of Team2 with <see cref="ITeamTunerUser.ProfileVisibility" /> =
        ///     <see cref="UserProfileVisibility.Team" />.
        /// </summary>
        protected ITeamTunerUser LeaderTeam2 => _users[s_memberTeam2Id];

        /// <summary>
        ///     Gets the admin user.
        /// </summary>
        protected ITeamTunerUser AdminUser => _users[s_adminId];

        #endregion

        #region Private Fields

        private static readonly Guid s_adminId = Guid.Parse("29B03E01-E6E2-4E5C-A74F-EF23DC309C84");

        private static readonly Guid s_memberTeam1Id = Guid.Parse("EBB80828-3D19-48B9-83DA-B359D516E925");
        private static readonly Guid s_coLeaderTeam1Id = Guid.Parse("C7465B59-FB72-4C90-BDC8-4A15BD55FA28");
        private static readonly Guid s_coLeader2Team1Id = Guid.Parse("9E5F38E6-3838-445F-AAC3-5DD6C424FA13");
        private static readonly Guid s_leaderTeam1Id = Guid.Parse("A90771A5-1627-40A2-A56A-6DEAFC4E1E4B");

        private static readonly Guid s_memberTeam2Id = Guid.Parse("A4FD6875-C979-4DA2-9E62-10658EDBF24A");
        private static readonly Guid s_coLeaderTeam2Id = Guid.Parse("1C8A1109-EDF5-441E-8BCA-16326E9C25A3");
        private static readonly Guid s_leaderTeam2Id = Guid.Parse("2D4DC562-6E4C-4419-ABB9-1C333D83F5AC");

        private static readonly Guid s_team1Id = Guid.Parse("1889AF22-ABDF-47C2-9B53-6D61D72F730B");
        private static readonly Guid s_team2Id = Guid.Parse("60698462-6331-4A52-927A-99637F42460D");

        private IDictionary<Guid, TeamTunerUser> _users;
        private IServiceProvider _serviceProvider;
        private IAuthorizationService _authorizationService;
        private ITeamTunerUserProvider _userProvider;

        private ITeamTunerUserProvider UserProvider => _userProvider ?? (_userProvider = _serviceProvider.GetRequiredService<ITeamTunerUserProvider>());

        #endregion

        #region Protected Methods

        protected AuthorizationRequest<TResource> GetAuthorizationRequest<TResource>(TResource parameter)
        {
            return new AuthorizationRequest<TResource>(_serviceProvider, parameter);
        }

        protected void SetCurrentUser(ITeamTunerUser teamTunerUser)
        {
            UserProvider.CurrentUser = teamTunerUser;
        }

        protected static ClaimsPrincipal GetCurrentUser()
        {
            return new ClaimsPrincipal();
        }

        #endregion

        #region Private Methods

        private void InitializeUsers()
        {
            _users = new Dictionary<Guid, TeamTunerUser>
                     {
                         // Admin
                         {
                             s_adminId,
                             new TeamTunerUser
                             {
                                 Id = s_adminId,
                                 Name = "Admin",
                                 ApplicationRole = CoreConstants.Authorization.Roles.ADMIN
                             }
                         },

                         // Team 1
                         {
                             s_memberTeam1Id,
                             new TeamTunerUser
                             {
                                 Id = s_memberTeam1Id,
                                 Name = "MemberTeam1",
                                 ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                 ProfileVisibility = UserProfileVisibility.User,
                                 TeamId = s_team1Id,
                                 TeamRole = CoreConstants.Authorization.Roles.MEMBER
                             }
                         },
                         {
                             s_coLeaderTeam1Id,
                             new TeamTunerUser
                             {
                                 Id = s_coLeaderTeam1Id,
                                 Name = "CoLeaderTeam1",
                                 ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                 ProfileVisibility = UserProfileVisibility.Team,
                                 TeamId = s_team1Id,
                                 TeamRole = CoreConstants.Authorization.Roles.CO_LEADER
                             }
                         },
                         {
                             s_coLeader2Team1Id,
                             new TeamTunerUser
                             {
                                 Id = s_coLeader2Team1Id,
                                 Name = "CoLeader2Team1",
                                 ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                 ProfileVisibility = UserProfileVisibility.Team,
                                 TeamId = s_team1Id,
                                 TeamRole = CoreConstants.Authorization.Roles.CO_LEADER
                             }
                         },
                         {
                             s_leaderTeam1Id,
                             new TeamTunerUser
                             {
                                 Id = s_leaderTeam1Id,
                                 Name = "LeaderTeam1",
                                 ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                 ProfileVisibility = UserProfileVisibility.Team,
                                 TeamId = s_team1Id,
                                 TeamRole = CoreConstants.Authorization.Roles.LEADER
                             }
                         },

                         // Team 2
                         {
                             s_memberTeam2Id,
                             new TeamTunerUser
                             {
                                 Id = s_memberTeam2Id,
                                 Name = "MemberTeam2",
                                 ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                 ProfileVisibility = UserProfileVisibility.User,
                                 TeamId = s_team2Id,
                                 TeamRole = CoreConstants.Authorization.Roles.MEMBER
                             }
                         },
                         {
                             s_coLeaderTeam2Id,
                             new TeamTunerUser
                             {
                                 Id = s_coLeaderTeam2Id,
                                 Name = "CoLeaderTeam2",
                                 ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                 ProfileVisibility = UserProfileVisibility.Team,
                                 TeamId = s_team2Id,
                                 TeamRole = CoreConstants.Authorization.Roles.CO_LEADER
                             }
                         },
                         {
                             s_leaderTeam2Id,
                             new TeamTunerUser
                             {
                                 Id = s_leaderTeam2Id,
                                 Name = "LeaderTeam2",
                                 ApplicationRole = CoreConstants.Authorization.Roles.USER,
                                 ProfileVisibility = UserProfileVisibility.Team,
                                 TeamId = s_team2Id,
                                 TeamRole = CoreConstants.Authorization.Roles.LEADER
                             }
                         }
                     };
        }

        private void BuildServiceProvider()
        {
            var services = new ServiceCollection();

            // Required framework services
            services.AddLogging();
            services.AddOptions();

            // The policies contain the code we want to test
            services.AddAuthorization(options => options.AddPolicies());

            // The policies rely on below services to authorize
            services.AddScoped<ITeamTunerUserProvider, TeamTunerUserProvider>();
            services.AddScoped(provider => BuildRepositoryResolver());

            _serviceProvider = services.BuildServiceProvider();
        }

        private IRepositoryResolver BuildRepositoryResolver()
        {
            var mock = new Mock<IRepositoryResolver>();
            mock.Setup(resolver => resolver.ResolveFor<TeamTunerUser>())
                .Returns(BuildTeamTunerUserRepository);

            return mock.Object;
        }

        private ITeamTunerUserRepository BuildTeamTunerUserRepository()
        {
            var mock = new Mock<ITeamTunerUserRepository>();
            foreach (var (userId, user) in _users)
            {
                mock.Setup(repository => repository.GetAsync(userId, null))
                    .Returns(() => Task.FromResult(user));
            }

            return mock.Object;
        }

        #endregion
    }
}