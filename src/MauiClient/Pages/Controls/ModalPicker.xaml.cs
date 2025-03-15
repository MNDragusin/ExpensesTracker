using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using Shared.Entities;

namespace MauiClient.Pages.Controls;

public partial class ModalPicker : Popup
{
    //this entire base definition should be even more abstract
    //Option(string Id, string Name);
    public ObservableCollection<BaseDefinition> Items { get; set; }
    
    private Action<BaseDefinition>? _onSelectedCallback;
    public ModalPicker(IEnumerable<BaseDefinition> options, Action<BaseDefinition>? onSelectedCallback)
    {
        InitializeComponent();
        
        _onSelectedCallback = onSelectedCallback;
        Items = new ObservableCollection<BaseDefinition>(options);
        
        BindingContext = this;
    }

    private void OnClose(object? sender, EventArgs e) => Close();

    private void OnOptionSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0)
        {
            return;
        }
        
        var selectedItem = e.CurrentSelection[0] as BaseDefinition;
        _onSelectedCallback?.Invoke(selectedItem);
        Close();
    }
}