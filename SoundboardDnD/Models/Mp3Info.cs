using SoundboardDnD.Helpers;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;

namespace SoundboardDnD.Models
{
    public enum Group
    {
        Background,
        SoundEffect,
        Ambience,
        None
    }

    public class Mp3Info
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public Group Group { get; set; }
        public MediaPlayer MP { get; set; }

        public bool HasName => string.IsNullOrWhiteSpace(Name);
        public bool HasPath => string.IsNullOrWhiteSpace(Path);
        public bool IsActive => MP == null ? false : MP.Position > new TimeSpan(0, 0, 1);

        public Mp3Info(string path, Group group, string name = null)
        {
            Path = path;
            Group = group;
            Name = name == null ? Mp3Helpers.GetName(path) : name;
            MP = SetupMP(path);
        }

        private MediaPlayer SetupMP(string path)
        {
            var mp = new MediaPlayer();
            mp.Open(new Uri(path));
            mp.MediaEnded += (sender, e) => Mp3InfoExtensions.Media_Ended(sender, e, this.Group);
            return mp;
        }
    }

    public static class Mp3InfoExtensions
    {
        public static bool IsPlayable(this Mp3Info mp3)
        {
            return mp3 != null && !string.IsNullOrWhiteSpace(mp3.Path);
        }

        public static Mp3Info Replace(this Mp3Info mp3, string path, Group group, string name = null)
        {
            mp3.Path = path;
            mp3.Group = group;
            mp3.Name = name == null ? Mp3Helpers.GetName(path) : name;
            mp3.MP?.Stop();
            mp3.MP = mp3.MP ?? new MediaPlayer();
            mp3.MP.Open(new Uri(path));
            mp3.MP.MediaEnded += (sender, e) => Media_Ended(sender, e, mp3.Group);
            return mp3;
        }

        public static void Media_Ended(object sender, EventArgs e, Group group)
        {
            var player = sender as MediaPlayer;

            var control = "cbLoop-" + group;
            var loop = SoundboardForm.ActiveForm?.Controls?.Find(control, true)?.FirstOrDefault() as CheckBox;

            if (loop != null && loop.Checked)
            {
                player.Position = TimeSpan.Zero;
                player.Play();
            }
            else
            {
                player.Position = TimeSpan.Zero;
                player.Stop();
            }
        }
    }
}
