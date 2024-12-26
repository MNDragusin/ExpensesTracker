using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppDataContext;

namespace MauiClient.PageModels;

public class WalletViewModel : INotifyPropertyChanged, IQueryAttributable
{
    public WalletViewModel(DataContext dataContext)
    {
        _dbContext = dataContext;
    }
    
    private readonly DataContext _dbContext;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        int a = 0;
        a++;
    }
}