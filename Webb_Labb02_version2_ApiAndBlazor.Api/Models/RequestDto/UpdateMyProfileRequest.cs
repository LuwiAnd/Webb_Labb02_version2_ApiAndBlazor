namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class UpdateMyProfileRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }   // Uppdateringsbar
        public string? PhoneNumber { get; set; }
        public string? HomeAddress { get; set; }
        public string? Password { get; set; } // Valfritt om man vill byta lösenord
    }
}
