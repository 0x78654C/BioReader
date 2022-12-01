using MaterialDesignThemes.Wpf.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics.Tracing;
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
using Bio = BioLib.Reader;

namespace BioReader.Utils
{
    public class Reader
    {

        StringBuilder _builder = new StringBuilder();

        /// <summary>
        /// Append bolded text in rtf format
        /// </summary>
        /// <param name="word"></param>
        private void AppendBold(string word)
        {
            _builder.Append(@"\b ");
            _builder.Append(word);
            _builder.Append(@"\b0");
        }

        /// <summary>
        /// Append word with space.
        /// </summary>
        /// <param name="word"></param>
        private void Append(string word) => _builder.Append($"{word} ");

        /// <summary>
        /// Append new line
        /// </summary>
        private void AppendLine() => _builder.Append(@"\line");

        /// <summary>
        /// Converto string builder data to rtf format.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private string ConvertStringToRtf(StringBuilder builder) => @"{\rtf1\ansi " + builder.ToString() + " }";

        /// <summary>
        /// Apply bionic reader converstion.
        /// </summary>
        /// <param name="richTextBox"></param>
        public void ApplyBionic(RichTextBox richTextBox)
        {
            string data = StringFromRichTextBox(richTextBox);
            if (string.IsNullOrEmpty(data))
                return;
            string line = string.Empty;
            var readerData = new StringReader(data);
            while ((line = readerData.ReadLine()) != null)
            {
                string[] words = line.Split(" ");

                foreach (var word in words)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        var halfWords = Bio.GetHalfChars(word);
                        foreach (var bioWrod in halfWords)
                        {
                            var secondHalf = bioWrod.Key.Substring(bioWrod.Value.Length);
                            AppendBold(bioWrod.Value);
                            Append(secondHalf);
                        }
                    }
                }
                AppendLine();
            }
            var rtf = ConvertStringToRtf(_builder);
            var docBytes = Encoding.UTF8.GetBytes(rtf);
            using (var reader = new MemoryStream(docBytes))
            {
                reader.Position = 0;
                richTextBox.SelectAll();
                richTextBox.Selection.Load(reader, DataFormats.Rtf);
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
