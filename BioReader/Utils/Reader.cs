using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

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
        public List<string> GetHalfChars()
        {
            List<string> outPut = new List<string>();

            if (Data.Length == 0)
                return outPut;

            using (StringReader reader = new StringReader(Data))
            {
                string line;
                while (null != (line = reader.ReadLine()))
                {
                    string[] letters = line.Split(" ");
                    foreach (var letter in letters)
                    {
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
                        outPut.Add(letter.Substring(0, splitChars));
                    }
                }
            }
            return outPut;
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
