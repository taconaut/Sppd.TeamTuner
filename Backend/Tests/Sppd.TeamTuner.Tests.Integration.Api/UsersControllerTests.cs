using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Controllers;
using Sppd.TeamTuner.Core.Utils.Extensions;
using Sppd.TeamTuner.DTOs;

using Xunit;

// ReSharper disable JoinDeclarationAndInitializer

namespace Sppd.TeamTuner.Tests.Integration.Api
{
    /// <summary>
    ///     Tests the <see cref="UsersController" />
    /// </summary>
    public class UsersControllerTests
    {
        public UsersControllerTests()
        {
            var testClientProvider = new TestClientProvider();
            Client = testClientProvider.Client;
        }

        private static readonly string s_userIdPlaceholder = "{userId}";

        private static readonly string s_registerRoute = "/users";
        private static readonly string s_authorizeRoute = "/users/authorize";
        private static readonly string s_updateRoute = "/users";
        private static readonly string s_deleteRoute = $"/users/{s_userIdPlaceholder}";
        private static readonly string s_getByIdRoute = $"/users/{s_userIdPlaceholder}";
        private static readonly string s_getCardLevelsRoute = $"/users/{s_userIdPlaceholder}/card-levels";
        private static readonly string s_getGetCardsWithUserLevelsRoute = $"/users/{s_userIdPlaceholder}/cards";

        protected HttpClient Client { get; }

        /// <summary>
        ///     Tests that following API calls work:<br />
        ///     1. Create a user by calling register with a valid user<br />
        ///     2. Log in by calling login with the created user<br />
        ///     3. Update some user fields<br />
        ///     4. Delete the user
        ///     5. Try to access deleted user
        /// </summary>
        [Fact]
        public async Task CreateAuthenticateUpdateDeleteUserAsOwnerIsAuthorizedTest()
        {
            // Arrange
            var initialUserDto = new UserCreateRequestDto
                                 {
                                     Name = "Mr. Slave",
                                     SppdName = "Garrison's bitch",
                                     Email = "slave@nukem.tom",
                                     PasswordMd5 = "SuperSecret".Md5Hash()
                                 };
            var updateUserDto = new UserUpdateRequestDto
                                {
                                    Name = "UnusedUsername",
                                    SppdName = "UnusedSppdName",
                                    Email = "garrisonsbitch@nukem.tom",
                                    PropertiesToUpdate = new HashSet<string> {nameof(UserUpdateRequestDto.Email)}
                                };
            var authorizeDto = new AuthorizationRequestDto {Name = initialUserDto.Name, PasswordMd5 = initialUserDto.PasswordMd5};

            // userId and token will be set according to API method responses
            Guid userId;
            string userVersion;
            string token;

            // Act

            // Register
            var registerResponse = await Client.PostAsync(s_registerRoute, TestsHelper.GetStringContent(initialUserDto));
            var registeredUserDto = JsonConvert.DeserializeObject<UserResponseDto>(await registerResponse.Content.ReadAsStringAsync());
            userId = registeredUserDto.Id;
            userVersion = registeredUserDto.Version;

            // Authenticate
            var authorizeResponse = await Client.PostAsync(s_authorizeRoute, TestsHelper.GetStringContent(authorizeDto));
            var authorizedUserDto = JsonConvert.DeserializeObject<UserAuthorizationResponseDto>(await authorizeResponse.Content.ReadAsStringAsync());
            token = authorizedUserDto.Token;

            // Wait a bit to see the ModifiedOnUtc date chance
            Thread.Sleep(TimeSpan.FromSeconds(2));

            // Update and delete
            updateUserDto.Id = userId;
            updateUserDto.Version = userVersion;

            HttpResponseMessage getUpdatedUserResponse;
            HttpResponseMessage updateResponse;
            HttpResponseMessage deleteResponse;
            HttpResponseMessage getDeletedUserResponse;

            var previousAuthenticationHeaderValue = Client.DefaultRequestHeaders.Authorization;
            UserResponseDto updateUserResponseDto;
            try
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                updateResponse = await Client.PutAsync(s_updateRoute, TestsHelper.GetStringContent(updateUserDto));
                getUpdatedUserResponse = await Client.GetAsync(s_getByIdRoute.Replace(s_userIdPlaceholder, userId.ToString()));
                updateUserResponseDto = JsonConvert.DeserializeObject<UserResponseDto>(await getUpdatedUserResponse.Content.ReadAsStringAsync());
                deleteResponse = await Client.DeleteAsync(s_deleteRoute.Replace(s_userIdPlaceholder, userId.ToString()));
                getDeletedUserResponse = await Client.GetAsync(s_getByIdRoute.Replace(s_userIdPlaceholder, userId.ToString()));
            }
            finally
            {
                Client.DefaultRequestHeaders.Authorization = previousAuthenticationHeaderValue;
            }

