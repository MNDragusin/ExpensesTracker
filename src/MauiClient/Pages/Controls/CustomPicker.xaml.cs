using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using Shared.Entities;

namespace MauiClient.Pages.Controls;

public partial class CustomPicker : ContentView
{
    public string? Value { get; set; }
    private ModalPicker CurrentModal { get; set; }
    public IEnumerable<BaseDefinition> Options { get; set; }
    
    public CustomPicker()
    {
        InitializeComponent();
        Task.Run(Init);
    }

    private Task Init()
    {
        CurrentModal = new ModalPicker(Options, OnOptionSelected);
        return Task.CompletedTask;
    }
    
    [RelayCommand]
    private async Task ShowPickerPopup()
    {
        while (CurrentModal == null)
        {
            
        }
        
        Shell.Current.ShowPopupAsync(CurrentModal);
    }

    private void OnOptionSelected(BaseDefinition currentSlectction)
    {
        CurrentSelectedLabel.Text = currentSlectction.Name;
        Value = currentSlectction.Id;
    }
}