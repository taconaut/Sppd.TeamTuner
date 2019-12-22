import { TeamMembershipRequestsClient, TeamMembershipRequestDto } from '@/api'

class TeamMembershipRequestService {
  private teamMembershipRequestsClient: TeamMembershipRequestsClient | undefined

  async sendTeamMembershipRequest(teamId: string, userId: string, comment?: string | undefined) {
    var request = new TeamMembershipRequestDto()
    request.teamId = teamId
    request.userId = userId
    request.comment = comment

    return this.getTeamMembershipRequestsClient().requestMembership(request)
  }

  async getPendingTeamMembershipRequest(userId: string) {
    return this.getTeamMembershipRequestsClient().getPendingTeamMembershipRequest(userId)
  }

  async abortTeamMembershipRequest(membershiprequestId: string) {
    return this.getTeamMembershipRequestsClient().abortMembershipRequest(membershiprequestId)
  }

  async acceptTeamMembershipRequest(membershipRequestId: string) {
    return this.getTeamMembershipRequestsClient().acceptMembershipRequest(membershipRequestId)
  }

  async rejectTeamMembershipRequest(membershipRequestId: string) {
    return this.getTeamMembershipRequestsClient().rejectMembershipRequest(membershipRequestId)
  }

  private getTeamMembershipRequestsClient() {
    if (!this.teamMembershipRequestsClient) {
      this.teamMembershipRequestsClient = new TeamMembershipRequestsClient()
    }
    return this.teamMembershipRequestsClient
  }
}

export const teamMembershipRequestService = new TeamMembershipRequestService()
