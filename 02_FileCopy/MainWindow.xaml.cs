using IOExtensions;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _02_FileCopy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel model;
        public MainWindow()
        {
            InitializeComponent();
            model = new ViewModel()
            {
                Source = "C:\\Users\\helen\\Downloads\\1GB.bin",
                Destination = "C:\\Users\\helen\\Desktop\\test",
                Progress = 0
            };

            this.DataContext = model;

        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            //copy file from sourse to destination
            string fileName = System.IO.Path.GetFileName(model.Source);
            //C:\\Users\\helen\\Desktop\\test\\1.png           
            string destFilePath = System.IO.Path.Combine(model.Destination, fileName);
            CopyProcessInfo info = new CopyProcessInfo(fileName);
            model.AddProcess(info);
            await CopyFileAsync(model.Source, destFilePath, info);
            info.Percentage = 100;
            MessageBox.Show("Complete!!!");
        }
        private Task CopyFileAsync(string src, string dest, CopyProcessInfo info)
        {
            //type 3 - using FileTransferManager
            return FileTransferManager.CopyWithProgressAsync(src, dest, (progress) =>
            {
                model.Progress = progress.Percentage;
                info.Percentage = progress.Percentage;
                info.BytesPerSecond = progress.BytesPerSecond ;
            }, false);
            #region Copy file hand


            //return Task.Run(() =>
            //{
            //1 - using File Class
            //File.Copy(Source, destFilePath, true);
            //2 - FileStream
            //using FileStream srcStream = new FileStream(src, FileMode.Open, FileAccess.Read);
            //using FileStream desStream = new FileStream(dest, FileMode.Create, FileAccess.Write);
            //byte[] buffer = new byte[1024 * 8];//8Kb
            //int bytes = 0;
            //do
            //{
            //    bytes = srcStream.Read(buffer, 0, buffer.Length);
            //    desStream.Write(buffer, 0, bytes);

            //    float percentage = desStream.Length / (srcStream.Length / 100);
            //    model.Progress = percentage;
            //} while (bytes > 0);
            //}// srcStream.Close();
            // }//desStream.Close();
            //});  
            #endregion
            int a = 123;
            int f = a / 100;//1
            int m = a / 10 % 10;//2
            int e = a % 10;//3
        }
        private void Open_Sourse_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == true)
            {
                model.Source = dialog.FileName;
                //srcTextBox.Text = Source;
            }
        }

        private void Open_Dest_Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();  
            dialog.IsFolderPicker = true;
            if(  dialog.ShowDialog() == CommonFileDialogResult.Ok )
            {
                model.Destination = dialog.FileName;  
                //destTextBox.Text = Destination; 
            }
            

        }
    }


    [AddINotifyPropertyChangedInterface]
    public class ViewModel
    {
        private ObservableCollection<CopyProcessInfo> processes;
        public string Source { get; set; }
        public string Destination { get; set; }
        public double Progress { get; set; }
        public bool IsWaiting => Progress == 0;
        public ViewModel()
        {
            processes = new ObservableCollection<CopyProcessInfo>();  
        }
        public IEnumerable<CopyProcessInfo> Processes => processes;//get
        public void AddProcess(CopyProcessInfo info)
        {
            processes.Add(info);    
        }
    }
    [AddINotifyPropertyChangedInterface]
    public class CopyProcessInfo
    {
        public string FileName { get; set; }
        public double Percentage { get; set; }
        public int PercentageInt => (int)Percentage;
        public double BytesPerSecond { get; set; }
        public double MegaBytesPerSecond => Math.Round(BytesPerSecond / 1024/1024, 1);
        public CopyProcessInfo(string fileName)
        {
            this.FileName = fileName;
        }
    }
}