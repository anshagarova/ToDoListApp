using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace ToDoListApp
{
    public partial class MainWindow : Window
    {
        private TextBox _taskEntry;
        private ListBox _tasksListBox;
        private TaskItem _editingTask;
        
        public ObservableCollection<TaskItem> Tasks { get; set; }
        public string ErrorMessage { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<TaskItem>();
            DataContext = this;
            ErrorMessage = string.Empty;

            LoadTasks();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _taskEntry = this.FindControl<TextBox>("TaskEntry");
            _tasksListBox = this.FindControl<ListBox>("TasksListBox");
        }

        private void LoadTasks()
        {
            if (File.Exists("tasks.json"))
            {
                try
                {
                    string tasksJson = File.ReadAllText("tasks.json");
                    var loadedTasks = JsonConvert.DeserializeObject<ObservableCollection<TaskItem>>(tasksJson) ?? new ObservableCollection<TaskItem>();
                    foreach (var task in loadedTasks)
                    {
                        task.PropertyChanged += Task_PropertyChanged;
                        Tasks.Add(task);
                    }
                    SortTasks();
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error loading tasks: " + ex.Message;
                }
            }
        }

        private void SaveTasks()
        {
            try
            {
                string tasksJson = JsonConvert.SerializeObject(Tasks);
                File.WriteAllText("tasks.json", tasksJson);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error saving tasks: " + ex.Message;
            }
        }

        private void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskItem.IsImportant))
            {
                SortTasks();
                SaveTasks();
            }
        }

        private void SortTasks()
        {
            var sorted = Tasks.OrderByDescending(t => t.IsImportant).ToList();
            for (int i = 0; i < sorted.Count; i++)
            {
                if (i >= Tasks.Count || !Tasks[i].Equals(sorted[i]))
                {
                    int currentIndex = Tasks.IndexOf(sorted[i]);
                    if (currentIndex != i)
                    {
                        Tasks.Move(currentIndex, i);
                    }
                }
            }
        }

        private TaskItem GetSelectedTask()
        {
            if (_tasksListBox?.SelectedItem is TaskItem task)
            {
                return task;
            }
            
            ErrorMessage = "Please select a task";
            return null;
        }

        private void OnAddTaskClicked(object sender, RoutedEventArgs e)
        {
            if (_taskEntry != null && !string.IsNullOrWhiteSpace(_taskEntry.Text))
            {
                if (_editingTask != null)
                {
                    _editingTask.Description = _taskEntry.Text;
                    _editingTask = null;
                }
                else
                {
                    var newTask = new TaskItem(_taskEntry.Text, false);
                    newTask.PropertyChanged += Task_PropertyChanged;
                    Tasks.Add(newTask);
                }
                
                _taskEntry.Text = string.Empty;
                SortTasks();
                SaveTasks();
            }
            else
            {
                ErrorMessage = "Please enter a task";
            }
        }

        private void OnRemoveTaskClicked(object sender, RoutedEventArgs e)
        {
            var task = GetSelectedTask();
            if (task != null)
            {
                task.PropertyChanged -= Task_PropertyChanged;
                Tasks.Remove(task);
                SaveTasks();
                
                if (_editingTask == task)
                {
                    _editingTask = null;
                    _taskEntry.Text = string.Empty;
                }
            }
        }
        
        private void OnEditTaskClicked(object sender, RoutedEventArgs e)
        {
            var task = GetSelectedTask();
            if (task != null && _taskEntry != null)
            {
                _editingTask = task;
                _taskEntry.Text = task.Description;
                _taskEntry.Focus();
            }
        }
        
        private void TaskEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnAddTaskClicked(sender, null);
            }
            else if (e.Key == Key.Escape && _editingTask != null)
            {
                _editingTask = null;
                _taskEntry.Text = string.Empty;
            }
        }

        private void OnMarkAsImportantClicked(object sender, RoutedEventArgs e)
        {
            var task = GetSelectedTask();
            if (task != null)
            {
                task.IsImportant = !task.IsImportant;
            }
        }
    }
}
