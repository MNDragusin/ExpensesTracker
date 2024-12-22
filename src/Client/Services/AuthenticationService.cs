using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Client.DTOs;
using Client.Model;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Services;

public class AuthenticationService: IAuthenticationService
{
    private HttpClient _httpClient;
    private CustomAuthStateProvider _authStateProvider;
    private ILocalStorageService _localStorage;
    
    public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authStateProvider = (CustomAuthStateProvider)authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<BaseResult> AuthenticateUser(LoginInputModel input)
    {   
        var loginData = new
        {
            email = input.Email,
            password = input.Password,
            twoFactorCode = string.Empty,
            twoFactorRecoveryCode = string.Empty
        };

        var respons = await _httpClient.PostAsJsonAsync("login", loginData);
        
        if (!respons.IsSuccessStatusCode)
        {
            return new BaseResult(){ Success = false, Errors = ["error: Login failed"] };
        }

        var bodyResponse = await respons.Content.ReadFromJsonAsync<LoginResponse>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (input.RememberMe)
        {
            await _localStorage.SetItemAsync("rememberMe", input.Email);
            await _localStorage.SetItemAsync("tokenData", bodyResponse);
        }
        
        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Name, input.Email),
            new Claim(ClaimTypes.Email, input.Email),
            new Claim(nameof(bodyResponse.TokenType), bodyResponse.TokenType),
            new Claim(nameof(bodyResponse.AccessToken), bodyResponse.AccessToken),
            new Claim(nameof(bodyResponse.ExpiresIn), bodyResponse.ExpiresIn.ToString()),
            new Claim(nameof(bodyResponse.RefreshToken), bodyResponse.RefreshToken),
        ], "Custom Auth State");
        
        var user = new ClaimsPrincipal(identity);
        _authStateProvider.UpdateUserState(user);
        
        return new BaseResult { Success = true};
    }

    public async Task<BaseResult> RegisterAsync(RegisterInputModel input)
    {
        var content = new
        {
            email = input.Email,
            password = input.Password
        };
        
        var respons = await _httpClient.PostAsJsonAsync("register", content);
        
        if (respons.IsSuccessStatusCode)
        {
            _authStateProvider.UpdateUserState(new ClaimsPrincipal());
            int a = 0;
            a++;
        }

        return new BaseResult() { Success = respons.IsSuccessStatusCode };
    }

    public class LoginResponse
    {
        public string? TokenType { get; set; }
        public required string AccessToken { get; set; }
        public required int ExpiresIn { get; set; }
        public required string RefreshToken { get; set; }
    }
}