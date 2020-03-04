import { TeamsClient, TeamCreateRequestDto, TeamResponseDto, TeamUpdateRequestDto, TeamMembershipRequestResponseDto, UserResponseDto } from '@/api'

class TeamService {
  private teamsClient: TeamsClient | undefined;

  async searchTeamByName(name: string): Promise<TeamResponseDto[]> {
    return this.getTeamsClient().searchTeamByName(name)
  }

  async createTeam(name: string): Promise<TeamResponseDto> {
    var createRequest = new TeamCreateRequestDto()
    createRequest.name = name

    return this.getTeamsClient().createTeam(createRequest)
  }

  async getTeam(teamId: string): Promise<TeamResponseDto> {
    return this.getTeamsClient().getTeamById(teamId)
  }

  async updateTeam(team: TeamResponseDto): Promise<TeamResponseDto> {
    var propertiesToUpdate = ['Name', 'Description']

    var request = new TeamUpdateRequestDto()
    request.id = team.id
    request.version = team.version
    request.name = team.name
    request.description = team.description

    request.propertiesToUpdate = propertiesToUpdate

    return this.getTeamsClient().updateTeam(request)
  }

  async getPendingMembershipRequests(teamId: string): Promise<TeamMembershipRequestResponseDto[]> {
    return this.getTeamsClient().getTeamMembershipRequests(teamId)
  }

  async getMembers(teamId: string): Promise<UserResponseDto[]> {
    return this.getTeamsClient().getTeamMembers(teamId)
  }

  async updateUserTeamRole(teamId: string, userId: string, role: string): Promise<string> {
    return this.getTeamsClient().updateMemberTeamRole(teamId, userId, role)
  }

  async removeMember(teamId: string, userId: string): Promise<void> {
    await this.getTeamsClient().removeMember(teamId, userId)
  }

  private getTeamsClient(): TeamsClient {
    if (!this.teamsClient) {
      this.teamsClient = new TeamsClient()
    }
    return this.teamsClient
  }
}

export const teamService = new TeamService()
