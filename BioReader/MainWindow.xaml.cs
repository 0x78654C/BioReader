using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;
using BioRead = BioReader.Utils.Reader;
using FileManage = BioReader.Utils.FileManagement;

namespace BioReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker worker;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BioConvert_Click(object sender, RoutedEventArgs e)
        {
            worker = new BackgroundWorker();
            worker.DoWork += Do_Work;
            worker.RunWorkerAsync();
        }

        private void Do_Work(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                ApplyBionicReader(bioTextConvertor);
            });
        }

        /// <summary>
        /// Convert text to bionic reading.
        /// </summary>
        /// <param name="richTextBox"></param>
        private void ApplyBionicReader(RichTextBox richTextBox)
        {
            workStatusLbl.Content = string.Empty;
            BioRead bioRead = new BioRead(); 
            string normalData = BioRead.StringFromRichTextBox(richTextBox);
            if (string.IsNullOrEmpty(normalData))
                return;
            workStatusLbl.Content = "Applying bionic reading...";
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
            workStatusLbl.Content = "Done!";
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


        /// <summary>
        /// Minimize button(label)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minimizeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// About window open button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutBTN_Click(object sender, RoutedEventArgs e)
        {
         //   var aB = new about();
           // aB.ShowDialog();
        }

        /// <summary>
        /// Close wpf form button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeBTN_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();//close the app
        }

        /// <summary>
        /// Drag window on mouse click left
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

        }

        /// <summary>
        /// Open file in rich text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            FileManage.OpenFile(bioTextConvertor);
        }

        private void SaveFile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            FileManage.SaveFile(bioTextConvertor);
        }
    }
}
