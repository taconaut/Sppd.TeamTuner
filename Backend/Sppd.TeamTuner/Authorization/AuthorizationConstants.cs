namespace Sppd.TeamTuner.Authorization
{
    public struct AuthorizationConstants
    {
        public struct ClaimTypes
        {
            public const string USER_ID = "UserId";
            public const string APPLICATION_ROLE = "ApplicationRole";
            public const string TEAM_ID = "TeamId";
            public const string TEAM_ROLE = "TeamRole";
            public const string FEDERATION_ID = "FederationId";
            public const string FEDERATION_ROLE = "FederationRole";
        }

        public struct Policies
        {
            public const string IS_ADMIN = "IsAdmin";
            public const string IS_OWNER = "IsOwner";
            public const string IS_IN_TEAM = "IsInTeam";
            public const string IS_IN_FEDERATION = "IsInFederation";
            public const string CAN_ACCEPT_TEAM_JOIN_REQUESTS = "CanAcceptTeamJoinRequests";
        }
    }
}