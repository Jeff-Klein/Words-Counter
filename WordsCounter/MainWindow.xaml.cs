using Counters.FrequencyCounter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace WordsCounter
{
    
    public partial class MainWindow : Window
    {
        private List<KeyValuePair<string, int>> WordsList { get; set; }
        private FrequencyWordsCounter fwCounter;

        public MainWindow()
        {
            InitializeComponent();
            fwCounter = new FrequencyWordsCounter();
            fwCounter.OnProgressChange += UpdateProgressBar;
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            if (btnExecute.Content.ToString() == "Cancel")
                CancelProcessing();
            else if (CheckForValidFile())
            {
                ProcessingStatus();
                GetWordsAsync(tbxFileName.Text);
            };             
        }

        private bool CheckForValidFile()
        {
            var fileName = tbxFileName.Text;

            if (fileName == "")
            {
                lblError.Content = "Please inform a file name before start.";
                return false;
            }
            else if(!File.Exists(fileName))
            {
                lblError.Content = "Could not find the specified file.";
                return false;
            }
            else if (Path.GetExtension(fileName) != ".txt")
            {
                lblError.Content = "Only text files are accepted.";
                return false;
            }

            return true;
                
        }

        private void CancelProcessing()
        {
            fwCounter.cancelToken.Cancel();
            pbConclusion.Foreground = Brushes.Red;
        }

        private async void GetWordsAsync(string fileName)
        {
            try
            {
                WordsList  = await CallFrequencyLibraryTask(fileName);
                dgWords.ItemsSource = WordsList;
                DoneStatus();
            }
            catch (Exception ex)
            {
                ErrorStatus(ex.Message);
            }

        }

        private Task<List<KeyValuePair<string, int>>> CallFrequencyLibraryTask(string fileName)
        {
            return Task.Run(() =>
            {
                var resultFromCounter = fwCounter.CountWordsFromTextFile(fileName);
                return new List<KeyValuePair<string, int>>(resultFromCounter);
            });
        }

        private void ProcessingStatus()
        {
            if(WordsList != null)
                WordsList.Clear();

            pbConclusion.Foreground = Brushes.LimeGreen;
            lblError.Content = string.Empty;
            lblStatus.Content = "Processing";
            UpdateProgressBar(0);
            lblPercentage.Visibility = Visibility.Visible;
            btnExecute.Background = Brushes.Red;
            btnExecute.Content = "Cancel";
        }

        private void DoneStatus()
        {
            btnExecute.Background = Brushes.LimeGreen;
            btnExecute.Content = "Execute";

            if (pbConclusion.Value == 100)
                lblStatus.Content = "Done";
            else
                lblStatus.Content = "Cancelled";           
        }

        private void ErrorStatus(string errorMessage)
        {
            pbConclusion.Foreground = Brushes.Red;
            lblError.Content = errorMessage;
            lblStatus.Content = "Error";
            btnExecute.Background = Brushes.LimeGreen;
            btnExecute.Content = "Execute";
        }

        private void UpdateProgressBar(double percentage)
        {
            this.Dispatcher.Invoke(() =>
            {
                pbConclusion.Value = percentage;
            });
        }

        private void FileZoom(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text Files(*.txt)|*.txt";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
                tbxFileName.Text = dialog.FileName;
        }
    }
}
