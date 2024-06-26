using System.ComponentModel;

public class TaskItem : INotifyPropertyChanged
{
    private string _description;
    private bool _isImportant;
    private bool _isEditing;

    public event PropertyChangedEventHandler PropertyChanged;

    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }

    public bool IsImportant
    {
        get => _isImportant;
        set
        {
            if (_isImportant != value)
            {
                _isImportant = value;
                OnPropertyChanged(nameof(IsImportant));
                OnPropertyChanged(nameof(DisplayColor));
            }
        }
    }

public bool IsEditing
    {
        get => _isEditing;
        set
        {
            if (_isEditing != value)
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string DisplayColor => IsImportant ? "#fb8500" : "Black";

    public TaskItem(string description, bool isImportant)
    {
        _description = description;
        _isImportant = isImportant;
    }
}
