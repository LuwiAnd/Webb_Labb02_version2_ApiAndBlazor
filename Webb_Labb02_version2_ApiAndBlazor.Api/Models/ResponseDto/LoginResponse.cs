namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
