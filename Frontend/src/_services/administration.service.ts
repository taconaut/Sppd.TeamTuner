import { AdministrationClient } from '@/api'

class AdministrationService {
  async getSystemInfo() {
    const administrationClient = new AdministrationClient()
    return administrationClient.getSystemInfo()
  }
}

export const administrationService = new AdministrationService()
