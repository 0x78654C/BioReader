using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BioReader
{
    public static class Extensions
    {
        /// <summary>
        /// Set/Restore background color for richtextbox on drag and drop event.
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="restore"></param>
        public static void SetBackgroundRTB(this RichTextBox richTextBox, bool restore)
        {
            SolidColorBrush brush;
            if (restore)
                brush = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
            else
                brush = new SolidColorBrush(Color.FromRgb(0xC0, 0xC0, 0xC0));
            richTextBox.Background = brush;
        }

        /// <summary>
        /// Set the font size of Richtextbox
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="size"></param>
        public static void SetFontSizeRTB(this RichTextBox richTextBox, double size)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            textRange.ApplyPropertyValue(Control.FontSizeProperty, size);
        }
    }
}
