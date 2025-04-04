using FastEndpoints;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class GetUserByEmailRequest
    {
        [QueryParam]
        public string Email { get; set; } = string.Empty;
    }
}
