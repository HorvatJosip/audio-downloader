using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AudioDownloader.WinFormsClient
{
    public static class Utils
    {
        public static void AvoidSpecificCharacters(TextBox textBox, IEnumerable<char> charactersToAvoid)
        {
            textBox.TextChanged += (sender, e) =>
            {
                if (textBox.TextLength == 0) return;

                var charIndex = textBox.SelectionStart - 1;

                if (charIndex >= 0 && charactersToAvoid.Contains(textBox.Text[charIndex]))
                {
                    textBox.Text = textBox.Text.Remove(charIndex, 1);

                    textBox.SelectionStart = charIndex;
                    textBox.SelectionLength = 0;
                }
            };
        }
    }
}
