using DrKCrazyAttendance.Util;
using System;
using System.Text;
namespace DrKCrazyAttendance.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    public sealed partial class Settings {
        
        public Settings() {
            this.SettingsSaving += SettingsSavingEventHandler;
            this.SettingsLoaded += SettingsLoadingEventHandler;
        }

        //Decrypts settings from config file
        private void SettingsLoadingEventHandler(object sender, System.Configuration.SettingsLoadedEventArgs e)
        {
            Settings.Default.Instructor = Environment.UserName;
            if (!Settings.Default.Encrypted)
            {
                //Encrypts
                Settings.Default.Save();
            }
        }
        
        //Encrypts before being written to a file
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            if (!Settings.Default.Encrypted)
            {
                Settings.Default.SqlDatabase = SecurityCrypt.AES_Encrypt(Settings.Default.SqlDatabase);
                Settings.Default.SqlServerAddr = SecurityCrypt.AES_Encrypt(Settings.Default.SqlServerAddr);
                Settings.Default.SqlUsername = SecurityCrypt.AES_Encrypt(Settings.Default.SqlUsername);
                Settings.Default.SqlPassword = SecurityCrypt.AES_Encrypt(Settings.Default.SqlPassword);
                Settings.Default.Encrypted = true;
            }
        }
    }
}
