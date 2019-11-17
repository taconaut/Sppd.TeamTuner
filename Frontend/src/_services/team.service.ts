import { TeamsClient, TeamCreateRequestDto } from '@/api'

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
}

export const teamService = new TeamService()
