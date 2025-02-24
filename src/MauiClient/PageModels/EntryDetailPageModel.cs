using AppDataContext;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiClient.Pages.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Entities;
using Category = Shared.Entities.Category;
using Label = Shared.Entities.Label;

namespace MauiClient.PageModels
{
    public partial class EntryDetailPageModel : ObservableObject, IQueryAttributable
    {
        private bool _canDelete;
        private readonly DataContext _walletContext;
        private readonly ModalErrorHandler _errorHandler;

        [ObservableProperty]
        private string _title = string.Empty;
        
        [ObservableProperty] private WalletEntry? _entry;
        [ObservableProperty] private List<Wallet>? _wallets;
        [ObservableProperty] private List<Category>? _categories;
        [ObservableProperty] private List<Label>? _labels;
        
        public EntryDetailPageModel(DataContext walletContext, ModalErrorHandler errorHandler)
        {
            _walletContext = walletContext;
            _errorHandler = errorHandler;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            LoadEntryAsync(query).FireAndForgetSafeAsync(_errorHandler);
        }

        private async Task LoadEntryAsync(IDictionary<string, object> query)
        {
            _canDelete = false;
            Wallets = await _walletContext.Wallets.ToListAsync(); 
            Categories = await _walletContext.Categories.ToListAsync(); 
            Labels = await _walletContext.Labels.ToListAsync();
            
            Entry = new WalletEntry
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                EntryId = null,
                WalletId = null,
                LabelId = null,
                CategoryId = null
            };
            
            if (query.TryGetValue(ProjectQueryKey, out var entry))
            {
                Entry = (WalletEntry)entry;
                _canDelete = true;
            }
        }

        public bool CanDelete
        {
            get => _canDelete;
            set
            {
                _canDelete = value;
                DeleteCommand.NotifyCanExecuteChanged();
            }
        }

        public static string ProjectQueryKey => "DetailsDataKey";

        [RelayCommand]
        private async Task Save()
        {
            if (Entry is null)
            {
                _errorHandler.HandleError(
                    new Exception("Entry is not valid"));

                return;
            }

            if (Entry.Wallet == null)
            {
                _errorHandler.HandleError(
                    new Exception("Entry has no wallet selected"));

                return;
            }
            
            if (Entry.Category == null)
            {
                _errorHandler.HandleError(
                    new Exception("Entry has no category selected"));

                return;
            }
            
            if (Entry.Label == null)
            {
                _errorHandler.HandleError(
                    new Exception("Entry has no label selected"));

                return;
            }

            Entry.WalletId = Entry.Wallet.Id;
            Entry.CategoryId = Entry.Category.Id;
            Entry.LabelId = Entry.Label.Id;
            
            if (_canDelete)
            {
                _walletContext.WalletEntries.Update(Entry);
            }
            else
            {
                _walletContext.WalletEntries.Add(Entry);    
            }
            
            var changed = await _walletContext.SaveChangesAsync();
            await Shell.Current.GoToAsync("..?refresh=true");
            string msg = changed > 0 ? "Entry Saved" : "Entry cannot be saved";
            await AppShell.DisplayToastAsync(msg);

        }

        [RelayCommand(CanExecute = nameof(CanDelete))]
        private async Task Delete()
        {
            if (!_canDelete)
            {
                return;
            }

            _walletContext.WalletEntries.Remove(_entry);
            var changed = await _walletContext.SaveChangesAsync();
            if (changed == 0)
            {
                await AppShell.DisplayToastAsync("Entry could not be deleted");   
                return;
            }
            
            await Shell.Current.GoToAsync("..?refresh=true");
            await AppShell.DisplayToastAsync("Entry deleted");
        }

        [RelayCommand]
        private async void OpenModal()
        {
            var popup = new ModalPicker(_labels, null);
            Shell.Current.ShowPopup(popup);
        }
    }
}