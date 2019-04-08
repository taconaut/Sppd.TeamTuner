namespace Sppd.TeamTuner.DTOs
{
    public class UserCreateDto
    {
        public string Name { get; set; }

        public string SppdName { get; set; }

        public string Email { get; set; }

        public string PasswordMd5 { get; set; }
    }
}