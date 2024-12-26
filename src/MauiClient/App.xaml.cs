using MauiClient.PageModels;

namespace MauiClient
{
    public partial class App : Application
    {
        private ShellViewModel _shellViewModel;
        public App(ShellViewModel shellViewModel)
        {
            _shellViewModel = shellViewModel;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_shellViewModel));
        }
    }
}