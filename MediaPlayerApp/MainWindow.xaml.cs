using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
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
//using static System.Net.Mime.MediaTypeNames;


namespace MediaPlayerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Mp3 mp = new Mp3();
       TagLib.File? currentFile;
        string fileName="";
        bool flag = false;
        bool saving = false;
        
        public MainWindow()
        {
            InitializeComponent();
            EditPanel.Visibility = Visibility.Hidden;
            pause.IsEnabled = false;
            stop.IsEnabled = false;
        }


        private void oponFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                currentFile = TagLib.File.Create(openFile.FileName);
                fileName = openFile.FileName;
                mp.setProperties(currentFile);
                flag = true;
            }

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

        private void NowPlaying_Click(object sender, RoutedEventArgs e)
        {

            brash.ImageSource = new BitmapImage(new Uri("C:\\Users\\abelg\\source\\repos\\MediaPlayerApp\\MediaPlayerApp\\images\\nowPlaying.jpg", UriKind.Absolute));
            brash.Stretch = Stretch.Uniform;

            // Freeze the brush (make it unmodifiable) for performance benefits.
            brash.Freeze();

            // Create a rectangle and paint it with the ImageBrush.
            Rectangle rectangle1 = new Rectangle();
            rectangle1.Width = 300;
            rectangle1.Height = 150;
            rectangle1.Stroke = Brushes.MediumBlue;
            rectangle1.StrokeThickness = 1.0;
            rectangle1.Fill = brash;
            NowPlaying.IsEnabled = false;
        }

 

        private void EditTag_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                EditPanel.Visibility = Visibility.Visible;
                EditPanel.tag_title.Text = mp.Title;
                string[] arr = EditPanel.tag_artist.Text.Split(" ");
                string strA = string.Join(" ", arr);
                string strB = string.Join(" ", mp.Artists);


                bool result = strA.Trim().Equals(strB.Trim());
               // MessageBox.Show(result.ToString());
                if (result == false)
               {
                    foreach (string artist in mp.Artists)
                    {
                        EditPanel.tag_artist.Text += artist + " ";
                    }
                }

                EditPanel.tag_album.Text = mp.Album;
                EditPanel.tag_year.Text = mp.Year.ToString();
                EditTag.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Please select a file first!");
                
            }
         
           
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                currentFile = TagLib.File.Create(openFile.FileName);
                fileName = openFile.FileName;
                mp.setProperties(currentFile);
                flag = true;

            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
           // if (flag)
           // {
                if (mediaPlayer.Source == null)
                {
                    mediaPlayer.Source = new Uri(fileName);
                }
      
            mediaPlayer.Play();
           


            pause.IsEnabled = true;
            stop.IsEnabled = true;
           // }
          //  else
           // {
              //  MessageBox.Show("Please select song first!");
            //}
          
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            
            mediaPlayer.Pause();
       
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            mediaPlayer.Close();

        }

        public void setTags() {
           
            if(currentFile != null)
            {
                mediaPlayer.Close();
                currentFile.Tag.Title = EditPanel.tag_title.Text;
                string[] artistArr = EditPanel.tag_artist.Text.Split("");
                currentFile.Tag.AlbumArtists = artistArr;
                currentFile.Tag.Album = EditPanel.tag_album.Text;
                currentFile.Tag.Year = uint.Parse(EditPanel.tag_year.Text);
                currentFile.Save();
                mp.setProperties(currentFile);
                EditTag.IsEnabled = true;
                tagCarrent.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Error, file not exist!");
            }

            EditPanel.Visibility = Visibility.Hidden;
         

        }

        private void Tag_Current_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                EditPanel.Visibility = Visibility.Visible;
                EditPanel.tag_title.Text = mp.Title;
                string[] arr = EditPanel.tag_artist.Text.Split(" ");
                string strA = string.Join(" ", arr);
                string strB = string.Join(" ", mp.Artists);


                bool result = strA.Trim().Equals(strB.Trim());
                // MessageBox.Show(result.ToString());
                if (result == false)
                {
                    foreach (string artist in mp.Artists)
                    {
                        EditPanel.tag_artist.Text += artist + " ";
                    }
                }

                EditPanel.tag_album.Text = mp.Album;
                EditPanel.tag_year.Text = mp.Year.ToString();
                tagCarrent.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Please select a file first!");

            }
            
            
        }
    }
}
