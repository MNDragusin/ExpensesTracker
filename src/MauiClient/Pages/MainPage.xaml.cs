using MauiClient.Models;
using MauiClient.PageModels;

namespace MauiClient.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}