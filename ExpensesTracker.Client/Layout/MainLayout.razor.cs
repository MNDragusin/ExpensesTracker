using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ExpensesTracker.Client;

public class MainLayoutBase : LayoutComponentBase
{
    protected bool DrawerOpen = true;
    protected bool IsDarkMode = false;
    protected string ModeIcon = Icons.Material.Filled.DarkMode;

    protected void ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        ModeIcon = IsDarkMode ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode;
    }

    protected void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }
}
