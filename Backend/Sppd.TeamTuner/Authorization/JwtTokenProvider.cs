using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain.Interfaces;

namespace Sppd.TeamTuner.Authorization
{
    internal class JwtTokenProvider : ITokenProvider
    {
        private readonly Lazy<AuthConfig> _authConfig;

        public JwtTokenProvider(IConfigProvider<AuthConfig> authConfigProvider)
        {
            _authConfig = new Lazy<AuthConfig>(() => authConfigProvider.Config);
        }

        public string GetToken(ITeamTunerUser user)
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

        private static IEnumerable<Claim> BuildClaims(ITeamTunerUser user)
        {
            yield return new Claim(ClaimTypes.Name, user.Id.ToString());
        }
    }
}