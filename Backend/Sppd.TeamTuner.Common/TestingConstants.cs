namespace Sppd.TeamTuner.Common
{
    /// <summary>
    ///     Contains constants used for seeding as well as for testing
    /// </summary>
    public static class TestingConstants
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
        }

        public struct Team
        {
            public const string HOLY_COW_ID = "1E45C5AE-9DA1-4B7D-AC75-72DAA2EE1D3C";
            public const string HOLY_SHIT_ID = "633FDF30-6C76-4961-A299-4110F9E28823";
            public const string UNHOLY_ID = "699A68FE-8546-4537-9219-37E4A5F03800";
        }

        public struct Federation
        {
            public const string HOLY_ID = "F442DEAA-0490-4C78-9EC9-3A5351974319";
        }

        public struct Card
        {
            // Neutral
            public const string TERRANCE_AND_PHILLIP_ID = "508EAA58-C124-4B44-89AF-106BF87BDD31";
            public const string NATHAN_ID = "4C66C0D8-4337-4460-A2A6-73218B1779D9";
            public const string DOGPOO_ID = "FA6DF38A-7CA7-4BA4-8C1C-DE384B9B2907";
            public const string SATAN_ID = "1E8B2B8F-944D-47BF-91F5-EA9F8DA69BDC";

            // Sci-Fy
            public const string ROBO_BEBE_ID = "45F793AF-891E-4E0A-9D29-42182C19B329";
            public const string ENFORCER_JIMMY_ID = "A55C6899-D7D2-40C1-BED5-723498C311E0";
            public const string AWESOMO_ID = "BFD3AA46-0DF3-4068-BAAF-E5157DD5B36E";
            public const string MECHA_TIMMY_ID = "F302C489-C694-486E-BD6F-302F98396FFC";
            public const string POISON_ID = "57D3A828-4073-4FF7-91FE-28B5300B6353";

            // Fantasy
            public const string BLOOD_ELF_BEBE_ID = "22765182-AEB0-41F0-ABD0-8D7AA3B3DD7F";
            public const string CANADIAN_KNIGHT_IKE_ID = "1A745BF7-FC29-4C74-8C76-C0FB64923856";
            public const string ROGUE_TOKEN_ID = "CEEB4205-4174-42B9-BE6A-D885A17F3F17";
            public const string GRAND_WIZARD_CARTMAN_ID = "D32F3AE0-E32E-4CC3-927A-8B681CB7ACE1";
        }

        public struct CardType
        {
            public const string ASSASSIN_ID = "59F49F99-0548-46EC-8F7A-955E8BC057AE";
            public const string TANK_ID = "86DD3114-D105-4C1C-810F-F90A4A96AFF5";
            public const string RANGED_ID = "423F055B-581E-4EBC-8B11-14E5E9DDE890";
            public const string FIGHTER_ID = "D4E2E80B-E56E-4D30-9A1C-0AD1944B1B34";
            public const string SPELL_ID = "5FCF292E-5C80-46F3-BA84-D8D8A4351C86";
        }

        public struct User
        {
            // Application users
            public const string ADMIN_ID = "6B4EFEAC-25E2-4708-9C36-1D4E65E3A95A";
            public const string ADMIN_NAME = "Admin";
            public const string ADMIN_PASSWORD_MD5 = "E3AFED0047B08059D0FADA10F400C1E5";

            // Team Holy Cow users
            public const string HOLY_COW_TEAM_LEADER_ID = "7B61D02B-B55A-4E25-A7F3-B25CF440D27B";
            public const string HOLY_COW_TEAM_LEADER_NAME = "HolyCowTeamLeader";
            public const string HOLY_COW_TEAM_LEADER_PASSWORD_MD5 = "0DB702DC2C5DD56DB04BDCA38989E83D";

            public const string HOLY_COW_TEAM_CO_LEADER_ID = "1DD7B2FC-2AC9-4C0A-A782-428D8E03AD39";
            public const string HOLY_COW_TEAM_CO_LEADER_NAME = "HolyCowTeamCoLeader";
            public const string HOLY_COW_TEAM_CO_LEADER_PASSWORD_MD5 = "2CF49AFFE638409204B9BA8D2E7A252C";

            public const string HOLY_COW_TEAM_MEMBER_ID = "2332B805-87DD-4D31-8266-F147568C82D1";
            public const string HOLY_COW_TEAM_MEMBER_NAME = "HolyCowTeamMember";
            public const string HOLY_COW_TEAM_MEMBER_PASSWORD_MD5 = "7FECFBAB110DE4143DD77AE9A7F8064F";

            // Federation Holy users
            public const string HOLY_FEDERATION_TEAM_LEADER_ID = "D786A5D7-9F0A-4CBE-AA30-30B2D8E2FB6E";
            public const string HOLY_FEDERATION_LEADER_NAME = "HolyFederationLeader";
            public const string HOLY_FEDERATION_LEADER_PASSWORD_MD5 = "C07A1AC1F2BE85C0C865CD96058C3ED7";

            public const string HOLY_FEDERATION_CO_LEADER_ID = "E4E52E97-2ACA-453B-A824-93C311EA948E";
            public const string HOLY_FEDERATION_CO_LEADER_NAME = "HolyFederationCoLeader";
            public const string HOLY_FEDERATION_CO_LEADER_PASSWORD_MD5 = "8FD22BA964871DF8CD33F23AEF6B7C7E";

            public const string HOLY_FEDERATION_MEMBER_ID = "233143E0-50A7-4376-87C3-0A814E6827D3";
            public const string HOLY_FEDERATION_MEMBER_NAME = "HolyFederationMember";
            public const string HOLY_FEDERATION_MEMBER_PASSWORD_MD5 = "C9D9FFF071756B36CD31F9CFBA7F5001";
        }
    }
}