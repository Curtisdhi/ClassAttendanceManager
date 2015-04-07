using DrKCrazyAttendance.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrKCrazyAttendance.Util
{
    //http://www.codeproject.com/Articles/769741/Csharp-AES-bits-Encryption-Library-with-Salt
    public class SecurityCrypt
    {
        //create the key from the assemblies name and a security pin
        private static readonly byte[] KEY = SHA256.Create().ComputeHash(
            Encoding.UTF8.GetBytes(Assembly.GetExecutingAssembly().GetName().Name + Settings.Default.SecurityPin));
        
        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        private static readonly byte[] SALT = new byte[] { 38, 127, 1, 68, 74, 14, 10, 45 };

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted)
        {
            byte[] encryptedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(KEY, SALT, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static string AES_Encrypt(string data)
        {
            string encrypt = "";
            if (!string.IsNullOrEmpty(data))
            {
                //Note encode as base64 to properly store encryption to file
                encrypt = Convert.ToBase64String(AES_Encrypt(Encoding.UTF8.GetBytes(data)));
            }
            return encrypt;
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted)
        {
            byte[] decryptedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(KEY, SALT, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static string AES_Decrypt(string data)
        {
            string decrypt = "";
            if (!string.IsNullOrEmpty(data))
            {
                try {
                    //Note, we decode from base64 and decrypt the contents and THEN encode using UTF8
                    decrypt = Encoding.UTF8.GetString(AES_Decrypt(Convert.FromBase64String(data)));
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Set Encyrpt to \"False\"\n"+ ex.Message, "Settings error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    throw ex;
                }
            }
            return decrypt;
        }

    }
}
