using Avalonia.Controls;
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
            var sortedTasks = new ObservableCollection<TaskItem>(Tasks.OrderByDescending(t => t.IsImportant));
            Tasks.Clear();
            foreach (var task in sortedTasks)
            {
                Tasks.Add(task);
            }
        }

        private void OnAddTaskClicked(object sender, RoutedEventArgs e)
        {
            var taskEntry = this.FindControl<TextBox>("TaskEntry");
            if (taskEntry != null && !string.IsNullOrWhiteSpace(taskEntry.Text))
            {
                var newTask = new TaskItem(taskEntry.Text, false);
                newTask.PropertyChanged += Task_PropertyChanged;
                Tasks.Add(newTask);
                taskEntry.Text = string.Empty;
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
            var tasksListBox = this.FindControl<ListBox>("TasksListBox");
            if (tasksListBox != null && tasksListBox.SelectedItem != null)
            {
                var task = (TaskItem)tasksListBox.SelectedItem;
                task.PropertyChanged -= Task_PropertyChanged;
                Tasks.Remove(task);
                SaveTasks();
            }
            else
            {
                ErrorMessage = "Please select a task to remove";
            }
        }
        private void OnEditTaskClicked(object sender, RoutedEventArgs e)
{
    var tasksListBox = this.FindControl<ListBox>("TasksListBox");
    var taskEntry = this.FindControl<TextBox>("TaskEntry");

    if (tasksListBox != null && tasksListBox.SelectedItem != null && taskEntry != null)
    {
        var selectedTask = (TaskItem)tasksListBox.SelectedItem;
        taskEntry.Text = selectedTask.Description;
        Tasks.Remove(selectedTask); 
    }
}

        private void OnMarkAsImportantClicked(object sender, RoutedEventArgs e)
        {
            var tasksListBox = this.FindControl<ListBox>("TasksListBox");
            if (tasksListBox != null && tasksListBox.SelectedItem != null)
            {
                var selectedTask = (TaskItem)tasksListBox.SelectedItem;
                selectedTask.IsImportant = !selectedTask.IsImportant;
            }
            else
            {
                ErrorMessage = "Please select a task to mark as important";
            }
        }
    }
}
