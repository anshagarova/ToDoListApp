using System.ComponentModel;

public class TaskItem : INotifyPropertyChanged
{
    private string description;
    private bool isImportant;
    public string Description 
    { 
        get => description; 
        set { description = value; OnPropertyChanged(nameof(Description)); } 
    }
    
    public bool IsImportant 
    { 
        get => isImportant; 
        set { isImportant = value; OnPropertyChanged(nameof(IsImportant)); OnPropertyChanged(nameof(DisplayColor)); } 
    }

    public string DisplayColor => IsImportant ? "Red" : "Black";

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public TaskItem(string description, bool isImportant)
    {
        Description = description;
        IsImportant = isImportant;
    }
}
