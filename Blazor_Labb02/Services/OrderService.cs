using Blazor_Labb02.BlazorModels.ResponseDto;
using System.Net.Http.Json;

namespace Blazor_Labb02.Services;

public class OrderService
{
    private readonly HttpClient _http;
    private readonly AuthState _authState;

    public OrderService(HttpClient http, AuthState authState)
    {
        _http = http;
        _authState = authState;
    }

    public async Task<List<OrderResponse>> GetAllOrders()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "orders");

        if (!string.IsNullOrWhiteSpace(_authState.Token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authState.Token);
        }

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<OrderResponse>>() ?? new();
    }
}
