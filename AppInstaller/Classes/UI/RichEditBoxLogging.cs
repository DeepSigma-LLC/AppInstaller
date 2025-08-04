using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstallerUI.Classes.UI
{
    internal static class RichEditBoxLogging
    {
        internal static void AppendColoredText(RichEditBox box, string text, System.Drawing.Color color)
        {
            bool wasReadOnly = box.IsReadOnly;
            if (wasReadOnly) { box.IsReadOnly = false; }

            try
            {
                var doc = box.Document;

                // Move selection to end of document
                doc.GetText(Microsoft.UI.Text.TextGetOptions.None, out string existingText);
                int end = existingText.Length;
                doc.Selection.SetRange(end, end); // collapse selection

                // SAFELY get and apply format
                var format = doc.Selection.CharacterFormat;
                var safeColor = ConvertColor(color);

                if (format != null)
                {
                    format.ForegroundColor = safeColor;
                    format.Bold = Microsoft.UI.Text.FormatEffect.Off;
                }

                // Insert text
                doc.Selection.Text = text;

                // Move caret to end
                int newEnd = doc.Selection.EndPosition;
                doc.Selection.SetRange(newEnd, newEnd);
            }
            finally
            {
                if (wasReadOnly)
                {
                    box.IsReadOnly = true;
                }
            }
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

        /// <summary>
        /// Convers color from system drawing color to the windows UI color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
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
