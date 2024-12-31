using MauiClient.PageModels;

namespace MauiClient
{
    public partial class App : Application
    {
        private ShellViewModel _shellViewModel;
        private ModalErrorHandler _errorHandler;
        
        public App(ShellViewModel shellViewModel, ModalErrorHandler modalErrorHandler)
        {
            _errorHandler = modalErrorHandler;
            _shellViewModel = shellViewModel;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_shellViewModel, _errorHandler));
        }
    }
}