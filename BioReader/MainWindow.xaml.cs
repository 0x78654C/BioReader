using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
        private void BioConvert_Click(object sender, RoutedEventArgs e)
        {
            ApplyBionicReader(bioTextConvertor);
        }

        private void ApplyBionicReader(RichTextBox richTextBox)
        {
            BioRead bioRead = new BioRead(); 
            string normalData = StringFromRichTextBox(richTextBox);
            if (string.IsNullOrEmpty(normalData))
                return;
            bioRead.Data = normalData;
            List<string> firstCharaters = bioRead.GetHalfChars();

            foreach (var bioChars in firstCharaters)
            {
                string pattern = @"[^\W\d](\w|[-']{1,2}(?=\w))*";
                TextPointer pointer = richTextBox.Document.ContentStart;

                while (pointer != null)
                {
                    if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                    {
                        string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                        MatchCollection matches = Regex.Matches(textRun, pattern);
                        foreach (Match match in matches)
                        {
                            int startIndex = match.Index;
                            int length = bioChars.Length;
                            TextPointer start = pointer.GetPositionAtOffset(startIndex);
                            TextPointer end = start.GetPositionAtOffset(length);
                            if (end != null)
                            {
                                TextRange range = new TextRange(start, end);
                                string word = range.Text;

                                if (bioChars.Length == 1 && match.ToString().Length < 4 &&
                                   match.ToString().Length > 1 && match.ToString().StartsWith(bioChars))
                                {
                                    richTextBox.Selection.Select(start, end);
                                    richTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                                }


                                if (match.ToString().StartsWith(bioChars) && match.ToString().Length >= 3
                                    && bioChars.Length > 1)
                                {
                                    richTextBox.Selection.Select(start, end);
                                    richTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                                }
                            }
                        }
                    }
                    pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
                }
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
