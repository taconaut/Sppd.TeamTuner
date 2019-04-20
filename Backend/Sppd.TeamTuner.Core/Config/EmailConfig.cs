namespace Sppd.TeamTuner.Core.Config
{
    public class EmailConfig : IConfig
    {
        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string SectionKey => "Email";
    }
}