using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

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

            if (File.Exists("tasks.json"))
            {
                try
                {
                    string tasksJson = File.ReadAllText("tasks.json");
                    var loadedTasks = JsonConvert.DeserializeObject<ObservableCollection<TaskItem>>(tasksJson) ?? new ObservableCollection<TaskItem>();
                    foreach (var task in loadedTasks)
                    {
                        Tasks.Add(task);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error loading tasks: " + ex.Message;
                }
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnAddTaskClicked(object sender, RoutedEventArgs e)
        {
            var taskEntry = this.FindControl<TextBox>("TaskEntry");
            if (taskEntry != null && !string.IsNullOrWhiteSpace(taskEntry.Text))
            {
                Tasks.Add(new TaskItem(taskEntry.Text, false)); 
                taskEntry.Text = string.Empty;
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
                Tasks.Remove((TaskItem)tasksListBox.SelectedItem);
                SaveTasks();
            }
            else
            {
                ErrorMessage = "Please select a task to remove";
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

        private void OnMarkAsImportantClicked(object sender, RoutedEventArgs e)
        {
            var tasksListBox = this.FindControl<ListBox>("TasksListBox");
            if (tasksListBox != null && tasksListBox.SelectedItem != null)
            {
                var selectedTask = (TaskItem)tasksListBox.SelectedItem;
                selectedTask.IsImportant = !selectedTask.IsImportant;
                SaveTasks();
            }
            else
            {
                ErrorMessage = "Please select a task to mark as important";
            }
        }
    }
}
