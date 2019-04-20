using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

namespace Sppd.TeamTuner.Tests.Integration.Api
{
    internal static class TestsHelper
    {
        private static readonly Encoding s_stringContentEncoding = Encoding.UTF8;
        private static readonly string s_stringContentMediaType = "application/json";

        public static HttpContent GetStringContent<TDto>(TDto dto)
        {
            return new StringContent(JsonConvert.SerializeObject(dto), s_stringContentEncoding, s_stringContentMediaType);
        }
    }
}