            // Assert

            // Call responses
            Assert.True(registerResponse.IsSuccessStatusCode);
            Assert.True(authorizeResponse.IsSuccessStatusCode);
            Assert.True(updateResponse.IsSuccessStatusCode);
            Assert.True(getUpdatedUserResponse.IsSuccessStatusCode);
            Assert.True(deleteResponse.IsSuccessStatusCode);
            Assert.False(getDeletedUserResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, getDeletedUserResponse.StatusCode);

            // User has been correctly updated
            Assert.Equal(updateUserResponseDto.Name, initialUserDto.Name);
            Assert.Equal(updateUserResponseDto.SppdName, initialUserDto.SppdName);
            Assert.Equal(updateUserResponseDto.Email, updateUserDto.Email);
            Assert.Equal(updateUserResponseDto.Name, initialUserDto.Name);
            Assert.Equal(updateUserResponseDto.SppdName, initialUserDto.SppdName);
            Assert.Equal(updateUserResponseDto.Email, updateUserDto.Email);
            Assert.True(updateUserResponseDto.ModifiedOnUtc > authorizedUserDto.ModifiedOnUtc);
            Assert.Equal(updateUserResponseDto.CreatedOnUtc, authorizedUserDto.CreatedOnUtc);
        }

        [Fact]
        public async Task GetCardLevelsTest()
        {
            // Arrange
            var authorizeDto = new AuthorizationRequestDto
                               {
                                   Name = TestingConstants.User.HOLY_COW_TEAM_LEADER_NAME,
                                   PasswordMd5 = TestingConstants.User.HOLY_COW_TEAM_LEADER_PASSWORD_MD5
                               };

            string token;

            // Act

            // Authenticate as user
            var authorizeUserResponse = await Client.PostAsync(s_authorizeRoute, TestsHelper.GetStringContent(authorizeDto));
            var authorizedUserDto = JsonConvert.DeserializeObject<UserAuthorizationResponseDto>(await authorizeUserResponse.Content.ReadAsStringAsync());
            token = authorizedUserDto.Token;

            // Get all users (should fail)
            HttpResponseMessage getCardLevelsResponse;
            IEnumerable<CardLevelResponseDto> cardLevels;
            var previousAuthenticationHeaderValue = Client.DefaultRequestHeaders.Authorization;
            try
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                getCardLevelsResponse = await Client.GetAsync(s_getCardLevelsRoute.Replace(s_userIdPlaceholder, TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_ID));
                cardLevels = JsonConvert.DeserializeObject<IEnumerable<CardLevelResponseDto>>(await getCardLevelsResponse.Content.ReadAsStringAsync())
                                        .ToList();
            }
            finally
            {
                Client.DefaultRequestHeaders.Authorization = previousAuthenticationHeaderValue;
            }

            // Assert
            Assert.True(getCardLevelsResponse.IsSuccessStatusCode);
            Assert.NotEmpty(cardLevels);
            Assert.Contains(cardLevels.Select(c => c.Level), level => 1 <= level && level <= 7);
        }

        [Fact]
        public async Task GetCardsWithUserLevelsTest()
        {
            // Arrange
            var authorizeDto = new AuthorizationRequestDto
                               {
                                   Name = TestingConstants.User.HOLY_COW_TEAM_LEADER_NAME,
                                   PasswordMd5 = TestingConstants.User.HOLY_COW_TEAM_LEADER_PASSWORD_MD5
                               };

            string token;

            // Act

            // Authenticate as user
            var authorizeUserResponse = await Client.PostAsync(s_authorizeRoute, TestsHelper.GetStringContent(authorizeDto));
            var authorizedUserDto = JsonConvert.DeserializeObject<UserAuthorizationResponseDto>(await authorizeUserResponse.Content.ReadAsStringAsync());
            token = authorizedUserDto.Token;

            // Get all users (should fail)
            HttpResponseMessage getCardsWithUserLevels;
            IEnumerable<UserCardResponseDto> cardResponseDtos;
            var previousAuthenticationHeaderValue = Client.DefaultRequestHeaders.Authorization;
            try
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                getCardsWithUserLevels = await Client.GetAsync(s_getGetCardsWithUserLevelsRoute.Replace(s_userIdPlaceholder, TestingConstants.User.HOLY_COW_TEAM_CO_LEADER_ID));
                cardResponseDtos = JsonConvert.DeserializeObject<IEnumerable<UserCardResponseDto>>(await getCardsWithUserLevels.Content.ReadAsStringAsync());
            }
            finally
            {
                Client.DefaultRequestHeaders.Authorization = previousAuthenticationHeaderValue;
            }

            // Assert
            Assert.True(getCardsWithUserLevels.IsSuccessStatusCode);
            Assert.NotEmpty(cardResponseDtos);
        }
    }
}