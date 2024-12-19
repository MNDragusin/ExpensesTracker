using System.Security.Claims;
using Client.Components.Account.Pages;
using Client.DTOs;
using Client.Model;

namespace Client.Services;

public interface IAuthenticationService
{
    public Task<BaseResult> RegisterAsync(RegisterInputModel input);
    public Task<BaseResult> AuthenticateUser(LoginInputModel input);
}