using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoMaui_Listview;

public class ToDoClass : INotifyPropertyChanged
{
    private int _id;
    private string _title;
    private string _detail;

    public int id
    {
        get => _id;
        set { _id = value; OnPropertyChanged(); }
    }

    public string title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    public string detail
    {
        get => _detail;
        set { _detail = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}