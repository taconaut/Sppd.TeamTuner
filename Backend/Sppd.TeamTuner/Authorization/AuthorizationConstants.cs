namespace Sppd.TeamTuner.Authorization
{
    public struct AuthorizationConstants
    {
        public struct Policies
        {
            // Application
            public const string IS_ADMIN = "IsAdmin";

            // User
            public const string CAN_READ_USER = "CanReadUser";
            public const string CAN_UPDATE_USER = "CanUpdateUser";
            public const string CAN_DELETE_USER = "CanDeleteUser";

            // Team
            public const string CAN_READ_TEAM = "CanReadTeam";
            public const string CAN_UPDATE_TEAM = "CanUpdateTeam";
            public const string CAN_DELETE_TEAM = "CanDeleteTeam";

            // Team membership requests
            public const string CAN_ACCEPT_TEAM_MEMBERSHIP_REQUESTS = "CanAcceptTeamMembershipRequests";
            public const string CAN_ABORT_TEAM_MEMBERSHIP_REQUESTS = "CanAbortTeamMembershipRequests";

            // Federation
            public const string CAN_READ_FEDERATION = "CanReadFederation";
            public const string CAN_UPDATE_FEDERATION = "CanUpdateFederation";
            public const string CAN_DELETE_FEDERATION = "CanDeleteFederation";
        }

        public struct StringLength
        {
            public struct AuthorizationRequestDto
            {
                public const int PASSWORD_MD5 = 32;
            }
        }
    }
}