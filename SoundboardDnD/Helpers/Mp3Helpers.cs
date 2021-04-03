using SoundboardDnD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;

namespace SoundboardDnD.Helpers
{
    public static class Mp3Helpers
    {
        public static bool EditSource(Button b, Dictionary<string, Mp3Info> buttonMp3s, string preferredPath)
        {
            var path = HandleMP3Selection(b.Name, preferredPath);

            if (!string.IsNullOrWhiteSpace(path))
            {
                Group group = Group.None;
                Enum.TryParse(b.Parent.Name, out group);

                if (buttonMp3s[b.Name] == null)
                {
                    buttonMp3s[b.Name] = new Mp3Info(path, group);
                }

                buttonMp3s[b.Name] = new Mp3Info(path, group);
                b.Text = GetName(path);
            }

            FileHelpers.SaveSettings(buttonMp3s, preferredPath);

            return false;
        }

        public static void FadeTracks(string group, Mp3Info mp3, Dictionary<string, Mp3Info> buttonMp3s, double bgVolume, double snVolume)
        {
            if (group == nameof(Group.gbBackground))
            {
                var items = buttonMp3s.Where(x => x.Value != null).ToList();
                var activePlayer = items.FirstOrDefault(x => x.Value.IsActive && x.Value.Group == Group.gbBackground).Value?.MP;

                if (mp3.MP == null) mp3.MP = new MediaPlayer();
                mp3.MP.Open(new Uri(mp3.Path));

                if (activePlayer != null)
                {
                    mp3.MP.Volume = 0;
                    mp3.MP.Play();
                    int seconds = 5;

                    Timer t = new Timer();
                    t.Interval = 1000;
                    t.Tick += new EventHandler((s, ea) => seconds = HandleTick(activePlayer, mp3.MP, seconds, bgVolume, s));
                    t.Start();
                }
                else
                {
                    mp3.MP.Volume = bgVolume / 10;
                    mp3.MP.Play();
                }
            }
            else
            {
                mp3.MP.Volume = snVolume / 10;
                mp3.MP.Play();
            }
        }

        private static int HandleTick(MediaPlayer a, MediaPlayer b, int seconds, double maxVolume, object sender)
        {
            var volDiff = (maxVolume / 10) / 5;
            a.Volume -= volDiff;
            if (b.Volume < (maxVolume / 10)) b.Volume += volDiff;
            seconds--;
            if (seconds == 0)
            {
                var s = sender as Timer;
                a.Stop();
                s.Stop();
            }
            return seconds;
        }

        public static string HandleMP3Selection(string buttonId, string preferredPath)
        {
            var path = FileHelpers.OpenFileDialog(preferredPath);

            if (string.IsNullOrWhiteSpace(path)) return null;

            return path;
        }

        public static bool EditTitle(Button b, Dictionary<string, Mp3Info> buttonMp3s)
        {
            var mp3 = buttonMp3s[b.Name];

            if (mp3 == null || (mp3.HasName && mp3.HasPath))
            {
                MessageBox.Show("No track loaded so no name to edit!", "No track to rename", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var newTitle = InputPrompt.ShowDialog("Enter a new title for this track", "New track title", mp3.Name);

            if (buttonMp3s.ContainsKey(b.Name))
            {
                buttonMp3s[b.Name].Name = newTitle;
                b.Text = buttonMp3s[b.Name].Name;
            }

            FileHelpers.SaveSettings(buttonMp3s);

            return false;
        }

        public static string GetName(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1, path.LastIndexOf(".") - path.LastIndexOf("\\"));
        }

        public static void ChangeVolume(List<MediaPlayer> mp, double volume)
        {
            mp.ForEach(x =>
            {
                if (x == null) return;
                x.Volume = volume; 
            });
        }
    }

    public static class ControlExtensions
    {
        public static Group GetGroup(this Button b)
        {
            return Group.None;
        }
    }

}
