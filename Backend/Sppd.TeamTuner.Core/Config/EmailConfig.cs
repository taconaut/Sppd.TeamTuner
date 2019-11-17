namespace Sppd.TeamTuner.Core.Config
{
    public class EmailConfig : IConfig
    {
        /// <summary>
        ///     If set to <c>True</c>, mails will be sent.
        /// </summary>
        public bool IsSendMailEnabled { get; set; }

        /// <summary>
        ///     the SMTP server to use.
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        ///     The port to use.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///     The account.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        ///     The password.
        /// </summary>
        public string Password { get; set; }

        public string SectionKey => "Email";
    }
}