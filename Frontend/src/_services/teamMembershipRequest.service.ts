import { TeamMembershipRequestsClient, TeamMembershipRequestDto, TeamMembershipRequestResponseDto } from '@/api'

class TeamMembershipRequestService {
  private teamMembershipRequestsClient: TeamMembershipRequestsClient | undefined

  async sendTeamMembershipRequest(teamId: string, userId: string, comment?: string | undefined): Promise<void> {
    var request = new TeamMembershipRequestDto()
    request.teamId = teamId
    request.userId = userId
    request.comment = comment

    return this.getTeamMembershipRequestsClient().requestMembership(request)
  }

  async getPendingTeamMembershipRequest(userId: string): Promise<TeamMembershipRequestResponseDto> {
    return this.getTeamMembershipRequestsClient().getPendingTeamMembershipRequest(userId)
  }

  async abortTeamMembershipRequest(membershiprequestId: string): Promise<void> {
    return this.getTeamMembershipRequestsClient().abortMembershipRequest(membershiprequestId)
  }

  async acceptTeamMembershipRequest(membershipRequestId: string): Promise<void> {
    return this.getTeamMembershipRequestsClient().acceptMembershipRequest(membershipRequestId)
  }

  async rejectTeamMembershipRequest(membershipRequestId: string): Promise<void> {
    return this.getTeamMembershipRequestsClient().rejectMembershipRequest(membershipRequestId)
  }

  private getTeamMembershipRequestsClient(): TeamMembershipRequestsClient {
    if (!this.teamMembershipRequestsClient) {
      this.teamMembershipRequestsClient = new TeamMembershipRequestsClient()
    }
    return this.teamMembershipRequestsClient
  }
}

export const teamMembershipRequestService = new TeamMembershipRequestService()
