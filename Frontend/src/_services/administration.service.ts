import { AdministrationClient } from '@/api'

class AdministrationService {
  private administrationClient: AdministrationClient | undefined
  
  async getSystemInfo() {
    return this.getAdministrationClient().getSystemInfo()
  }

  private getAdministrationClient() {
    if (!this.administrationClient) {
      this.administrationClient = new AdministrationClient()
    }
    return this.administrationClient
  }
}

export const administrationService = new AdministrationService()
