class ConfigurationService {
  public apiUrl: string | undefined;

  public constructor() {
    var configElement = document.getElementById('apiUrl')

    if (configElement !== null) {
      var apiUrlValue = configElement.getAttribute('content')
      this.apiUrl = apiUrlValue !== null ? apiUrlValue : undefined
    }
  }
}

export const configurationService = new ConfigurationService()
