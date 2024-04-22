using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ExpensesTracker.Client;

public class AuthComponentBase : ComponentBase
{
    [Inject] private AuthenticationStateProvider? _persistentAuthenticationStateProvider { get; set; }

    protected async Task<string> TryGetAuthenticatedUser()
    {
        AuthenticationState state = await _persistentAuthenticationStateProvider!.GetAuthenticationStateAsync();

        if (!state.User.Identity.IsAuthenticated)
        {            
            return string.Empty;
        }

        return state.User.Claims.FirstOrDefault().Value;
    }
}
