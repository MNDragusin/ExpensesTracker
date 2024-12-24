using CommunityToolkit.Mvvm.Input;
using MauiClient.Models;

namespace MauiClient.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}