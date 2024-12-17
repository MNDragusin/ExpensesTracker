using Microsoft.AspNetCore.Components;

namespace ExpensesTracker.Blazor.Client.Components;

public class PersistentStateComponentBase<T>: ComponentBase
{
    [Inject]
    public PersistentComponentState ApplicationState { get; set; }
    private PersistingComponentStateSubscription _subscription { get; set; }
    
    private T? Data { get; set; }
    protected virtual string _key { get; set; }

    protected void Init()
    {
        _subscription = ApplicationState.RegisterOnPersisting(PersistState);
        if (ApplicationState.TryTakeFromJson<T>(_key, out var persistingData))
        {
            Data = persistingData;
        }
    }
    
    private Task PersistState()
    {
        ApplicationState.PersistAsJson(_key, Data);
        return Task.CompletedTask;
    } 
}