using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Sppd.TeamTuner.Core.Domain;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.Jobs
{
    internal class CardImportJob
    {
        private readonly ICardImportService _cardImportService;
        private readonly ITeamTunerUserProvider _userProvider;
        private readonly ILogger<CardImportJob> _logger;

        public CardImportJob(ICardImportService cardImportService, ITeamTunerUserProvider userProvider, ILogger<CardImportJob> logger)
        {
            _cardImportService = cardImportService;
            _userProvider = userProvider;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            // Persist changes as system user
            _userProvider.CurrentUser = new SystemUser();

            try
            {
                await _cardImportService.DoImportAsync();
                _logger.LogInformation("Card import job has been executed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to import cards");
            }
        }
    }
}