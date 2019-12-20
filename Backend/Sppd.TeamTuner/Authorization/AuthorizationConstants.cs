namespace Sppd.TeamTuner.Authorization
{
    public struct AuthorizationConstants
    {
        public struct Policies
        {
            // User
            public const string CAN_READ_USER = "CanReadUser";
            public const string CAN_UPDATE_USER = "CanUpdateUser";
            public const string CAN_DELETE_USER = "CanDeleteUser";

            // Team
            public const string CAN_READ_TEAM = "CanReadTeam";
            public const string CAN_UPDATE_TEAM = "CanUpdateTeam";
            public const string CAN_DELETE_TEAM = "CanDeleteTeam";
            public const string CAN_UPDATE_USER_TEAM_ROLE = "CanUpdateUserTeamRole";
            public const string CAN_REMOVE_TEAM_MEMBER = "CanRemoveTeamMember";

            // Team membership requests
            public const string CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS = "CanAcceptTeamMembershipRequests";
            public const string CAN_ABORT_TEAM_MEMBERSHIP_REQUESTS = "CanAbortTeamMembershipRequests";
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