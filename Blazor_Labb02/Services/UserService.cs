using Blazor_Labb02.BlazorModels.RequestDto;
using Blazor_Labb02.BlazorModels.ResponseDto;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace Blazor_Labb02.Services
{
    public class UserService
    {
        private readonly HttpClient _http;
        private readonly AuthState _authState;

        public UserService(HttpClient http, AuthState authState)
        {
            _http = http;
            _authState = authState;
        }

        public async Task<UserResponse?> GetByEmail(string email)
        {
            var url = $"users/search?email={Uri.EscapeDataString(email)}";

            var token = _authState.Token;
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserResponse>();
            }

            return null;
        }

        public async Task<UserResponse?> GetById(int id)
        {
            var url = $"users/{id}";

            var token = _authState.Token;
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserResponse>();
            }

            return null;
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            var url = $"users";

            var token = _authState.Token;
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<UserResponse>>() ?? new();
            }

            return new();
        }

        //public async Task UpdateUser(int userId, UserResponse user)
        //public async Task UpdateUser(int userId, UserRequest request)
        public async Task UpdateUser(UserRequest request)
        {
            //var url = $"users/{request.Id}";
            var url = $"users";
            var token = _authState.Token;


            var updateRequest = new UpdateUserRequest
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.Phone,
                HomeAddress = request.Address ?? "",
                Role = request.Role
            };


            //var request = new HttpRequestMessage(HttpMethod.Put, url)
            var httpRequest = new HttpRequestMessage(HttpMethod.Put, url)
            {
                //Content = JsonContent.Create(user)
                //Content = JsonContent.Create(request)
                Content = JsonContent.Create(updateRequest)
            };

            if (!string.IsNullOrWhiteSpace(token))
            {
                httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", 
                    token
                );
            }

            //var response = await _http.SendAsync(request);
            var response = await _http.SendAsync(httpRequest);

            Console.WriteLine($"Uppdaterar användare med id {request.Id} - status: {response.StatusCode}");
            response.EnsureSuccessStatusCode();
        }


        public async Task UpdateMyProfile(UserRequest updatedUser)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "users/me")
            {
                Content = JsonContent.Create(updatedUser)
            };

            var token = _authState.Token;
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.SendAsync(request);
            Console.WriteLine($"PUT /users/me svar: {response.StatusCode}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<UserRequest?> GetMyProfile()
        {
            var token = _authState.Token;
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var request = new HttpRequestMessage(HttpMethod.Get, "users/me");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                //var profile = await response.Content.ReadFromJsonAsync<UserRequest>();
                var profile = await response.Content.ReadFromJsonAsync<UserResponse>();

                //return profile;
                if(profile != null)
                {
                    return new UserRequest
                    {
                        Id = profile!.UserID,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        Email = profile.Email,
                        Phone = profile.PhoneNumber,
                        Address = profile.HomeAddress,
                        Role = profile.Role
                    };
                }
                
            }

            return null;
        }


        public async Task RegisterUser(RegisterUserRequest request)
        {
            var response = await _http.PostAsJsonAsync("register", request);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Registrering misslyckades: {content}");
                throw new Exception("Registreringen misslyckades.");
            }
        }


    }
}
