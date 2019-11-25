using System;
using System.Globalization;
using System.Reflection;

using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.Services
{
    internal class AdministrationService : IAdministrationService
    {
        private SystemInfo _systemInfo;

        public SystemInfo GetSystemInfo()
        {
            if (_systemInfo == null)
            {
                // Lazy initialize and cache the system info the first time this method is being called.
                var versionString = GetRuntimeVersion();
                var elements = versionString.Split('-');

                var version = new Version(elements[0]);
                var gitCommitHash = elements.Length > 1 ? elements[1] : null;

                DateTime? buildTimeUtc = null;
                if (elements.Length > 2)
                {
                    // ReSharper disable once StringLiteralTypo
                    var dateTimeFormat = "yyyyMMddHHmmss";
                    var style = DateTimeStyles.AdjustToUniversal;
                    var provider = CultureInfo.InvariantCulture;
                    if (DateTime.TryParseExact(elements[2], dateTimeFormat, provider, style, out var parsedBuildTime))
                    {
                        buildTimeUtc = DateTime.SpecifyKind(parsedBuildTime, DateTimeKind.Utc);
                    }
                }

                _systemInfo = new SystemInfo
                              {
                                  Version = version,
                                  GitCommitHash = gitCommitHash,
                                  BuildTimeUtc = buildTimeUtc
                              };
            }

            return _systemInfo;
        }

        private string GetRuntimeVersion()
        {
            return GetType()
                   .GetTypeInfo()
                   .Assembly
                   .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                   .InformationalVersion;
        }
    }
}