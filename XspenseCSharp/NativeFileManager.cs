using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XspenseCSharp
{
    class NativeFileManager
    {
        public static NativeFileManager shared = new NativeFileManager();
        string programDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        public void SaveTextToFile(string text, string fileName)
        {
            string filePath = Path.Combine(programDataFolderPath, fileName + ".heng");
            try
            {
                File.WriteAllText(filePath, text);
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
            string filePath = Path.Combine(programDataFolderPath, fileName + ".heng");
            try
            {
                text = File.ReadAllText(filePath);
                Console.WriteLine("Text read from file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return text;
        }
        public bool IsFileExists(string fileName)
        {
            string filePath = Path.Combine(programDataFolderPath, fileName + ".heng");
            return File.Exists(filePath);
        }
    }
}
