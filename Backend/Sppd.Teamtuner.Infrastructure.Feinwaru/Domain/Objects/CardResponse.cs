using System.Collections.Generic;

using Newtonsoft.Json;

namespace Sppd.TeamTuner.Infrastructure.Feinwaru.Domain.Objects
{
    public class CardResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public Card Card { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class Card
    {
        [JsonProperty("aliases")]
        public List<string> Aliases { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("mana_cost")]
        public int ManaCost { get; set; }

        [JsonProperty("damage")]
        public long Damage { get; set; }

        [JsonProperty("health")]
        public long Health { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("character_type")]
        public string CharacterType { get; set; }

        [JsonProperty("rarity")]
        public int Rarity { get; set; }

        [JsonProperty("theme")]
        public string Theme { get; set; }

        [JsonProperty("health_loss")]
        public object HealthLoss { get; set; }

        [JsonProperty("cast_area")]
        public string CastArea { get; set; }

        [JsonProperty("max_velocity")]
        public double? MaxVelocity { get; set; }

        [JsonProperty("time_to_reach_max_velocity")]
        public double? TimeToReachMaxVelocity { get; set; }

        [JsonProperty("agro_range_multiplier")]
        public double? AgroRangeMultiplier { get; set; }

        [JsonProperty("can_attack")]
        public bool? CanAttack { get; set; }

        [JsonProperty("attack_range")]
        public double? AttackRange { get; set; }

        [JsonProperty("pre_attack_delay")]
        public double? PreAttackDelay { get; set; }

        [JsonProperty("knockback")]
        public long? Knockback { get; set; }

        [JsonProperty("knockback_angle")]
        public long? KnockbackAngle { get; set; }

        [JsonProperty("time_between_attacks")]
        public double? TimeBetweenAttacks { get; set; }

        [JsonProperty("has_aoe")]
        public bool HasAoe { get; set; }

        [JsonProperty("aoe_type")]
        public object AoeType { get; set; }

        [JsonProperty("aoe_damage_percentage")]
        public object AoeDamagePercentage { get; set; }

        [JsonProperty("aoe_knockback_percentage")]
        public object AoeKnockbackPercentage { get; set; }

        [JsonProperty("aoe_radius")]
        public object AoeRadius { get; set; }

        [JsonProperty("min_episode_completed")]
        public long MinEpisodeCompleted { get; set; }

        [JsonProperty("min_pvp_rank")]
        public long MinPvpRank { get; set; }

        [JsonProperty("min_player_level")]
        public long MinPlayerLevel { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("powers")]
        public List<Power> Powers { get; set; }
    }

    public class Power
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("amount")]
        public long? Amount { get; set; }

        [JsonProperty("duration")]
        public double? Duration { get; set; }

        [JsonProperty("is_charged")]
        public bool IsCharged { get; set; }

        [JsonProperty("charged_regen")]
        public double? ChargedRegen { get; set; }

        [JsonProperty("radius")]
        public long? Radius { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }
    }
}