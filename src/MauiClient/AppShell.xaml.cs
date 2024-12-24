using AppDataContext;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace MauiClient
{
    public partial class AppShell : Shell
    {
        private DataContext _dataContext;
        public AppShell(DataContext dataContext)// context data should not be here directly
        {
            _dataContext = dataContext;
            InitializeComponent();
            var currentTheme = Application.Current!.UserAppTheme;
            ThemeSegmentedControl.SelectedIndex = currentTheme == AppTheme.Light ? 0 : 1;

            InitWallets();
        }

        private void InitWallets()
        {
            foreach (var wallet in _dataContext.Wallets)
            {
                var tab = new Tab
                {
                    Title = wallet.Name,
                };

                var shellContent = new ShellContent
                {
                    Content = new DataTemplate(typeof(MainPage))
                    //Content = new WalletPage(wallet)
                };

                var flyoutItem = new FlyoutItem
                {
                    Title = wallet.Name,
                };

                tab.Items.Add(shellContent);
                flyoutItem.Items.Add(tab);

                Items.Add(flyoutItem);
            }
        }

        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#FF3300"),
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(0),
                Font = Font.SystemFontOfSize(18),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }

        public static async Task DisplayToastAsync(string message)
        {
            // Toast is currently not working in MCT on Windows
            if (OperatingSystem.IsWindows())
                return;

            var toast = Toast.Make(message, textSize: 18);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await toast.Show(cts.Token);
        }

        private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
        }
    }
}
