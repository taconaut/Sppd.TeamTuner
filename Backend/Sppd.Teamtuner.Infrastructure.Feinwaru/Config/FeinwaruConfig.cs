using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Infrastructure.Feinwaru.Domain.Enumerations;

namespace Sppd.TeamTuner.Infrastructure.Feinwaru.Config
{
    internal class FeinwaruConfig : IConfig
    {
        public CardImportMode ImportMode { get; set; }

        public string ApiUrl { get; set; }

        public string CardsListEndpoint { get; set; }

        public string CardEndpoint { get; set; }

        public string SectionKey => "Feinwaru";
    }
}