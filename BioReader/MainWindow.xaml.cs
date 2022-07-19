using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BioRead = BioReader.Utils.Reader;

namespace BioReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bioTextConvertor_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private void BioConvert_Click(object sender, RoutedEventArgs e)
        {
              ApplyBionicReader(bioTextConvertor);
          //  bioTextConvertor.Selection.Select
        }

        private void ApplyBionicReader(RichTextBox richTextBox)
        {
            BioRead bioRead = new BioRead();
            Paragraph paragraph = new Paragraph();
            string normalData = StringFromRichTextBox(richTextBox);
            if (string.IsNullOrEmpty(normalData))
                return;
            bioRead.Data = normalData;
            List<string> firstCharaters = bioRead.GetHalfChars();

            foreach (var bioChars in firstCharaters)
            {
                IEnumerable<TextRange> wordRanges = GetAllWordRanges(richTextBox.Document);
                foreach (TextRange wordRange in wordRanges)
                {
                    if (wordRange.Text.Contains($"{bioChars}"))
                    {
                        wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, FontWeights.Bold);
                    }
                }
                //TextPointer text = richTextBox.Document.ContentStart;
                //while (text.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
                //{
                //    text = text.GetNextContextPosition(LogicalDirection.Forward);
                //}
                //TextPointer startPos = text.GetPositionAtOffset(5);
                //TextPointer endPos = text.GetPositionAtOffset(7);
                //var textRange = new TextRange(startPos, endPos);
                //bioTextConvertor.Selection.Select(startPos, endPos);
                //bioTextConvertor.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
        }

        public static IEnumerable<TextRange> GetAllWordRanges(FlowDocument document)
        {
            string pattern = @"[^\W\d](\w|[-']{1,2}(?=\w))*";
            TextPointer pointer = document.ContentStart;
            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    MatchCollection matches = Regex.Matches(textRun, pattern);
                    foreach (Match match in matches)
                    {
                        int startIndex = match.Index;
                        int length = match.Length;
                        TextPointer start = pointer.GetPositionAtOffset(startIndex);
                        TextPointer end = start.GetPositionAtOffset(length);
                        yield return new TextRange(start, end);
                    }
                }
                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtb.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtb.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            return textRange.Text;
        }
    }
}
