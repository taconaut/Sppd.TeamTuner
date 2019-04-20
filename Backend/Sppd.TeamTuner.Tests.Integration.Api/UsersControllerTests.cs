using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

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

        private static readonly string s_registerRoute = "/Users/register";
        private static readonly string s_loginRoute = "/Users/login";
        private static readonly string s_updateRoute = "/Users";
        private static readonly string s_deleteRoute = $"/Users/{s_userIdPlaceholder}";
        private static readonly string s_getByIdRoute = $"/Users/{s_userIdPlaceholder}";

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
                                    PropertiesToUpdate = new List<string> {nameof(UserUpdateRequestDto.Email)}
                                };
            var loginDto = new UserLoginRequestDto {Name = initialUserDto.Name, PasswordMd5 = initialUserDto.PasswordMd5};

            // userId and token will be set according to API method responses
            Guid userId;
            string token;

            // Act

            // Register
            var registerResponse = await Client.PutAsync(s_registerRoute, TestsHelper.GetStringContent(initialUserDto));
            var registeredUserDto = JsonConvert.DeserializeObject<UserResponseDto>(await registerResponse.Content.ReadAsStringAsync());
            userId = registeredUserDto.Id;

            // Authenticate
            var loginResponse = await Client.PostAsync(s_loginRoute, TestsHelper.GetStringContent(loginDto));
            var authenticatedUserDto = JsonConvert.DeserializeObject<UserLoginResponseDto>(await loginResponse.Content.ReadAsStringAsync());
            token = authenticatedUserDto.Token;

            // Wait a bit to see the ModifiedOnUtc date chance
            Thread.Sleep(TimeSpan.FromSeconds(2));

            // Update and delete
            updateUserDto.Id = userId;

            HttpResponseMessage getUpdatedUserResponse;
            HttpResponseMessage updateResponse;
            HttpResponseMessage deleteResponse;
            HttpResponseMessage getDeletedUserResponse;

            var previousAuthenticationHeaderValue = Client.DefaultRequestHeaders.Authorization;
            UserResponseDto updateUserResponseDto;
            try
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                updateResponse = await Client.PostAsync(s_updateRoute, TestsHelper.GetStringContent(updateUserDto));
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
            Assert.True(loginResponse.IsSuccessStatusCode);
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
            Assert.True(updateUserResponseDto.ModifiedOnUtc > authenticatedUserDto.ModifiedOnUtc);
            Assert.Equal(updateUserResponseDto.CreatedOnUtc, authenticatedUserDto.CreatedOnUtc);
        }
    }
}