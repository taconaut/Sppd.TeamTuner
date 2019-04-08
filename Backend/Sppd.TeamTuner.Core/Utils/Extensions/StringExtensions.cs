using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Sppd.TeamTuner.Core.Utils.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="string" />
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Generates an MD5 hash for the given string
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A string containing the MD5 hash of the <see cref="input" /></returns>
        public static string Md5Hash(this string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hash = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        ///     Determines whether an email address is semantically valid.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        ///     <c>true</c> if the specified email address is semantically valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmail(this string email)
        {
            return email != null && new EmailAddressAttribute().IsValid(email);
        }
    }
}