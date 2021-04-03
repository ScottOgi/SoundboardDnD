using SoundboardDnD.Helpers;
using System;
using System.Windows.Media;

namespace SoundboardDnD.Models
{
    public enum Group
    {
        Background,
        Single,
        Playlist,
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
            mp.MediaEnded += new EventHandler(Media_Ended);
            return mp;
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            var player = sender as MediaPlayer;
            player.Position = TimeSpan.Zero;
            player.Stop();
        }

    }
}
