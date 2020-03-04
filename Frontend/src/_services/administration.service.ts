import { AdministrationClient, SystemInfoDto } from '@/api'

class AdministrationService {
  private administrationClient: AdministrationClient | undefined

  async getSystemInfo(): Promise<SystemInfoDto> {
    return this.getAdministrationClient().getSystemInfo()
  }

  private getAdministrationClient(): AdministrationClient {
    if (!this.administrationClient) {
      this.administrationClient = new AdministrationClient()
    }
    return this.administrationClient
  }
}

export const administrationService = new AdministrationService()
