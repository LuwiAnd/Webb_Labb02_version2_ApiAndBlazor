using Blazor_Labb02.BlazorModels.ResponseDto;
using Blazor_Labb02.BlazorModels.RequestDto;
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






    //public async Task<ProductResponse?> GetByProductNumber(int productNumber)
    //{
    //    var response = await _http.GetAsync($"products/{productNumber}");
    //    if (response.IsSuccessStatusCode)
    //    {
    //        return await response.Content.ReadFromJsonAsync<ProductResponse>();
    //    }

    //    return null;
    //}

    public async Task<ProductResponse?> GetByProductNumber(int productNumber)
    {
        var url = $"products/by-number/{productNumber}";
        Console.WriteLine($"📡 Anropar API: {url}");

        var response = await _http.GetAsync(url);

        Console.WriteLine($"📥 HTTP-status: {response.StatusCode}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ProductResponse>();
            return result;
        }

        return null;
    }


    public async Task UpdateProduct(int productId, UpdateProductRequest request)
    {
        var response = await _http.PutAsJsonAsync($"products/{productId}", request);
        response.EnsureSuccessStatusCode();
    }



}
