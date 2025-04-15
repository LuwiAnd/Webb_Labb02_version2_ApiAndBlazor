using Microsoft.AspNetCore.Mvc;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class GetProductByProductNumberRequest
    {
        [FromRoute]
        public int Number { get; set; }
    }
}
