using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazor_Labb02.BlazorModels.ResponseDto;
using Blazor_Labb02.BlazorModels.RequestDto;

namespace Blazor_Labb02.Services
{
    public class CartService
    {
        private readonly HttpClient _http;
        private readonly AuthState _authState;

        public CartService(HttpClient http, AuthState authState)
        {
            _http = http;
            _authState = authState;
        }

        private void AddAuthHeader(HttpRequestMessage request)
        {
            var token = _authState.Token;
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task AddToCart(int productId, int quantity = 1)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "cart/add")
            {
                Content = JsonContent.Create(new { ProductID = productId, Quantity = quantity })
            };
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<CartItemResponse>> GetCart()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "cart");
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CartItemResponse>>() ?? new();
            }
            return new();
        }

        public async Task UpdateQuantity(int productId, int quantity)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "cart/update")
            {
                Content = JsonContent.Create(new { ProductID = productId, Quantity = quantity })
            };
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveFromCart(int productId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"cart/item/{productId}");
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearCart()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "cart/clear");
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }



        public async Task Checkout()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "cart/checkout");
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

    }
}
