namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string HomeAddress { get; set; }
        public string? Role { get; set; }
    }
}
