namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto
{
    public class UserResponse
    {
        public int UserID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string HomeAddress { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
