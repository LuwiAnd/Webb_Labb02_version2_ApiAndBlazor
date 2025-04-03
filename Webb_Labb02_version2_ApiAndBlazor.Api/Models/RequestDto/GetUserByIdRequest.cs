using Microsoft.AspNetCore.Mvc;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class GetUserByIdRequest
    {
        //[FromRoute]
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}
