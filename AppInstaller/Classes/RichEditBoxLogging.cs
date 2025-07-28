using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller.Classes
{
    internal static class RichEditBoxLogging
    {
        internal static void AppendColoredText(RichEditBox box, string text, System.Drawing.Color color)
        {
            // Move selection to the end
            box.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string existingText);
            int length = existingText.Length;

            // Set selection at end of document
            var doc = box.Document;
            doc.Selection.SetRange(length, length);

            // Insert text
            doc.Selection.Text = text;

            // Apply color format to just-inserted text
            doc.Selection.CharacterFormat.ForegroundColor = ConvertColor(color);
        }

        internal static void AppendColoredText(RichTextBlock block, string text, System.Drawing.Color color)
        {
            var run = new Run
            {
                Text = text,
                Foreground = new SolidColorBrush(ConvertColor(color))
            };

            Paragraph paragraph;

            // Reuse the last paragraph if available
            if (block.Blocks.LastOrDefault() is Paragraph lastParagraph)
            {
                paragraph = lastParagraph;
            }
            else
            {
                paragraph = new Paragraph();
                block.Blocks.Add(paragraph);
            }

            paragraph.Inlines.Add(run);
        }


        private static Windows.UI.Color ConvertColor(System.Drawing.Color color)
        {
            Windows.UI.Color winUIColor = Windows.UI.Color.FromArgb(
                color.A,
                color.R,
                color.G,
                color.B);
            return winUIColor;
        }

    }
}
