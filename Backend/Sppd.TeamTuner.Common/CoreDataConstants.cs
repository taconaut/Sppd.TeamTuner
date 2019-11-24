using System;
using System.Collections.Generic;
using System.Text;

namespace Sppd.TeamTuner.Common
{
    public static class CoreDataConstants
    {
        public struct Rarity
        {
            public const string COMMON_ID = "7A8C1943-4CE5-4801-8E05-9D376B9152AD";
            public const string RARE_ID = "C548B645-7F80-4D65-A7EB-24B3C2179DB6";
            public const string EPIC_ID = "B699ED14-F672-46F5-885E-460EE8381802";
            public const string LEGENDARY_ID = "D4A7E01F-C1BB-431C-8687-B4946345DB29";
        }

        public struct Theme
        {
            public const string ADVENTURE_ID = "4AF29C4A-282A-4FB8-8691-9D44398A97F2";
            public const string SCIFI_ID = "5AA42BEE-0C1C-49E7-B402-CC58B19C0DDD";
            public const string MYSTICAL_ID = "BF148B37-4E92-446F-9AB6-B80BF6BA7D05";
            public const string FANTASY_ID = "2A0C0C38-97F1-4CD7-8072-89113FA738DA";
            public const string NEUTRAL_ID = "EA08E01B-1418-43B7-B7D1-926FCAED2A66";
            public const string SUPERHERO_ID = "2584122B-4A69-486B-8CBC-8FC55015B738";
        }

        public struct CardType
        {
            public const string SPELL_ID = "59F49F99-0548-46EC-8F7A-955E8BC057AE";
            public const string SPAWN_ID = "423F055B-581E-4EBC-8B11-14E5E9DDE890";
            public const string CHARACTER_ID = "D4E2E80B-E56E-4D30-9A1C-0AD1944B1B34";
            public const string TRAP_ID = "9B8BCF5E-3757-47CC-AAA0-4B9E77DCF2A7";
        }

        public struct CharacterType
        {
            public const string ASSASSIN_ID = "B43590BB-CE86-450D-B5DD-A6297B11F884";
            public const string MELEE_ID = "0E70F6F6-FF96-48A2-8DA4-AD8798C61FC9";
            public const string RANGED_ID = "BB881B6D-1C1E-4C08-8DD5-E3E528397416";
            public const string TANK_ID = "6C457448-50A9-4181-B643-D695F74018E1";
            public const string TOTEM_ID = "9F114687-F240-4116-B5BD-6CD167A08C1D";
        }
    }
}
