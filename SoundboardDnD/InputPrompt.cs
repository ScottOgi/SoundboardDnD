using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundboardDnD
{
    public static class InputPrompt
    {
        public static string ShowDialog(string text, string caption, string placeHolder = null)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400, Text = placeHolder };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => {
                if (Validate(textBox.Text)) prompt.Close();
                else
                {
                    prompt.DialogResult = DialogResult.None;
                    MessageBox.Show("Title cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : placeHolder;
        }

        private static bool Validate(string text)
        {
            return !string.IsNullOrWhiteSpace(text) && text.Length > 1;
        }
    }
}
