import { TeamsClient, TeamCreateRequestDto, TeamResponseDto, TeamUpdateRequestDto } from '@/api'

class TeamService {
  async searchTeamByName(name: string) {
    const teamsClient = new TeamsClient()
    return teamsClient.searchTeamByName(name)
  }

  async createTeam(name: string) {
    var createRequest = new TeamCreateRequestDto()
    createRequest.name = name

    const teamsClient = new TeamsClient()
    return teamsClient.createTeam(createRequest)
  }

  async getTeam(teamId: string) {
    const teamsClient = new TeamsClient()
    return teamsClient.getTeamById(teamId)
  }

  async updateTeam(team: TeamResponseDto) {
    var propertiesToUpdate = ['Name', 'Description']

    var request = new TeamUpdateRequestDto()
    request.id = team.id
    request.version = team.version
    request.name = team.name
    request.description = team.description

    request.propertiesToUpdate = propertiesToUpdate

    const teamsClient = new TeamsClient()
    return teamsClient.updateTeam(request)
  }
}

export const teamService = new TeamService()
