using Microsoft.AspNetCore.Components;

namespace ExpensesTracker.Blazor.Client.Components.DialogBox;

public partial class DialogComponent : ComponentBase
{
    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? BodyContent { get; set; }
    [Parameter] public RenderFragment? FooterContent { get; set; }
    private bool _isOpen;
    [Parameter] public bool IsOpen
    {
        get => _isOpen; set { _isOpen = value; StateHasChanged(); }}
    [Parameter] public EventCallback OnCloseBtnCallback { get; set; }

    private void CloseDialog()
    {
        IsOpen = false;
        OnCloseBtnCallback.InvokeAsync();
    }
}   
