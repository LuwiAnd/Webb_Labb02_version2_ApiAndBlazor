using Blazor_Labb02.BlazorModels.ResponseDto;
using Blazor_Labb02.BlazorModels.RequestDto;
using System.Net.Http.Json;

namespace Blazor_Labb02.Services;

public class ProductService
{
    private readonly HttpClient _http;
    private readonly AuthState _authState;

    public ProductService(HttpClient http, AuthState authState)
    {
        _http = http;
        _authState = authState;
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
        var url = $"products/{productId}";
        var token = _authState.Token;

        var requestMessage = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = JsonContent.Create(request)
        };

        if (!string.IsNullOrWhiteSpace(token))
        {
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.SendAsync(requestMessage);

        Console.WriteLine($"Put-status: {response.StatusCode}");

        response.EnsureSuccessStatusCode();

        //Gammal kod:
        //var response = await _http.PutAsJsonAsync($"products/{productId}", request);
        //response.EnsureSuccessStatusCode();
    }

    //public async Task DeleteProduct(int productId)
    public async Task DeleteProduct(int productNumber)
    {
        var token = _authState.Token;

        //Console.WriteLine($"Försöker ta bort produkt med ID: {productId}");
        Console.WriteLine($"Försöker ta bort produkt med nummer: {productNumber}");

        //var request = new HttpRequestMessage(HttpMethod.Delete, $"products/{productId}");
        var request = new HttpRequestMessage(HttpMethod.Delete, $"products/by-number/{productNumber}");

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.SendAsync(request);

        Console.WriteLine($"DELETE-status: {response.StatusCode}");

        response.EnsureSuccessStatusCode();
    }


    public async Task CreateProduct(ProductRequest request)
    {
        var token = _authState.Token;

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "products")
        {
            Content = JsonContent.Create(request)
        };

        if (!string.IsNullOrWhiteSpace(token))
        {
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.SendAsync(requestMessage);

        Console.WriteLine($"POST-status: {response.StatusCode}");

        response.EnsureSuccessStatusCode();
    }


}
