using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Sppd.TeamTuner.DTOs;
using Sppd.TeamTuner.Shared;

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
        private static readonly string s_teamJoinRequestIdPlaceholder = "{teamJoinRequestId}";

        private static readonly string s_loginRoute = "/Users/login";
        private static readonly string s_requestJoinRoute = "/Teams/requestJoin";
        private static readonly string s_getTeamHolyCowJoinRequestsRoute = $"/Teams/{s_teamIdPlaceholder}/joinRequests";
        private static readonly string s_getTeamHolyCowUsersRoute = $"/Teams/{s_teamIdPlaceholder}/users";
        private static readonly string s_acceptJoinRequestRoute = $"/Teams/acceptJoin/{s_teamJoinRequestIdPlaceholder}";

        protected HttpClient Client { get; }

        /// <summary>
        ///     Tests the process to join a team:
        ///     1. Log in as user wanting to join a team
        ///     2. Create a team join request
        ///     3. Log in as team Co-Leader
        ///     4. Accept the join request
        ///     5. Make sure the user is now in the team and that the join request doesn't exists anymore
        /// </summary>
        [Fact]
        public async Task JoinTeamTest()
        {
            // Arrange
            var joinRequestDto = new TeamJoinRequestDto
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

            // Create the join request
            var joinRequestResponse = await Client.PutAsync(s_requestJoinRoute, TestsHelper.GetStringContent(joinRequestDto));

            // Log in as team co-leader
            var coLeaderLoginResponse = await Client.PostAsync(s_loginRoute, TestsHelper.GetStringContent(holyCowCoLeaderLoginDto));
            var authenticatedCoLeaderDto = JsonConvert.DeserializeObject<UserLoginResponseDto>(await coLeaderLoginResponse.Content.ReadAsStringAsync());

            // Authorize co-leader
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticatedCoLeaderDto.Token);

            // Get team join requests as team co-leader
            var getJoinRequestsResponse = await Client.GetAsync(s_getTeamHolyCowJoinRequestsRoute.Replace(s_teamIdPlaceholder, TestingConstants.Team.HOLY_COW_ID));
            var joinRequests = JsonConvert.DeserializeObject<IEnumerable<TeamJoinRequestResponseDto>>(await getJoinRequestsResponse.Content.ReadAsStringAsync());

            // Accept join request as co-leader
            var joinRequestId = joinRequests.Single().Id;
            var acceptJoinRequestResponse = await Client.PostAsync(s_acceptJoinRequestRoute.Replace(s_teamJoinRequestIdPlaceholder, joinRequestId.ToString()), null);

            // Get team join requests after having accepted the only one; it should be empty
            var getEmptyJoinRequestsResponse = await Client.GetAsync(s_getTeamHolyCowJoinRequestsRoute.Replace(s_teamIdPlaceholder, TestingConstants.Team.HOLY_COW_ID));
            var emptyJoinRequests = JsonConvert.DeserializeObject<IEnumerable<TeamJoinRequestResponseDto>>(await getEmptyJoinRequestsResponse.Content.ReadAsStringAsync());

            // Get team holy cow users
            var getTeamHolyCowUsers = await Client.GetAsync(s_getTeamHolyCowUsersRoute.Replace(s_teamIdPlaceholder, TestingConstants.Team.HOLY_COW_ID));
            var holyCowUsers = JsonConvert.DeserializeObject<IEnumerable<UserResponseDto>>(await getTeamHolyCowUsers.Content.ReadAsStringAsync());

            // Assert
            Assert.True(userLoginResponse.IsSuccessStatusCode);
            Assert.True(joinRequestResponse.IsSuccessStatusCode);
            Assert.True(coLeaderLoginResponse.IsSuccessStatusCode);
            Assert.True(getJoinRequestsResponse.IsSuccessStatusCode);
            Assert.True(acceptJoinRequestResponse.IsSuccessStatusCode);
            Assert.True(getEmptyJoinRequestsResponse.IsSuccessStatusCode);
            Assert.True(getTeamHolyCowUsers.IsSuccessStatusCode);

            Assert.Empty(emptyJoinRequests);
            Assert.Contains(joinRequestDto.UserId, holyCowUsers.Select(u => u.Id));
        }
    }
}