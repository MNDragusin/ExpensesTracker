using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private AuthenticationState _state;

    public CustomAuthStateProvider()
    {
        _state = new AuthenticationState(new ClaimsPrincipal());
    }

    public void UpdateUserState(ClaimsPrincipal newUser)
    {
        _state = new AuthenticationState(newUser);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(_state);
    }
}