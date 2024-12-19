using System.Net;
using System.Security.Claims;
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
    
    public Task<BaseResult> RegisterAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResult> AuthenticateUser(LoginInputModel input)
    {
        // var jsonData = JsonSerializer.Serialize(Input);
        //
        // var respons = await _httpClient.PostAsync("register",new StringContent(jsonData, Encoding.UTF8, "application/json"));
        //
        // if (respons.IsSuccessStatusCode)
        // {
        //     
        // }
        
        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Name, "TestUser")
        ], "Custom Auth State");
        
        var user = new ClaimsPrincipal(identity);
        _authStateProvider.UpdateUserState(user);
        
        return Task.FromResult(new BaseResult(){ Success = true});
    }
}