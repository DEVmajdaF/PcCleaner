using System;
using System.IO;
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
using System.Windows.Threading;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Net;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        long size = 0;
      


        public MainWindow()
        {
            InitializeComponent();

            // Here i work with the last Analyse I make   
            derniere_maj.Text = derniernet();
            derniere_analyse.Text = derniernAnalyse();

        }

        string  sizecalculator(double size)
        {

            //ses valeurs se sont on octes:
            long ko = 1024;
            long mo = 1048576;
            long go = 1073741824;
            long to = 1099511627776;
            if (size<1024)
            {
                double sizes =size;
                espace_nettoyer.Text = $"Espace à nettoyer: {Math.Round(sizes, 2)} Octets";
            }
            else if(size >= ko && size < mo)
            {
                double sizes = size / 1024;
                espace_nettoyer.Text =  $"Espace à nettoyer: {Math.Round(sizes, 2)} KO";
            }
            else if (size >= mo && size < go)
            {
                double sizes = size/mo ;
                espace_nettoyer.Text = $"Espace à nettoyer: {Math.Round(sizes, 2)} MO" ;
            }
            else if (size>= go && size < to)
            {
                double sizes = size/go;
                espace_nettoyer.Text = $"Espace à nettoyer: {Math.Round(sizes, 2)} GO";
            }

            return espace_nettoyer.Text;
        }
        string derniernet()
        {
            // Here i work with the last Delete I make 
            var lastLineN = File.ReadLines("../../../Historique/nettoyer.txt").Last();
            string[] splitLineN = lastLineN.Split('"');
            string dateAnalyseN = splitLineN[1];
            DateTime strdate2 = Convert.ToDateTime(dateAnalyseN);
            DateTime dateN = DateTime.Now;

            if ((dateN - strdate2).Seconds < 60 && (dateN - strdate2).Minutes == 0 && (dateN - strdate2).Hours == 0)
            {
                derniere_maj.Text = $"Dernièr Nettoyage: {(dateN - strdate2).Seconds} Seconds Ago";
            }
            else if ((dateN - strdate2).Minutes < 60 && (dateN - strdate2).Hours == 0)
            {
                derniere_maj.Text = $"Dernièr Nettoyage: {(dateN - strdate2).Minutes} Minutes Ago";
            }
            else if ((dateN - strdate2).Hours < 24)
            {
                derniere_maj.Text = $"Dernièr Nettoyage: depuis {(dateN - strdate2).Hours} Heures  et {(dateN - strdate2).Minutes} Minutes ";
            }
            else if ((dateN - strdate2).Hours > 24)
            {
                derniere_maj.Text = $"Dernièr Nettoyage: {(dateN - strdate2).Days} days Ago";
            }
            return derniere_maj.Text;
        }


        string derniernAnalyse()
        {

            var lastLine = File.ReadLines("../../../Historique/analyse.txt").Last();
            string[] splitLine = lastLine.Split('"');
            string dateAnalyse = splitLine[1];
            DateTime strdate1 = Convert.ToDateTime(dateAnalyse);
            DateTime date = DateTime.Now;
            if ((date - strdate1).Seconds < 60 && (date - strdate1).Minutes == 0 && (date - strdate1).Hours == 0)
            {
                derniere_analyse.Text = $"Dernière Analyse: {(date - strdate1).Seconds} Seconds Ago";
            }
            else if ((date - strdate1).Minutes < 60 && (date - strdate1).Hours == 0)
            {

                derniere_analyse.Text = $"Dernière Analyse: {(date - strdate1).Minutes} Minutes Ago";
            }
            else if ((date - strdate1).Hours < 24)
            {
                derniere_analyse.Text = $"Dernière Analyse: depuis  {(date - strdate1).Hours} heures et {(date - strdate1).Minutes} minutes ";
            }
            else if ((date - strdate1).Hours > 24)
            {
                derniere_analyse.Text = $"Dernière Analyse: {(date - strdate1).Days} days Ago";

            }
            return derniere_analyse.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btn_analyser_Click_1(object sender, RoutedEventArgs e)
        {

            size = 0;
            hist_listBox.Visibility = Visibility.Hidden;
            percent.Visibility = Visibility.Visible;


            //specifier the direcory que tu veux manipuler;
            //var path = @"C:\hello";
            //

            string path = System.IO.Path.GetTempPath();
            DirectoryInfo appTemp = new DirectoryInfo(path);


            if (appTemp.Exists)
            {
                //give the number of files that exist in this directory
                decimal count = appTemp.GetFiles().Length;
                //Console.WriteLine($"the length of files: {count}"); //241
                if (count == 0)
                {
                    
                    espace_nettoyer.Visibility = Visibility.Hidden;
                    derniere_maj.Visibility = Visibility.Hidden;
                    derniere_analyse.Visibility = Visibility.Hidden;
                    progressBar.Visibility = Visibility.Hidden;
                    count0.Text = "Aucun fichier a analyser";
                    progressBar.Visibility = Visibility.Visible;
                    progressBar.Value = 0;
                    percent.Text = "";


                }
                else
                {  
                    //la fonction retourne une collection enumerable des fichiers with no search pattern 
                    foreach (var file in appTemp.EnumerateFiles("*.*", SearchOption.AllDirectories))
                    {
                        //files.Items.Add(file);
                        size += file.Length;
                        Console.WriteLine($"{file.Name} {file.Length} ");
                    }


                    //Pass the filepath and filename to the StreamWriter Constructor
                    var streamWriters = new List<StreamWriter>();
                    streamWriters.Add(new StreamWriter("../../../Historique/analyse.txt", true, Encoding.ASCII));
                    streamWriters.Add(new StreamWriter("../../../Historique/hist.txt", true, Encoding.ASCII));
                    Parallel.ForEach(streamWriters, s => { s.WriteLine($"La Derniere action est de \"{DateTime.Now}\" , et la taille du cach est de: {(size / 1024) / 1024} Mo "); s.Dispose(); s.Close(); });

                    

                    //txt_analyse.Text = "l'analyse de votre pc est en cours ...";
                    espace_nettoyer.Visibility = Visibility.Hidden;
                    derniere_analyse.Visibility = Visibility.Hidden;

                    // Display the ProgressBar control.
                    progressBar.Visibility = Visibility.Visible;
                    derniere_maj.Visibility = Visibility.Hidden;
                    decimal l = 0;


                    for (var i = 0; i < count; i++)
                    {

                        decimal text = 100 / count;
                        l = l + text;

                        decimal f = Math.Round(l, 2);

                        Console.WriteLine(f);


                        progressBar.Minimum = 0;


                        progressBar.Value = Convert.ToDouble(f);
                        progressBar.Dispatcher.Invoke(() => progressBar.Value = Convert.ToDouble(f), DispatcherPriority.Background);
                        Thread.Sleep(100);
                        percent.Text = Convert.ToString(f) + "%";

                        if (progressBar.Value == 100)
                        {

                            espace_nettoyer.Visibility = Visibility.Visible;
                            derniere_maj.Visibility = Visibility.Visible;
                            derniere_analyse.Visibility = Visibility.Visible;
                            progressBar.Visibility = Visibility.Visible;
                            derniere_maj.Text = derniernet();
                            txt_analyse.Text = "l'analyse de votre pc est terminéé...";
                            espace_nettoyer.Text =  sizecalculator(size);
                            derniere_analyse.Text = derniernAnalyse();

                            //btn_historique.IsEnabled = true;
                            //btn_maj.IsEnabled = true;
                            //btn_histo.IsEnabled = true;
                            //btn_nettoyer.IsEnabled = true;
                            //btn_vue.IsEnabled = true;
                            //btn_option.IsEnabled = true;
                            //btn_analyse_menu.IsEnabled = true;



                        }

                    }


                }


            }

     }
        


        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
        //historique 
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            histtext.Visibility = Visibility.Visible;
            count0.Text = "";
            btn_histo.Visibility= Visibility.Hidden;
            btn_maj.Visibility = Visibility.Hidden;
            btn_nettoyer.Visibility = Visibility.Hidden;
            btn_analyser.Visibility = Visibility.Hidden;
            espace_nettoyer.Visibility = Visibility.Hidden;
            derniere_maj.Visibility = Visibility.Hidden;
            derniere_analyse.Visibility = Visibility.Hidden;
            hist_listBox.Visibility = Visibility.Visible;
            progressBar.Visibility = Visibility.Hidden;
            percent.Visibility = Visibility.Hidden;

            string line;

                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader("../../../Historique/hist.txt");

                //Read the first line of text
                line = sr.ReadLine();
                hist_listBox.Items.Add(line);
                //Continue to read until you reach end of file
                while ((line = sr.ReadLine()) != null)
                    {
                        hist_listBox.Items.Add(line);
                    }
                //close the file
                sr.Close();

        }

        //btn_netoyer //delete 
        private void btn_nettoyer_Click(object sender, RoutedEventArgs e)
        {

            espace_nettoyer.Visibility = Visibility.Hidden;
            derniere_maj.Visibility = Visibility.Hidden;
            derniere_analyse.Visibility = Visibility.Hidden;

            //string path = @"C:\hello";
            //specifier the directory que tu veux manipuler 
            string path = System.IO.Path.GetTempPath();
            DirectoryInfo appTemp = new DirectoryInfo(path);
            double length = appTemp.GetFiles().Length;

           if(appTemp.Exists)
            {

                string message = "Do you want to delete the file?";
                string title = "Close Window";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxResult result = (MessageBoxResult)System.Windows.MessageBox.Show(message, title, (MessageBoxButton)buttons);
                if (result == MessageBoxResult.Yes)
                {
                    if (length == 0)
                    {

                        count0.Text = "Aucun fichier a supprimer";
                    }
                    else
                    {

                        foreach (var file in appTemp.EnumerateFiles("*.*", SearchOption.AllDirectories))
                        {
                            file.Delete();
                        }


                        foreach (DirectoryInfo dir in appTemp.GetDirectories())
                        {
                            dir.Delete(true);
                        }

                        count0.Text = "opération terminé avec succès";

                        var streamWriters = new List<StreamWriter>();
                        streamWriters.Add(new StreamWriter("../../../Historique/nettoyer.txt", true, Encoding.ASCII));
                        streamWriters.Add(new StreamWriter("../../../Historique/hist.txt", true, Encoding.ASCII));
                        Parallel.ForEach(streamWriters, s => { s.WriteLine($"La Derniere action de delete est de \"{DateTime.Now}\", et la taille du cach supprime est de: {(size / 1024) / 1024} Mo "); s.Dispose(); s.Close(); });

                    }
                   
                }
                else
                {

                }
                size = 0;

                //Pass the filepath and filename to the StreamWriter Constructor
               



            }

    }

        private void hist_listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_vue_Click(object sender, RoutedEventArgs e)
        {
            btn_analyser.Visibility= Visibility.Visible;
            histtext.Visibility = Visibility.Hidden;

            btn_histo.Visibility = Visibility.Visible;
            btn_maj.Visibility = Visibility.Visible;
            btn_nettoyer.Visibility = Visibility.Visible;     
            hist_listBox.Visibility = Visibility.Hidden;
            espace_nettoyer.Visibility = Visibility.Visible;
            derniere_maj.Visibility = Visibility.Visible;
            derniere_analyse.Visibility = Visibility.Visible;
            progressBar.Visibility = Visibility.Visible;
            progressBar.Value = 0;
            count0.Text = "";
            derniere_analyse.Text = derniernAnalyse();
            derniere_maj.Text = derniernet();
            txt_analyse.Text = "Analyse du pc necessaire";
            //espace_nettoyer.Text = sizecalculator(size);
            percent.Text = "";
            hist_listBox.Items.Clear();

        }

        private void btn_maj_Click(object sender, RoutedEventArgs e)
        {
            //Provides common methods for sending data to and receiving data from a resource identified by a URI
            WebClient webc = new WebClient();
            if (!webc.DownloadString("https://pastebin.com/WiepHdik").Contains("v1.1.0"))
            {
                if (MessageBox.Show("there is an update! Do you want to download it?", "demo app", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(@".\updaterdemo.exe");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("You Arleady make an update");
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
