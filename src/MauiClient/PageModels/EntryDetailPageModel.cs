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
        [ObservableProperty]
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
            CanDelete = false;
            Wallets = await _walletContext.Wallets.ToListAsync();
            Categories = await _walletContext.Categories.ToListAsync();
            Labels = await _walletContext.Labels.ToListAsync();

            Entry = WalletEntry.NewEmptyWalletEntry();

            if (query.TryGetValue(EntryQueryKey, out var entry))
            {
                Entry = (WalletEntry)entry;
                CanDelete = true;
            }
        }

        public static string EntryQueryKey => "DetailsDataKey";

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

            if (CanDelete)
            {
                _walletContext.WalletEntries.Update(Entry);
            }
            else
            {
                _walletContext.WalletEntries.Add(Entry);
            }

            var changed = await _walletContext.SaveChangesAsync();

            await Shell.Current.GoToAsync("..", new ShellNavigationQueryParameters(){
                    {"refresh", true}
                });

            string msg = changed > 0 ? "Entry Saved" : "Entry cannot be saved";
            await AppShell.DisplayToastAsync(msg);

        }

        [RelayCommand]
        private async Task Delete()
        {
            if (!CanDelete)
            {
                return;
            }

            _walletContext.WalletEntries.Remove(Entry!);
            var changed = await _walletContext.SaveChangesAsync();
            if (changed == 0)
            {
                await AppShell.DisplayToastAsync("Entry could not be deleted");
                return;
            }

            await AppShell.DisplayToastAsync("Entry deleted");
            await Shell.Current.GoToAsync("..", new ShellNavigationQueryParameters(){
                    {"refresh", true}
                });

        }

        [RelayCommand]
        private void OpenModal()
        {
            var popup = new ModalPicker(Labels!, null);
            Shell.Current.ShowPopup(popup);
        }
    }
}