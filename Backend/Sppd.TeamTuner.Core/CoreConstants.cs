namespace Sppd.TeamTuner.Core
{
    public struct CoreConstants
    {
        public struct Config
        {
            /// <summary>
            ///     Relative path to the folder containing configurations
            /// </summary>
            public const string CONFIG_FOLDER = "Config";

            /// <summary>
            ///     The log4net configuration file name
            /// </summary>
            // ReSharper disable once InconsistentNaming
            public const string LOG4NET_CONFIG_FILE_NAME = "log4net.config";

            public const string APP_CONFIG_FILE_NAME = "appsettings.json";
        }

        public struct Application
        {
            public const string APP_DLL_PREFIX = "Sppd.TeamTuner";
        }

        public struct Authorization
        {
            public struct Roles
            {
                public const string ADMIN = "Admin";
                public const string LEADER = "Leader";
                public const string CO_LEADER = "CoLeader";
                public const string MEMBER = "Member";
                public const string USER = "User";
                public const string SYSTEM = "System";
            }
        }

        public struct StringLength
        {
            public struct TeamTunerUser
            {
                public const int SPPD_NAME = 50;
                public const int EMAIL = 200;
                public const int APPLICATION_ROLE = 20;
                public const int TEAM_ROLE = 20;
                public const int FEDERATION_ROLE = 20;
            }

            public struct Named
            {
                public const int NAME = 50;
            }

            public struct Descriptive
            {
                public const int DESCRIPTION = 2000;
            }

            public struct Card
            {
                public const int NAME = 50;
                public const int FRIENDLY_NAME = 10;
            }

            public struct TeamJoinRequest
            {
                public const int COMMENT = 500;
            }
        }

        public struct ArrayLength
        {
            public struct TeamTunerUser
            {
                public const int PASSWORD_HASH = 64;
                public const int PASSWORD_SALT = 128;
            }
        }
    }
}