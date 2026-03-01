using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoMaui_Listview;

public class ToDoClass : INotifyPropertyChanged
{
    private int _id;
    private string _title = "";
    private string _detail = "";
    private DateTime? _dateTime; // optional

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

    // Optional datetime (nullable)
    public DateTime? dateTime
    {
        get => _dateTime;
        set
        {
            _dateTime = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DisplayDateTime));
        }
    }

    // What we show in the ListView
    public string DisplayDateTime
    {
        get
        {
            if (dateTime == null) return "";
            // Day of week included
            return dateTime.Value.ToString("dddd, MMM dd yyyy - hh:mm tt");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}