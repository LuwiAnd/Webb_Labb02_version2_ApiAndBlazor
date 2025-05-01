using System.ComponentModel.DataAnnotations;

namespace Blazor_Labb02.BlazorModels.RequestDto
{
    public class RegisterUserRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        //[MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
        public string? HomeAddress { get; set; }
    }
}
