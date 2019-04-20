using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Services
{
    public interface IEmailService
    {
        Task SendJoinRequestNotificationAsync(Guid teamId, TeamJoinRequest joinRequest);
    }
}