using Newtonsoft.Json;
using SoundboardDnD.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundboardDnD.Helpers
{
    public static class FileHelpers
    {
        private static string PreferredPath = null;

        public static void SaveSettings(Dictionary<string, Mp3Info> buttonMp3s, string preferredPath = null)
        {
            var mp3ToSave = new Dictionary<string, Mp3Info>();
            PreferredPath = preferredPath;

            foreach (KeyValuePair<string, Mp3Info> mp3 in buttonMp3s)
            {
                if (mp3.Value != null)
                {
                    Mp3Info mp = new Mp3Info(mp3.Value.Path, mp3.Value.Group);
                    mp.MP = null;

                    mp3ToSave.Add(mp3.Key, mp);
                }
            }

            var saveObject = new SaveInfo { 
                Mp3Info = mp3ToSave, 
                PreferredPath = string.IsNullOrWhiteSpace(preferredPath) ? PreferredPath : preferredPath
            };

            var saved = JsonConvert.SerializeObject(saveObject);

            File.WriteAllText("Soundboardsettings.txt", saved);
        }

        public static void GetSettings(List<Button> buttons, out Dictionary<string, Mp3Info> buttonMp3s, out string preferredPath)
        {
            preferredPath = "c:\\"; 
            try
            {
                var saved = File.ReadAllText("Soundboardsettings.txt");
                SaveInfo savedInfo = JsonConvert.DeserializeObject<SaveInfo>(saved);
                preferredPath = savedInfo.PreferredPath;
                PreferredPath = savedInfo.PreferredPath;
                buttonMp3s = new Dictionary<string, Mp3Info>();

                foreach (KeyValuePair<string, Mp3Info> x in savedInfo.Mp3Info)
                {
                    if (x.Value != null)
                        buttonMp3s.Add(x.Key, new Mp3Info(x.Value.Path, x.Value.Group, x.Value.Name));
                }

                foreach (var button in buttons)
                {
                    if (buttonMp3s.ContainsKey(button.Name) && !string.IsNullOrWhiteSpace(buttonMp3s[button.Name]?.Name))
                    {
                        button.Text = buttonMp3s[button.Name].Name;
                    }
                    else if (!buttonMp3s.ContainsKey(button.Name))
                    {
                        buttonMp3s.Add(button.Name, null);
                        button.Text = "[____]";
                    }
                    else
                    {
                        button.Text = "[____]";
                    }
                }
            }
            catch
            {
                buttonMp3s = new Dictionary<string, Mp3Info>();

                foreach (var button in buttons)
                {
                    if (!buttonMp3s.ContainsKey(button.Name))
                    {
                        buttonMp3s.Add(button.Name, null);
                    }
                }

                SaveSettings(buttonMp3s, preferredPath);
            }
        }

        public static string OpenFileDialog(string preferredPath)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = string.IsNullOrWhiteSpace(preferredPath) ? "c:\\" : preferredPath;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }

            return filePath;
        }

        public static string OpenFolderDialog()
        {
            string path = string.Empty;

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    path = fbd.SelectedPath;
                }
            }

            return path;
        }
    }
}
