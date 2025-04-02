namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class RegisterUserRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string HomeAddress { get; set; }
        public required string Password { get; set; }
    }
}


