using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BioRead = BioReader.Utils.Reader;

namespace BioReader.Utils
{

    public class FileManagement
    {
        private static OpenFileDialog s_openFileDialog = new OpenFileDialog();
        private static SaveFileDialog s_saveFileDialog = new SaveFileDialog();

        /// <summary>
        /// Open file.
        /// </summary>
        /// <param name="richTextBox"></param>
        public static void OpenFile(RichTextBox richTextBox, Slider slider)
        {
            s_openFileDialog.Filter = "Text files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";
            s_openFileDialog.Title = "Select file to open in convertor";
            Nullable<bool> result = s_openFileDialog.ShowDialog();
            if (result == true)
            {
                richTextBox.Document.Blocks.Clear();
                string filePath = s_openFileDialog.FileName;
                if (filePath.EndsWith(".rtf"))
                {
                    LoadRTFPackage(richTextBox, filePath);
                    richTextBox.SetFontSizeRTB(GlobalVariables.defaultFontSize);
                    slider.Value = 0;
                    return;
                }
                LoadDataRichTextBox(richTextBox, s_openFileDialog.FileName, true);
                richTextBox.SetFontSizeRTB(GlobalVariables.defaultFontSize);
                slider.Value = 0;
            }
        }

        /// <summary>
        /// Load data in richtextbox
        /// </summary>
        /// <param name="richTextBox">Richtextbox controler.</param>
        /// <param name="data">Data to be loaded(text/file path)</param>
        /// <param name="path">Enable/Disable read data from file.</param>
        public static void LoadDataRichTextBox(RichTextBox richTextBox, string data, bool path)
        {
            richTextBox.Document.Blocks.Clear();
            var paragraph = new Paragraph();
            if (path)
                paragraph.Inlines.Add(File.ReadAllText(data));
            else
                paragraph.Inlines.Add(data);

            richTextBox.Document.Blocks.Add(paragraph);
        }

        /// <summary>
        /// Save converted data to RTF file.
        /// </summary>
        /// <param name="richTextBox"></param>
        public static void SaveFile(RichTextBox richTextBox)
        {
            s_saveFileDialog.Title = "Save file";
            s_saveFileDialog.Filter = "Rich Text Format (.rtf)|*.rtf";
            Nullable<bool> result = s_saveFileDialog.ShowDialog();
            string dataToSave = BioRead.StringFromRichTextBox(richTextBox);

            if (result == true && !string.IsNullOrEmpty(dataToSave))
            {
                RichTextBoxSave(richTextBox, s_saveFileDialog.FileName);
            }
        }


        /// <summary>
        /// Save RIF in RichTextBox to a file specified by fileName.
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="fileName"></param>
        private static void RichTextBoxSave(RichTextBox richTextBox, string fileName)
        {
            try
            {
                TextRange range;
                FileStream fStream;
                range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                fStream = new FileStream(fileName, FileMode.Create);
                range.Save(fStream, DataFormats.Rtf);
                fStream.Close();
                MessageBox.Show($"Data saved to: {fileName}", "BioReader", MessageBoxButton.OK,
        MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}", "BioReader", MessageBoxButton.OK,
        MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Load RTF files in richtextbox.
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="fileName"></param>
        public static void LoadRTFPackage(RichTextBox richTextBox, string fileName)
        {
            TextRange range;
            FileStream fStream;
            if (File.Exists(fileName))
            {
                range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                fStream = new FileStream(fileName, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                richTextBox.FontSize = 16;
                fStream.Close();
            }
        }
    }
}
