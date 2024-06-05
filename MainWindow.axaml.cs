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
        public ObservableCollection<string> Tasks { get; set; }
        public string ErrorMessage { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists("tasks.json"))
            {
                try
                {
                    string tasksJson = File.ReadAllText("tasks.json");
                    Tasks = JsonConvert.DeserializeObject<ObservableCollection<string>>(tasksJson) ?? new ObservableCollection<string>();
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Error loading tasks: " + ex.Message;
                    Tasks = new ObservableCollection<string>();
                }
            }
            else
            {
                Tasks = new ObservableCollection<string>();
            }

            DataContext = this;
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
                Tasks.Add(taskEntry.Text);
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
                Tasks.Remove((string)tasksListBox.SelectedItem);
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
    }
}
