using FastEndpoints;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class SearchProductsRequest
    {
        [QueryParam]
        public string? Name { get; set; }

        [QueryParam]
        public string? ProductNumber { get; set; }
    }
}
