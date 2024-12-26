namespace MauiClient.Pages;

public partial class WalletsPage : ContentPage
{
    public WalletsPage(WalletViewModel walletViewModel)
    {
        InitializeComponent();
        _viewModel = walletViewModel;
        BindingContext = _viewModel;
    }

    private readonly WalletViewModel _viewModel;
}