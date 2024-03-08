using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
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
        public string Source { get; set; }
        public string Destination { get; set; }
        public float Progress { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            srcTextBox.Text = Source= "C:\\Users\\helen\\Desktop\\title_homework\\1.png";
            destTextBox.Text = Destination = "C:\\Users\\helen\\Desktop\\test";
            this.DataContext = Progress;

        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            //copy file from sourse to destination
            string fileName = System.IO.Path.GetFileName(srcTextBox.Text);
            //C:\\Users\\helen\\Desktop\\test\\1.png           
            string destFilePath = System.IO.Path.Combine(Destination, fileName);
            await CopyFileAsync(Source, destFilePath);
            MessageBox.Show("Complete!!!");
        }
        private Task CopyFileAsync(string src, string dest)
        {
            return Task.Run(() =>
            {
                //1 - using File Class
                //File.Copy(Source, destFilePath, true);
                //2 - FileStream
                using FileStream srcStream = new FileStream(src, FileMode.Open, FileAccess.Read);
                using FileStream desStream = new FileStream(dest, FileMode.Create, FileAccess.Write);
                byte[] buffer = new byte[1024 * 8];//8Kb
                int bytes = 0;
                do
                {
                    bytes = srcStream.Read(buffer, 0, buffer.Length);
                    desStream.Write(buffer, 0, bytes);

                    float percentage = srcStream.Length / desStream.Length * 100;
                    Progress = percentage;
                } while (bytes > 0);
                //}// srcStream.Close();
                // }//desStream.Close();
            });           
        }
        private void Open_Sourse_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == true)
            {
                Source = dialog.FileName;
                srcTextBox.Text = Source;
            }
        }

        private void Open_Dest_Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();  
            dialog.IsFolderPicker = true;
            if(  dialog.ShowDialog() == CommonFileDialogResult.Ok )
            {
                Destination = dialog.FileName;  
                destTextBox.Text = Destination; 
            }
            

        }
    }
}