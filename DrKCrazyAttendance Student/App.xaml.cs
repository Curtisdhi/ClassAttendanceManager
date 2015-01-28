using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DrKCrazyAttendance.Student
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Hidden;
            foreach (string arg in e.Args)
            {
                switch (arg.ToLower())
                {
                    case "settings":
                        mainWindow.OpenSettings();
                        break;
                }
            }
        }

    }
}
