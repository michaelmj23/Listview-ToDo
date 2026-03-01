using System.Collections.ObjectModel;
using System.Linq;

namespace ToDoMaui_Listview;

public partial class MainPage : ContentPage
{
    private readonly ObservableCollection<ToDoClass> _todos = new();
    private ToDoClass? _selectedToDo = null;
    private int _nextId = 1;

    public MainPage()
    {
        InitializeComponent();

        // Bind ListView to ObservableCollection
        todoLV.ItemsSource = _todos;

        // Checkbox toggles visibility of Date/Time pickers
        dateCheckBox.CheckedChanged += DateCheckBox_CheckedChanged;

        // Optional sample item
        _todos.Add(new ToDoClass
        {
            id = _nextId++,
            title = "Example",
            detail = "Tap me to edit. Delete removes me.",
            dateTime = null
        });
    }

    private void DateCheckBox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        datePicker.IsVisible = e.Value;
        timePicker.IsVisible = e.Value;
    }

    private void AddToDoItem(object sender, EventArgs e)
    {
        string title = titleEntry.Text?.Trim() ?? "";
        string detail = detailsEditor.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(title))
        {
            DisplayAlert("Missing Title", "Please enter a title.", "OK");
            return;
        }

        DateTime? selectedDateTime = null;
        if (dateCheckBox.IsChecked)
        {
            selectedDateTime = datePicker.Date + timePicker.Time;
        }

        var todo = new ToDoClass
        {
            id = _nextId++,
            title = title,
            detail = detail,
            dateTime = selectedDateTime
        };

        _todos.Insert(0, todo);

        ClearInputs();
    }

    private void TodoLV_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;

        _selectedToDo = (ToDoClass)e.SelectedItem;

        // Fill inputs
        titleEntry.Text = _selectedToDo.title;
        detailsEditor.Text = _selectedToDo.detail;

        // Load optional datetime
        if (_selectedToDo.dateTime != null)
        {
            dateCheckBox.IsChecked = true;
            datePicker.Date = _selectedToDo.dateTime.Value.Date;
            timePicker.Time = _selectedToDo.dateTime.Value.TimeOfDay;
        }
        else
        {
            dateCheckBox.IsChecked = false;
        }

        // Switch buttons to Edit mode
        addBtn.IsVisible = false;
        editBtn.IsVisible = true;
        cancelBtn.IsVisible = true;
    }

    private void EditToDoItem(object sender, EventArgs e)
    {
        if (_selectedToDo == null)
        {
            DisplayAlert("No item selected", "Tap an item to edit first.", "OK");
            return;
        }

        string title = titleEntry.Text?.Trim() ?? "";
        string detail = detailsEditor.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(title))
        {
            DisplayAlert("Missing Title", "Please enter a title.", "OK");
            return;
        }

        _selectedToDo.title = title;
        _selectedToDo.detail = detail;

        if (dateCheckBox.IsChecked)
            _selectedToDo.dateTime = datePicker.Date + timePicker.Time;
        else
            _selectedToDo.dateTime = null;

        EndEditMode();
    }

    private async void DeleteToDoItem(object sender, EventArgs e)
    {
        if (sender is not Button btn) return;

        if (!int.TryParse(btn.ClassId, out int idToDelete)) return;

        var item = _todos.FirstOrDefault(x => x.id == idToDelete);
        if (item == null) return;

        bool confirm = await DisplayAlert("Delete", $"Delete \"{item.title}\"?", "Yes", "No");
        if (!confirm) return;

        // If deleting the one being edited, exit edit mode
        if (_selectedToDo != null && _selectedToDo.id == item.id)
        {
            EndEditMode();
        }

        _todos.Remove(item);
    }

    private void CancelEdit(object sender, EventArgs e)
    {
        EndEditMode();
    }

    private void EndEditMode()
    {
        _selectedToDo = null;
        todoLV.SelectedItem = null;

        addBtn.IsVisible = true;
        editBtn.IsVisible = false;
        cancelBtn.IsVisible = false;

        ClearInputs();
    }

    private void ClearInputs()
    {
        titleEntry.Text = "";
        detailsEditor.Text = "";
        dateCheckBox.IsChecked = false;
        titleEntry.Focus();
    }
}