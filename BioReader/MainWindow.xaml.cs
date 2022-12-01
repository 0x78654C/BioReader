using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;
using BioRead = BioReader.Utils.Reader;
using FileManage = BioReader.Utils.FileManagement;
using GlobalVariable = BioReader.Utils.GlobalVariables;
using System.Windows.Threading;
using System;

namespace BioReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker worker;
        DispatcherTimer dispatcherTimer;
        private int _clicks = 0;
        private bool _clickMaximize = false;
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer = new DispatcherTimer();
            workStatusLbl.Content = string.Empty;
        }

        /// <summary>
        /// Start bionic reader convertor in background.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BioConvert_Click(object sender, RoutedEventArgs e)
        {
            worker = new BackgroundWorker();
            worker.DoWork += Do_Work;
            worker.RunWorkerAsync();
        }


        /// <summary>
        /// Set timer for clear convertion status after 3 seconds.
        /// </summary>
        /// <param name="dispatcherTimer"></param>
        private void ClearStatusMessage(DispatcherTimer dispatcherTimer)
        {
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Clear convertion status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            workStatusLbl.Content = string.Empty;
        }

        /// <summary>
        /// Background function for start bionic reader convertor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Do_Work(object sender, DoWorkEventArgs e)
        {
            ApplyBionicReader(bioTextConvertor);
        }

        /// <summary>
        /// Convert text to bionic reading.
        /// </summary>
        /// <param name="richTextBox"></param>
        private void ApplyBionicReader(RichTextBox richTextBox)
        {
            Dispatcher.Invoke(() => { workStatusLbl.Content = string.Empty; });
            BioRead bioRead = new BioRead();
            Dispatcher.Invoke(() =>
            {
                workStatusLbl.Content = "Applying bionic reading...";
                bioRead.ApplyBionic(richTextBox);
            });
            Dispatcher.Invoke(() => { workStatusLbl.Content = "Finished converting!"; });
            ClearStatusMessage(dispatcherTimer);
        }


        /// <summary>
        /// Minimize icon event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimize_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _clickMaximize = true;
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Maximimize icon event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maximize_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _clickMaximize = true;
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                MaximizeWindow.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                MaximizeLbl.ToolTip = "Normal";
                return;
            }
            WindowState = WindowState.Normal;
            MaximizeWindow.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
            MaximizeLbl.ToolTip = "Maximize";
        }

        /// <summary>
        /// About window open button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutBTN_Click(object sender, RoutedEventArgs e)
        {
            var about = new About();
            about.ShowDialog();
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
            FileManage.OpenFile(bioTextConvertor, ZoomSlider);
        }

        /// <summary>
        /// Save file to rtf format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            FileManage.SaveFile(bioTextConvertor);
        }

        /// <summary>
        /// Zoom in/out event on richtextbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bioTextConvertor.SetFontSizeRTB(GlobalVariable.defaultFontSize + (double)e.NewValue);
        }


        /// <summary>
        /// Drop file load event for RTB and txt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bioTextConvertor_Drop(object sender, DragEventArgs e)
        {
            bioTextConvertor.SetBackgroundRTB(true);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] data = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (data != null && data.Length > 0)
                {
                    if (data[0].EndsWith(".rtf"))
                    {
                        FileManage.LoadRTFPackage(bioTextConvertor, data[0]);
                        bioTextConvertor.SetFontSizeRTB(GlobalVariable.defaultFontSize);
                        ZoomSlider.Value = 0;
                    }
                    else
                    {
                        FileManage.LoadDataRichTextBox(bioTextConvertor, data[0], true);
                        bioTextConvertor.SetFontSizeRTB(GlobalVariable.defaultFontSize);
                        ZoomSlider.Value = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Change richtextbox background on drag over to gray.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bioTextConvertor_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
            bioTextConvertor.SetBackgroundRTB(false);
        }

        /// <summary>
        /// Change richtextbox background on drag leave to white.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bioTextConvertor_PreviewDragLeave(object sender, DragEventArgs e)
        {
            e.Handled = true;
            bioTextConvertor.SetBackgroundRTB(true);
        }

        /// <summary>
        /// Double click maximize event on title bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetClickCounters(dispatcherTimer);
            if (_clickMaximize)
                _clickMaximize = false;
            else
                _clicks++;
            if (_clicks == 2)
            {
                if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Maximized;
                    MaximizeWindow.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                    MaximizeLbl.ToolTip = "Normal";
                    _clicks = 0;
                    return;
                }
                WindowState = WindowState.Normal;
                MaximizeWindow.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                MaximizeLbl.ToolTip = "Maximize";
                _clicks = 0;
            }
        }

        /// <summary>
        /// Set timer for clear clicks for maximize window to 1 second.
        /// </summary>
        /// <param name="dispatcherTimer"></param>
        private void ResetClickCounters(DispatcherTimer dispatcherTimer)
        {
            dispatcherTimer.Tick += DispatcherTimerReset_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Clear clicks on title bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimerReset_Tick(object sender, EventArgs e)
        {
            _clicks = 0;
        }
    }
}
