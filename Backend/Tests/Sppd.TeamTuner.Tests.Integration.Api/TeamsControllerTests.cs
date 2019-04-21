using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Sppd.TeamTuner.DTOs;
using Sppd.TeamTuner.Common;

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

        private static readonly string s_loginRoute = "/Users/login";
        private static readonly string s_requestMembershipRoute = "/Teams/membership/request";
        private static readonly string s_getTeamHolyCowMembershipRequestsRoute = $"/Teams/{s_teamIdPlaceholder}/membership/requests";
        private static readonly string s_getTeamHolyCowUsersRoute = $"/Teams/{s_teamIdPlaceholder}/users";
        private static readonly string s_acceptMembershipRequestRoute = $"/Teams/membership/accept/{s_teamMembershipRequestIdPlaceholder}";

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
            var userLoginDto = new UserLoginRequestDto
                               {
                                   Name = TestingConstants.User.HOLY_FEDERATION_MEMBER_NAME,
                                   PasswordMd5 = TestingConstants.User.HOLY_FEDERATION_MEMBER_PASSWORD_MD5
                               };
            var holyCowCoLeaderLoginDto = new UserLoginRequestDto
                                          {
                                              Name = TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_NAME,
                                              PasswordMd5 = TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_PASSWORD_MD5
                                          };

            // Act

            // Log in as user wanting to join the team
            var userLoginResponse = await Client.PostAsync(s_loginRoute, TestsHelper.GetStringContent(userLoginDto));
            var authenticatedUserDto = JsonConvert.DeserializeObject<UserLoginResponseDto>(await userLoginResponse.Content.ReadAsStringAsync());

            // Authorize co-leader
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticatedUserDto.Token);

            // Create the membership request
            var membershipRequestResponse = await Client.PutAsync(s_requestMembershipRoute, TestsHelper.GetStringContent(membershipRequestDto));

            // Log in as team co-leader
            var coLeaderLoginResponse = await Client.PostAsync(s_loginRoute, TestsHelper.GetStringContent(holyCowCoLeaderLoginDto));
            var authenticatedCoLeaderDto = JsonConvert.DeserializeObject<UserLoginResponseDto>(await coLeaderLoginResponse.Content.ReadAsStringAsync());

            // Authorize co-leader
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticatedCoLeaderDto.Token);

            // Get team join requests as team co-leader
            var getMembershipRequestsResponse = await Client.GetAsync(s_getTeamHolyCowMembershipRequestsRoute.Replace(s_teamIdPlaceholder, TestingConstants.Team.HOLY_COW_ID));
            var membershipRequests = JsonConvert.DeserializeObject<IEnumerable<TeamMembershipRequestResponseDto>>(await getMembershipRequestsResponse.Content.ReadAsStringAsync());

            // Accept join request as co-leader
            var membershipRequestId = membershipRequests.Single().Id;
            var acceptMembershipRequestResponse =
                await Client.PostAsync(s_acceptMembershipRequestRoute.Replace(s_teamMembershipRequestIdPlaceholder, membershipRequestId.ToString()), null);

            // Get team join requests after having accepted the only one; it should be empty
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