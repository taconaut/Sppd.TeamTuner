namespace Sppd.TeamTuner.DTOs
{
    public class UserLoginRequestDto
    {
        public string Name { get; set; }

        public string PasswordMd5 { get; set; }
    }
}