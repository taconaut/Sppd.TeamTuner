using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class RegistrationRequestRepository : Repository<TeamTunerUserRegistrationRequest>, IRegistrationRequestRepository
    {
        public RegistrationRequestRepository(TeamTunerContext context)
            : base(context)
        {
        }

        public async Task<TeamTunerUserRegistrationRequest> GetByEmailAsync(string email)
        {
            try
            {
                return await GetQueryableWithIncludes(new[] {$"{nameof(TeamTunerUserRegistrationRequest.User)}"})
                    .SingleAsync(e => e.User.Email == email);
            }
            catch (InvalidOperationException)
            {
                throw new EntityNotFoundException(typeof(TeamTunerUserRegistrationRequest), email);
            }
        }
    }
}