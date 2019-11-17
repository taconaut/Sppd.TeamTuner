import { TeamMembershipRequestsClient, TeamMembershipRequestDto } from '@/api'

class TeamMembershipRequestService {
  async sendTeamMembershipRequest(teamId: string, userId: string, comment?: string | undefined) {
    var teamMembershipRequestsClient = new TeamMembershipRequestsClient()

    var request = new TeamMembershipRequestDto()
    request.teamId = teamId
    request.userId = userId
    request.comment = comment

    return teamMembershipRequestsClient.requestMembership(request)
  }

  async getPendingTeamMembershipRequest(userId: string) {
    var teamMembershipRequestsClient = new TeamMembershipRequestsClient()
    return teamMembershipRequestsClient.getPendingTeamMembershipRequest(userId)
  }

  async abortTeamMembershipRequest(membershiprequestId: string) {
    var teamMembershipRequestsClient = new TeamMembershipRequestsClient()
    return teamMembershipRequestsClient.abortMembershipRequest(membershiprequestId)
  }
}

export const teamMembershipRequestService = new TeamMembershipRequestService()