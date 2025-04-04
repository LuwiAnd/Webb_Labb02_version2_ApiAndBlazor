using Microsoft.AspNetCore.Mvc;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class GetUserByIdRequest
    {
        //[FromRoute]
        //public int Id { get; set; }
        public int id { get; set; }
    }
}
