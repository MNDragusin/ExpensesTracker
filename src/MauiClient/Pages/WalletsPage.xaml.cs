namespace MauiClient.Pages;

public partial class WalletsPage : ContentPage
{
    public WalletsPage(WalletViewModel walletViewModel)
    {
        InitializeComponent();
        BindingContext = walletViewModel;
    }
}