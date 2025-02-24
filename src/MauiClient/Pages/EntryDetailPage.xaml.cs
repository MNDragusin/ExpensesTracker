namespace MauiClient.Pages
{
    public partial class TaskDetailPage : ContentPage
    {
        public TaskDetailPage(EntryDetailPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}