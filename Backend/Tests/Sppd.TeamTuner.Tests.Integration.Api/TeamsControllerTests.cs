using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.DTOs;

using Xunit;

namespace Sppd.TeamTuner.Tests.Integration.Api
{
    public class TeamsControllerTests
    {
        public TeamsControllerTests()
        {
            var testClientProvider = new TestClientProvider();
            Client = testClientProvider.Client;
        }

        private static readonly string s_teamIdPlaceholder = "{teamId}";
        private static readonly string s_teamMembershipRequestIdPlaceholder = "{teamMembershipRequestId}";

        private static readonly string s_authorizeRoute = "/users/authorize";
        private static readonly string s_membershipRequestsRoute = "/team-membership-requests";
        private static readonly string s_getTeamHolyCowMembershipRequestsRoute = $"/teams/{s_teamIdPlaceholder}/membership-requests";
        private static readonly string s_getTeamHolyCowUsersRoute = $"/teams/{s_teamIdPlaceholder}/users";
        private static readonly string s_acceptMembershipRequestRoute = $"/team-membership-requests/{s_teamMembershipRequestIdPlaceholder}/accept";

        protected HttpClient Client { get; }

        /// <summary>
        ///     Tests the process to join a team:
        ///     1. Log in as user wanting to join a team
        ///     2. Create a team membership request
        ///     3. Log in as team Co-Leader
        ///     4. Accept the membership request
        ///     5. Make sure the user is now in the team and that the membership request doesn't exists anymore
        /// </summary>
        [Fact]
        public async Task JoinTeamTest()
        {
            // Arrange
            var membershipRequestDto = new TeamMembershipRequestDto
                                       {
                                           Comment = "You look so cool",
                                           TeamId = Guid.Parse(TestingConstants.Team.HOLY_COW_ID),
                                           UserId = Guid.Parse(TestingConstants.User.HOLY_FEDERATION_MEMBER_ID)
                                       };
            var userLoginDto = new AuthorizationRequestDto
                               {
                                   Name = TestingConstants.User.HOLY_FEDERATION_MEMBER_NAME,
                                   PasswordMd5 = TestingConstants.User.HOLY_FEDERATION_MEMBER_PASSWORD_MD5
                               };
            var holyCowCoLeaderLoginDto = new AuthorizationRequestDto
                                          {
                                              Name = TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_NAME,
                                              PasswordMd5 = TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_PASSWORD_MD5
                                          };

            // Act

            // Log in as user wanting to join the team
            var userLoginResponse = await Client.PostAsync(s_authorizeRoute, TestsHelper.GetStringContent(userLoginDto));
            var authenticatedUserDto = JsonConvert.DeserializeObject<UserAuthorizationResponseDto>(await userLoginResponse.Content.ReadAsStringAsync());

            // Authorize co-leader
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticatedUserDto.Token);

            // Create the membership request
            var membershipRequestResponse = await Client.PostAsync(s_membershipRequestsRoute, TestsHelper.GetStringContent(membershipRequestDto));

            // Log in as team co-leader
            var coLeaderLoginResponse = await Client.PostAsync(s_authorizeRoute, TestsHelper.GetStringContent(holyCowCoLeaderLoginDto));
            var authenticatedCoLeaderDto = JsonConvert.DeserializeObject<UserAuthorizationResponseDto>(await coLeaderLoginResponse.Content.ReadAsStringAsync());

            // Authorize co-leader
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticatedCoLeaderDto.Token);

            // Get team membership requests as team co-leader
            var getMembershipRequestsResponse = await Client.GetAsync(s_getTeamHolyCowMembershipRequestsRoute.Replace(s_teamIdPlaceholder, TestingConstants.Team.HOLY_COW_ID));
            var membershipRequests = JsonConvert.DeserializeObject<IEnumerable<TeamMembershipRequestResponseDto>>(await getMembershipRequestsResponse.Content.ReadAsStringAsync());

            // Accept membership request as co-leader
            var membershipRequestId = membershipRequests.Single().Id;
            var acceptMembershipRequestResponse =
                await Client.PostAsync(s_acceptMembershipRequestRoute.Replace(s_teamMembershipRequestIdPlaceholder, membershipRequestId.ToString()), null);

            // Get team membership requests after having accepted the only one; it should be empty
            var getEmptyMembershipRequestsResponse = await Client.GetAsync(s_getTeamHolyCowMembershipRequestsRoute.Replace(s_teamIdPlaceholder, TestingConstants.Team.HOLY_COW_ID));
            var emptyMembershipRequests =
                JsonConvert.DeserializeObject<IEnumerable<TeamMembershipRequestResponseDto>>(await getEmptyMembershipRequestsResponse.Content.ReadAsStringAsync());

            // Get team holy cow users
            var getTeamHolyCowUsers = await Client.GetAsync(s_getTeamHolyCowUsersRoute.Replace(s_teamIdPlaceholder, TestingConstants.Team.HOLY_COW_ID));
            var holyCowUsers = JsonConvert.DeserializeObject<IEnumerable<UserResponseDto>>(await getTeamHolyCowUsers.Content.ReadAsStringAsync());

            // Assert
            Assert.True(userLoginResponse.IsSuccessStatusCode);
            Assert.True(membershipRequestResponse.IsSuccessStatusCode);
            Assert.True(coLeaderLoginResponse.IsSuccessStatusCode);
            Assert.True(getMembershipRequestsResponse.IsSuccessStatusCode);
            Assert.True(acceptMembershipRequestResponse.IsSuccessStatusCode);
            Assert.True(getEmptyMembershipRequestsResponse.IsSuccessStatusCode);
            Assert.True(getTeamHolyCowUsers.IsSuccessStatusCode);

            Assert.Empty(emptyMembershipRequests);
            Assert.Contains(membershipRequestDto.UserId, holyCowUsers.Select(u => u.Id));
        }
    }
}