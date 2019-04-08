using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Authorization
{
    internal class JwtTokenProvider : ITokenProvider
    {
        private readonly Lazy<AuthConfig> _authConfig;

        public JwtTokenProvider(IConfigProvider<AuthConfig> authConfigProvider)
        {
            _authConfig = new Lazy<AuthConfig>(() => authConfigProvider.Config);
        }

        public string GetToken(TeamTunerUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfig.Value.Secret);
            var claims = BuildClaims(user);
            var tokenDescriptor = new SecurityTokenDescriptor
                                  {
                                      Subject = new ClaimsIdentity(claims),
                                      IssuedAt = DateTime.UtcNow,
                                      Expires = DateTime.UtcNow.AddDays(_authConfig.Value.TokenExpirationDays),
                                      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                                  };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private static IEnumerable<Claim> BuildClaims(TeamTunerUser user)
        {
            yield return new Claim(AuthorizationConstants.ClaimTypes.USER_ID, user.Id.ToString());
            yield return new Claim(AuthorizationConstants.ClaimTypes.APPLICATION_ROLE, user.ApplicationRole);

            if (user.TeamId.HasValue)
            {
                yield return new Claim(AuthorizationConstants.ClaimTypes.TEAM_ID, user.TeamId.ToString());
                yield return new Claim(AuthorizationConstants.ClaimTypes.TEAM_ROLE, user.TeamRole);
            }

            if (user.FederationId.HasValue)
            {
                yield return new Claim(AuthorizationConstants.ClaimTypes.FEDERATION_ID, user.FederationId.ToString());
                if (!string.IsNullOrEmpty(user.FederationRole))
                {
                    yield return new Claim(AuthorizationConstants.ClaimTypes.FEDERATION_ROLE, user.FederationRole);
                }
            }
            else if (user.Team?.FederationId != null)
            {
                // Only set the Id but not the role: The user is implicitly part of the federation through his team, but has no explicit rights on it
                yield return new Claim(AuthorizationConstants.ClaimTypes.FEDERATION_ID, user.Team.FederationId.ToString());
            }
        }
    }
}