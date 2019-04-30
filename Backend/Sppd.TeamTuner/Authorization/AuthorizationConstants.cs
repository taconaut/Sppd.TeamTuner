namespace Sppd.TeamTuner.Authorization
{
    public struct AuthorizationConstants
    {
        public struct Policies
        {
            public const string IS_ADMIN = "IsAdmin";
            public const string IS_OWNER = "IsOwner";
            public const string IS_IN_TEAM = "IsInTeam";
            public const string IS_IN_FEDERATION = "IsInFederation";
            public const string CAN_ACCEPT_TEAM_MEMBERSHIP_REQUESTS = "CanAcceptTeamJoinRequests";
        }
    }
}