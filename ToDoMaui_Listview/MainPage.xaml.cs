using System.Collections.ObjectModel;

namespace ToDoMaui_Listview;

public partial class MainPage : ContentPage
{
    private ObservableCollection<ToDoClass> _todos = new();
    private ToDoClass? _selectedToDo = null;
    private int _nextId = 1;

    public MainPage()
    {
        InitializeComponent();

        // Bind ListView to ObservableCollection
        todoLV.ItemsSource = _todos;

        // Optional sample items (you can remove these later)
        _todos.Add(new ToDoClass { id = _nextId++, title = "Example: Study", detail = "Read notes for quiz" });
        _todos.Add(new ToDoClass { id = _nextId++, title = "Example: Grocery", detail = "Milk, eggs, bread" });
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

        var todo = new ToDoClass
        {
            id = _nextId++,
            title = title,
            detail = detail
        };

        _todos.Insert(0, todo);
        ClearInputs();
    }

    private void TodoLV_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;

        _selectedToDo = (ToDoClass)e.SelectedItem;

        titleEntry.Text = _selectedToDo.title;
        detailsEditor.Text = _selectedToDo.detail;

        addBtn.IsVisible = false;
        editBtn.IsVisible = true;
        cancelBtn.IsVisible = true;
    }

    private void EditToDoItem(object sender, EventArgs e)
    {
        if (_selectedToDo == null)
        {
            DisplayAlert("No selection", "Tap an item first to edit.", "OK");
            return;
        }

        string title = titleEntry.Text?.Trim() ?? "";
        string detail = detailsEditor.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(title))
        {
            DisplayAlert("Missing Title", "Please enter a title.", "OK");
            return;
        }

        // Updates ListView automatically because ToDoClass supports INotifyPropertyChanged
        _selectedToDo.title = title;
        _selectedToDo.detail = detail;

        EndEditMode();
    }

    private void CancelEdit(object sender, EventArgs e)
    {
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

        if (_selectedToDo != null && _selectedToDo.id == item.id)
            EndEditMode();

        _todos.Remove(item);
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
        titleEntry.Focus();
    }
}