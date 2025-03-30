namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
