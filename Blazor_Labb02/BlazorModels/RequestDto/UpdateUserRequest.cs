using System.ComponentModel.DataAnnotations;

namespace Blazor_Labb02.BlazorModels.RequestDto
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? HomeAddress { get; set; }

        public string Role { get; set; }
        public string? Password { get; set; }
    }
}
