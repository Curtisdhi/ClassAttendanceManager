using DrKCrazyAttendance_Instructor.Properties;
using DrKCrazyAttendance_Instructor.Util;
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
using System.Windows.Shapes;

namespace DrKCrazyAttendance_Instructor
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsForm : Window
    {
        private bool locked;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!locked)
            {
                Settings.Default.Classroom = txtClassroom.Text;
                Settings.Default.SqlServerAddr = txtDbServerAddr.Text;
                Settings.Default.SqlDatabase = txtDatabase.Text;
                Settings.Default.SqlUsername = txtDbUsername.Text;
                if (txtDbPassword.Password != null && txtDbPassword.Password != "") 
                    Settings.Default.SqlPassword = txtDbPassword.Password;
                
                Settings.Default.Save();
                this.Close();
            }
            else
            {
                MessageBox.Show("Settings are locked and can not be saved.");
            }
        }

        private void btnUnlock_Click(object sender, RoutedEventArgs e)
        {
            if (!UACHelper.IsProcessElevated())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                startInfo.Verb = "runas";
                try
                {
                    Process p = Process.Start(startInfo);
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    return;
                }
            }
            else
            {
                if (txtPin.Password.Equals(Settings.Default.SecurityPin))
                {
                    locked = false;
                    txtClassroom.IsEnabled = true;
                    txtDatabase.IsEnabled = true;
                    txtDbPassword.IsEnabled = true;
                    txtDbUsername.IsEnabled = true;
                    txtDbServerAddr.IsEnabled = true;

                    txtDbServerAddr.Text = Settings.Default.SqlServerAddr;
                    txtDatabase.Text = Settings.Default.SqlDatabase;
                    txtDbUsername.Text = Settings.Default.SqlUsername;

                    txtPin.IsEnabled = false;
                    btnUnlock.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("The security pin you entered doesn't match.");
                }
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtClassroom.Text = Settings.Default.Classroom;
            uacImage.Source = UACHelper.getUACShield();
        }

    }
}
