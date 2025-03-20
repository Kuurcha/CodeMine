using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.Helper
{
    internal static class FileHelper
    {

        /// <summary>
        /// Saves text to a file in the user:// directory.
        /// </summary>
        public static void SaveText(string text, string FilePath)
        {
            using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Write);
            file.StoreString(text);
            GD.Print("Text saved successfully!");
        }

        /// <summary>
        /// Loads text from the saved file.
        /// </summary>
        public static string LoadText(string FilePath)
        {
            if (FileAccess.FileExists(FilePath))
            {
                using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Read);
                string loadedText = file.GetAsText();
                GD.Print("Text loaded successfully!");
                return loadedText;
            }

            GD.Print("No saved text found.");
            return "";
        }
    }
}
