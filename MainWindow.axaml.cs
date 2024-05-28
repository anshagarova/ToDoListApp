using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using Avalonia;
using ReactiveUI;
using System.IO;
using Newtonsoft.Json;

namespace ToDoListApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Tasks { get; init; }

        public MainWindow()
{
    InitializeComponent();
    DataContext = this;
    
    if (File.Exists("tasks.json"))
    {
        string tasksJson = File.ReadAllText("tasks.json");
        Tasks = JsonConvert.DeserializeObject<ObservableCollection<string>>(tasksJson);
    }
    else
    {
        Tasks = new ObservableCollection<string>();
    }

    SaveTasks(); 
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
                ShowMessage("Please enter a task", "Error");
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
                ShowMessage("Please select a task to remove", "Error");
            }
        }

        private void SaveTasks()
        {
            string tasksJson = JsonConvert.SerializeObject(Tasks);
            File.WriteAllText("tasks.json", tasksJson);
        }

        private async void ShowMessage(string message, string title)
        {
            Window? messageBox = null;

            messageBox = new Window
            {
                Title = title,
                Width = 300,
                Height = 200,
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock { Text = message, Margin = new Thickness(20) },
                        new Button { Content = "OK", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, Command = ReactiveCommand.Create(() => { messageBox.Close(); }), Margin = new Thickness(20) }
                    }
                }
            };

            await messageBox.ShowDialog(this);
        }
    }
}
