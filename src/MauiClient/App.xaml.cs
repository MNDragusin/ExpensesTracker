using AppDataContext;

namespace MauiClient
{
    public partial class App : Application
    {
        private DataContext _dataContext;
        public App(DataContext dataContext)
        {
            _dataContext = dataContext;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_dataContext));
        }
    }
}