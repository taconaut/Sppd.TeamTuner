using System.Collections.Generic;

using Newtonsoft.Json;

namespace Sppd.TeamTuner.Infrastructure.Feinwaru.Domain.Objects
{
    public class CardListResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<CardListCard> Data { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class CardListCard
    {
        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}