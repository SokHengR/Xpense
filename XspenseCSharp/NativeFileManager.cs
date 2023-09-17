using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XspenseCSharp
{
    class NativeFileManager
    {
        public static NativeFileManager shared = new NativeFileManager();
        string programDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        public void SaveTextToFile(string text, string fileName)
        {
            string filePath = Path.Combine(programDataFolderPath, fileNameToPath(fileName));
            try
            {
                File.WriteAllText(filePath, Base64Encode(text));
                Console.WriteLine("Text saved to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public string ReadTextFromFile(string fileName)
        {
            string text = string.Empty;
            string filePath = Path.Combine(programDataFolderPath, fileNameToPath(fileName));
            try
            {
                text = File.ReadAllText(filePath);
                Console.WriteLine("Text read from file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return Base64Decode(text);
        }
        public bool IsFileExists(string fileName)
        {
            string filePath = Path.Combine(programDataFolderPath, fileNameToPath(fileName));
            return File.Exists(filePath);
        }

        public void deleteFile(string fileName)
        {
            if (IsFileExists(fileName))
            {
                string filePath = Path.Combine(programDataFolderPath, fileNameToPath(fileName));
                File.Delete(filePath);
            }
        }

        private string fileNameToPath(string fileName)
        {
            if (!Directory.Exists(programDataFolderPath + "/Xpense/"))
            {
                Directory.CreateDirectory(programDataFolderPath + "/Xpense/");
            }
            return "Xpense/" + fileName + ".dat";
        }

        string Base64Encode(string text)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes);
        }

        string Base64Decode(string base64)
        {
            byte[] base64Bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(base64Bytes);
        }
    }
}
