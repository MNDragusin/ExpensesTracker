using System;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Client.Components.Account.Pages.Manage;
using Client.DTOs;
using Client.Model;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Services;

public class AuthenticationService: IAuthenticationService
{
    private HttpClient _httpClient;
    private CustomAuthStateProvider _authStateProvider;
    
    public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = (CustomAuthStateProvider)authStateProvider;
    }

    public async Task<BaseResult> AuthenticateUser(LoginInputModel input)
    {   
        var jsonData = JsonSerializer.Serialize(new
        {
            email = input.Email,
            password = input.Password,
            twoFactorCode = string.Empty,
            twoFactorRecoveryCode = string.Empty
        });

        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var respons = await _httpClient.PostAsync("login", content);

        if (!respons.IsSuccessStatusCode)
        {
            return new BaseResult(){ Success = false, Errors = new[] { "error: Login failed" } };
        }

        var jsonRespons = await respons.Content.ReadAsStringAsync();
        var bodyResponse = JsonSerializer.Deserialize<LoginResponse>(jsonRespons);

        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Name, input.Email),
            new Claim(ClaimTypes.Email, input.Email),
            new Claim(nameof(bodyResponse.tokenType), bodyResponse.tokenType),
            new Claim(nameof(bodyResponse.accessToken), bodyResponse.accessToken),
            new Claim(nameof(bodyResponse.expiresIn), bodyResponse.expiresIn.ToString()),
            new Claim(nameof(bodyResponse.refreshToken), bodyResponse.refreshToken),
        ], "Custom Auth State");
        
        var user = new ClaimsPrincipal(identity);
        _authStateProvider.UpdateUserState(user);
        
        return new BaseResult(){ Success = true};
    }

    public async Task<BaseResult> RegisterAsync(RegisterInputModel input)
    {
        var jsonData = JsonSerializer.Serialize(new
        {
            email = input.Email,
            password = input.Password
        });

        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var respons = await _httpClient.PostAsync("register", content);


        if (respons.IsSuccessStatusCode)
        {
            _authStateProvider.UpdateUserState(new ClaimsPrincipal());
            int a = 0;
            a++;
        }

        return new BaseResult() { Success = respons.IsSuccessStatusCode };
    }

    private class LoginResponse
    {
        public string? tokenType;
        public required string accessToken;
        public required int expiresIn;
        public required string refreshToken;
    }
}