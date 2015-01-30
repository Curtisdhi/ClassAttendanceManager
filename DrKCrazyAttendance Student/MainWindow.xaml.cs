using DrKCrazyAttendance.Properties;
using DrKCrazyAttendance.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrKCrazyAttendance.Student
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsForm settingsForm;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        public static MainWindow Instance
        {
            get;
            private set;
        }

        public void OpenSettings()
        {
            if (!UACHelper.IsProcessElevated())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                startInfo.Verb = "runas";
                //add this to tell our program to auto open settings form
                startInfo.Arguments = "settings"; 
                try
                {
                    Process p = Process.Start(startInfo);
                    this.Close();
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    MessageBox.Show("Warning! Unable to restart program with elevated privledges.");
                }
            }
            else
            {
                if (settingsForm == null)
                {
                    settingsForm = new SettingsForm();
                    settingsForm.Show();
                }

                settingsForm.Focus();
            }
        }

        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            OpenSettings();
        }

        private void MenuItem_Attendance_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            uacImage.Source = UACHelper.getUACShield();
        }

    }
}
