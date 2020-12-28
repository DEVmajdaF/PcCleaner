using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;


namespace updaterdemo
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void update_btn_Click_1(object sender, RoutedEventArgs e)
        {


            WebClient webc = new WebClient();
            var newClient = new WebClient();
            try
            {
                System.Threading.Thread.Sleep(5000);

                //string[] files = Directory.GetFiles(@".\");
                File.Delete(@".\WpfApp1.exe");

                //foreach (string file in files)
                //{
                //    File.Delete(file);
                //}
                newClient.DownloadFile("https://docs.google.com/uc?export=download&id=1zpLOrtRYQrSATYlLV4egtOdluKdh5mmq", @"version2.zip");
                string zippath = @".\version2.zip";
                string extractPath = @".\";
                ZipFile.ExtractToDirectory(zippath, extractPath);
                File.Delete(@".\version2.zip");
                Process.Start(@".\WpfApp1.exe");
                this.Close();
            }
            catch
            {
                Process.Start(@".\WpfApp1.exe");
                this.Close();

            }

        }
    }
}
