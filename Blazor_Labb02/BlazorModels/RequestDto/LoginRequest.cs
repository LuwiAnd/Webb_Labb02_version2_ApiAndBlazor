using System.ComponentModel.DataAnnotations;

namespace Blazor_Labb02.BlazorModels.RequestDto
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "E-post är obligatoriskt")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lösenord är obligatoriskt")]
        public string Password { get; set; } = string.Empty;
    }
}
