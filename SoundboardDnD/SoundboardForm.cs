﻿using SoundboardDnD.Helpers;
using SoundboardDnD.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace SoundboardDnD
{
    public partial class SoundboardForm : Form
    {
        private Dictionary<string, Mp3Info> ButtonMp3s = new Dictionary<string, Mp3Info>();
        private List<ToolStripButton> editButtons = new List<ToolStripButton>();

        private string preferredPath = null;

        public SoundboardForm()
        {
            InitializeComponent();

            tsEditBar.Renderer = new ToolStripRenderer();

            List<Button> buttons = new List<Button>();
            buttons.AddRange(gbBackground.Controls.OfType<Button>());
            buttons.AddRange(gbSingle.Controls.OfType<Button>());

            editButtons.Add(tsbEditDirectory);
            editButtons.Add(tsbEditSource);
            editButtons.Add(tsbEditTitle);

            FileHelpers.GetSettings(buttons, out ButtonMp3s, out preferredPath);
        }

        private void MP3Button_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            var group = b.Parent.Name;
            var mp3 = ButtonMp3s[b.Name];

            if (tsbEditSource.Checked)
            {
                Mp3Helpers.EditSource(b, ButtonMp3s, preferredPath);
                return;
            }
            else if (tsbEditTitle.Checked)
            {
                Mp3Helpers.EditTitle(b, ButtonMp3s);
                return;
            }
            else if (mp3 != null && !string.IsNullOrWhiteSpace(mp3.Path))
            {
                Mp3Helpers.FadeTracks(group, mp3, ButtonMp3s, tbBackground.Value, tbSingle.Value);
                return;
            }
            else
            {
                MessageBox.Show("No track loaded for this button", "No track to play", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void StopAll_Click(object sender, EventArgs e)
        {
            foreach(Mp3Info mp in ButtonMp3s.Values)
            {
                if (mp != null)
                {
                    mp.MP.Stop();
                }
            }
        }

        private void PreferredDirectory_Click(object sender, EventArgs e)
        {
            var path = FileHelpers.OpenFolderDialog();

            preferredPath = path;
        }

        private void MakeButtonActive(object sender, EventArgs e)
        {
            foreach (ToolStripButton button in tsEditBar.Items.OfType<ToolStripButton>())
            {
                button.BackColor = SystemColors.Control;
                button.Checked = false;
            }

            var b = sender as ToolStripButton;
            b.BackColor = Color.Orange;
            b.Checked = true;
        }

        private void tbAdjustVolume_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = sender as TrackBar;
            Group group = Group.None;
            Enum.TryParse(tb.Parent.Name, out group);

            List<MediaPlayer> mp = ButtonMp3s.Where(x => x.Value?.Group == group)
                .Select(x => x.Value.MP).ToList();

            Mp3Helpers.ChangeVolume(mp, ((double)tb.Value / 10));
        }

        private class ToolStripRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
            {
                var btn = e.Item as ToolStripButton;
                if (btn != null && btn.CheckOnClick && btn.Checked)
                {
                    Rectangle bounds = new Rectangle(System.Drawing.Point.Empty, e.Item.Size);
                    e.Graphics.FillRectangle(System.Drawing.Brushes.LightGreen, bounds);
                    e.Item.Text = btn.Text;
                }
                else base.OnRenderButtonBackground(e);
            }
        }
    }
}
