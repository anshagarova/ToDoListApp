using System.ComponentModel;

public class TaskItem : INotifyPropertyChanged
{
    private string description;
    private bool isImportant;

    public string Description
    {
        get => description;
        set
        {
            description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public bool IsImportant
    {
        get => isImportant;
        set
        {
            isImportant = value;
            OnPropertyChanged(nameof(IsImportant));
            OnPropertyChanged(nameof(DisplayColor));
        }
    }

    public TaskItem(string description, bool isImportant)
    {
        Description = description;
        IsImportant = isImportant;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public string DisplayColor => IsImportant ? "Red" : "Black";
}
