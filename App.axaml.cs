using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace ToDoListApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
{
    var mainWindow = new MainWindow();

    string iconPath = "avares:Icons/icon.png";
System.Diagnostics.Debug.WriteLine($"Checking for icon file at: {iconPath}");

if (System.IO.File.Exists(iconPath))
{
    mainWindow.Icon = new WindowIcon(iconPath);
    System.Diagnostics.Debug.WriteLine("Icon file found and set successfully.");
}
else
{
    System.Diagnostics.Debug.WriteLine("Icon file not found.");
}

    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        desktop.MainWindow = mainWindow; // Assign the initialized mainWindow instance
    }

    base.OnFrameworkInitializationCompleted();
}

}