using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
using VisioForge.Libs.NAudio.Wave;
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
        private DispatcherTimer timer;
        private int progress;
        string fileName="";
        bool flag = false;
        bool saving = false;
        bool isPlaying = false;
        bool isPaused =  false;
        private Mp3FileReader audioFile;

        public MainWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
            EditPanel.Visibility = Visibility.Hidden;
            pause.IsEnabled = false;
            stop.IsEnabled = false;
            play.IsEnabled = false;
           


            // Create and configure the DispatcherTimer control
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Update every 1 second
            timer.Tick += Timer_Tick;
        }

        private void CenterWindowOnScreen()
        {
            //center the window 
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the progress bar value
            progressBar.Value = progress;

            // Check if the progress has reached the maximum value
            if (progress >= progressBar.Maximum)
            {
                // Stop the timer when the progress reaches the maximum
                timer.Stop();
                MessageBox.Show("Music playback completed!");
            }
            else
            {
                // Increment the progress value
                progress++;
            }
        }

        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                currentFile = TagLib.File.Create(openFile.FileName);
                fileName = openFile.FileName;

                audioFile = new Mp3FileReader(fileName);
                // Set the maximum value of the progress bar based on the duration of the audio file
                progressBar.Maximum = (int)audioFile.TotalTime.TotalSeconds;

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

            brash.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/nowPlaying.jpg", UriKind.Absolute));
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

 
        //
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
          

           

            if (flag == true)
            {
                if (mediaPlayer.Source == null)
                {
                    mediaPlayer.Source = new Uri(fileName);
                }

                if (!isPlaying)
                {
                    if (isPaused)
                    {
                        // Start playing the music
                        mediaPlayer.Play();
                        timer.Start();
                        isPlaying = true;
                        isPaused = false;

                    }
                    else
                    {
                        // Start playing the music
                        mediaPlayer.Play();

                        // Reset the progress bar and start the timer
                        progress = (int)progressBar.Minimum;
                        timer.Start();
                        isPlaying = true;
                    }


                }
            }
            else
            {
                // Calculate the position to center the MessageBox
                double windowLeft = Left + (Width - ActualWidth) / 2; 
                double windowTop = Top + (Height - ActualHeight) / 2;

                MessageBox.Show("Please select a song first!");
            }

        

            pause.IsEnabled = true;
            stop.IsEnabled = true;

        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                // Pause the music
                mediaPlayer.Pause();
                progress = (int)progressBar.Value;
                // Stop the timer
                timer.Stop();
                isPlaying = false;
                isPaused = true;
            }
         
       
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                
                mediaPlayer.Stop();
                mediaPlayer.Close();
                // Reset the progress bar and stop the timer
                progress = (int)progressBar.Minimum;
                timer.Stop();
                isPlaying = false;
            }
           

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
