using Blazor_Labb02.BlazorModels.ResponseDto;
using System.Net.Http.Json;

namespace Blazor_Labb02.Services;

public class ProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ProductResponse>> GetAllProducts()
    {
        var response = await _http.GetAsync("products");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
            return data ?? new();
        }

        return new(); // eller throw om du vill visa fel
    }


    public async Task<List<ProductResponse>> SearchProducts(string? name, int? productNumber)
    {
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(name))
            query.Add($"name={Uri.EscapeDataString(name)}");
        if (productNumber is not null)
            query.Add($"productNumber={productNumber}");

        var url = "products/search";
        if (query.Any())
            url += "?" + string.Join("&", query);

        var response = await _http.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
            return data ?? new();
        }

        return new();
    }

}
