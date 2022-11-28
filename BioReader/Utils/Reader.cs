using MaterialDesignThemes.Wpf.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace BioReader.Utils
{
    public class Reader
    {
        /// <summary>
        /// Data imput for get half letters from a word.
        /// </summary>
        public string Data { get; set; } = string.Empty;

        /// <summary>
        /// Get first half letters from a word.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetHalfChars(string letter)
        {
            var outDictionary = new Dictionary<string, string>();
            if (Data.Length == 0)
                return new Dictionary<string, string>();

            double split = (double)letter.Count() / 2;
            string letterLengt = Math.Round(split, 2).ToString();
            if (letterLengt.Contains("."))
            {
                if (letter.Length > 3)
                    split = Math.Round(split, MidpointRounding.AwayFromZero);
                else
                    split = Math.Round(split, MidpointRounding.ToZero);
            }

            if (letter.Length == 1)
                split = 1;
            int splitChars = Int32.Parse(split.ToString());
            var value = letter.Substring(0, splitChars);
            if (!outDictionary.ContainsKey(letter))
                outDictionary.Add(letter, value);
            return outDictionary;
        }

        public void ApplyBionic(RichTextBox richTextBox)
        {

            TextPointer pointer = richTextBox.Document.ContentStart;
            var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            string line;
            int count = 0;
            int ind = 0;

            //test only
            string[] a = { "Simp", "te", "editor" };
            //  foreach(var w in a)
            File.WriteAllText("x.txt", "");
            List<string> xList = new List<string>();
            //  string[] letters = line.Split(" ");
            string pattern = @"[^\W\d](\w|[-']{1,2}(?=\w))*";
            //TextPointer start = null;

            //TextPointer end = null;

            var words = StringFromRichTextBox(richTextBox).Split(" ");
            string data = StringFromRichTextBox(richTextBox);
            textRange.ClearAllProperties();
            foreach (var word in words)
            {
                foreach (var letter in GetHalfChars(word))
                {
                    if (letter.Key == word)
                    {
                        Regex reg = new Regex(letter.Value, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        Regex reg2 = new Regex(word, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        var start = richTextBox.Document.ContentStart;
                        while (start != null && start.CompareTo(richTextBox.Document.ContentEnd) < 0)
                        {
                            if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                            {
                                var match = reg.Match(start.GetTextInRun(LogicalDirection.Forward));
                                var match0 = reg2.Match(start.GetTextInRun(LogicalDirection.Forward));
                                var index0 = match0.Index;
                                int index = match.Index;
                                int length = index + match.Length;
                                var textrange = new TextRange(start.GetPositionAtOffset(index0, LogicalDirection.Forward), start.GetPositionAtOffset(length, LogicalDirection.Backward));

                                if (letter.Value.Length == 1 && word.Length < 4 &&
                               word.Length > 2 && word.StartsWith(letter.Value))
                                {
                                    textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                                }


                                if (word.ToString().StartsWith(letter.Value) && word.Length >= 2
                                && letter.Value.Length > 1)
                                {
                                    textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                                }
                                start = textrange.End; // I'm not sure if this is correct or skips ahead too far, try it out!!!
                            }
                            start = start.GetNextContextPosition(LogicalDirection.Forward);
                        }
                    }
                }
            }
            // HighlightText(richTextBox, 0,3);
            //HighlightText(richTextBox, 7,2);

            //IEnumerable<TextRange> wordRanges = GetAllWordRanges(richTextBox.Document);
            //foreach (TextRange wordRange in wordRanges)
            //{
            //    foreach (var letter in GetHalfChars(wordRange.Text))
            //    {
            //        if (letter.Key == wordRange.Text)
            //        {
            //            Regex reg = new Regex(letter.Value, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            //           var start = richTextBox.Document.ContentStart;
            //            while (start != null && start.CompareTo(richTextBox.Document.ContentEnd) < 0)
            //            {
            //                if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
            //                {
            //                    var match = reg.Match(start.GetTextInRun(LogicalDirection.Forward));

            //                    var textrange = new TextRange(start.GetPositionAtOffset(match.Index, LogicalDirection.Forward), start.GetPositionAtOffset(match.Index + match.Length, LogicalDirection.Backward));
            //                    textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            //                    start = textrange.End; // I'm not sure if this is correct or skips ahead too far, try it out!!!
            //                }
            //                start = start.GetNextContextPosition(LogicalDirection.Forward);
            //            }
            //        }
            //    }
            //while (pointer != null)
            //{
            //    if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
            //    {
            //        string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
            //        MatchCollection matches = Regex.Matches(textRun, pattern);
            //        count = matches.Count;
            //        ind = count;
            //        foreach (Match match in matches)
            //        {
            //            count--;

            //            foreach (var letter in GetHalfChars(match.ToString()))
            //            {
            //                //textRange.ClearAllProperties();
            //                if (letter.Key == match.ToString())
            //                {
            //                    //int index  = textRange.Text.IndexOf(letter.Key, ind, StringComparison.OrdinalIgnoreCase);
            //                    int index = match.Index;
            //                    ind = index;
            //                    int length = letter.Value.Length + index;
            //                   start = pointer.GetPositionAtOffset(index);
            //                    end = start.GetPositionAtOffset(length);

            //                    var word = match.ToString();
            //                    File.AppendAllText("x.txt", $"{word} | {letter.Value}\n");

            //                    if (end != null)
            //                    {
            //                        //var rangeSelection = new TextRange(start, end);
            //                        //richTextBox.Selection.Select(start, end);
            //                        //rangeSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            //                    }
            //                    HighlightText(richTextBox, index, length, Color.FromRgb(21, 12, 33));
            //                    //Find(richTextBox, textRange, letter.Value, index);
            //                }
            //            }
            //        }
            //        if (count == 0)
            //            return;
            //    }
            //    pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            //}
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
        public static void HighlightText(RichTextBox richTextBox, int startPoint, int endPoint)
        {
            //Trying to highlight charactars here
            TextPointer pointer = richTextBox.Document.ContentStart;
            TextPointer start = null, end = null;
            int count = 0;
            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    if (count == startPoint) start = pointer.GetInsertionPosition(LogicalDirection.Forward);
                    if (count == endPoint) end = pointer.GetInsertionPosition(LogicalDirection.Forward);
                    count++;
                }
                pointer = pointer.GetNextInsertionPosition(LogicalDirection.Forward);
            }
            if (start == null) start = richTextBox.Document.ContentEnd;
            if (end == null) end = richTextBox.Document.ContentEnd;

            TextRange range = new TextRange(start, end);
            range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }
        private static void Find(RichTextBox richTextBox, TextRange textRange, string data, int indexStart)
        {
            //richTextBox.Selection.Select(textRange.Start, textRange.Start);

            richTextBox.CaretPosition = richTextBox.CaretPosition.GetPositionAtOffset(data.Length, LogicalDirection.Forward);

            var index = textRange.Text.IndexOf(data, indexStart, StringComparison.OrdinalIgnoreCase);
            if (index > -1)
            {

                var textPointerStart = textRange.Start.GetPositionAtOffset(index);
                var textPointerEnd = textRange.Start.GetPositionAtOffset(index + data.Length);

                var rangeSelection = new TextRange(textPointerStart, textPointerEnd);
                rangeSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

                //richTextBox.Selection.Select(textPointerStart, textPointerEnd);
            }
        }



        /// <summary>
        /// Read data from richtextbox controler.
        /// </summary>
        /// <param name="rtb"></param>
        /// <returns></returns>
        public static string StringFromRichTextBox(RichTextBox rtb)
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
