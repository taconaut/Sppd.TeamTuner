# Configuration

## General
| Parameter       | Description                                                                                              |
| --------------- | -------------------------------------------------------------------------------------------------------- |
| EnableSwaggerUI | If set to `true`, Swagger UI will be available at &lt;BaseUrl&gt;/swagger                                |
| EnableHangfire  | If set to `true`, hangfire will be enabled and Hangfire UI will be available at &lt;BaseUrl&gt;/hangfire |

## Database
| Parameter            | Description                                                                                                                                                                                                                                                                                             |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Provider             | The configured provider will determine which database will be used. Available providers are `MsSql`, `Sqlite` and `InMemory`. The in memory DB is only suited for tests.                                                                                                                                |
| ConnectionString     | Specifies the connection to the database. This paramtere wont be used if `Provider: InMemory` has been configured.                                                                                                                                                                                      |
| ManageDatabaseSchema | If set to `true`, the database will automatically be migrated on application startup, if there are pending migrations                                                                                                                                                                                   |
| SeedMode             | Defines the data which will be automatically written to the database if a new one is being created. Following are available: `None`: Do data will be written; `Required`: Required data like Card Types and an Admin user; `Test`: Seeds some data which can be used to develop or run automated tests. |
| DeleteOnStartup      | If set to `true`, the database will be deleted each time the application starts.                                                                                                                                                                                                                        |

## Email
| Parameter            | Description                                                                                                                                                                |
| -------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| IsSendMailEnabled    | If set to `true`, emails like the account verification when a user is being created, or the team membership request email, when a user wants to join a team, will be sent. |
| EmailVerificationUrl | The URL which will be added to the verification mail sent when a user registers. This parameter has no effect if `IsSendMailEnabled: false` has been set.                  |
| SmtpServer           | The SMTP server                                                                                                                                                            |
| Port                 | The SMTP server port                                                                                                                                                       |
| Account              | The SMTP server account name                                                                                                                                               |
| Password             | The SMTP server account passworxd                                                                                                                                          |
| SmtpServer           | The SMTP server                                                                                                                                                            |

## Auth
| Parameter           | Description                                                          |
| ------------------- | -------------------------------------------------------------------- |
| Secret              | The secret to use to salt and hash the password received as MD5 hash |
| TokenExpirationDays | The duration in days, a token will remain valid                      |

## Feinwaru
Cards are being imported from the Feinwaru API. Thanks for letting us use this!
| Parameter         | Description                                                                                                                                                                                     |
| ----------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ImportMode        | How to import cards. Available values are `UpdateAll`: All cards will be imported and updated; `Update`: All cards which don't exist in Sppd.TeamTuner or cards having changed will be updated. |
| ApiUrl            | Base URL of the API                                                                                                                                                                             |
| CardsListEndpoint | The URL, relative to `ApiUrl`, to retrieve the list of cards.                                                                                                                                   |
| CardEndpoint      | The URL, relative to `ApiUrl`, to retrieve the card details.                                                                                                                                    |

## Logging
Default .net core configuration. Specify the wanted levels.