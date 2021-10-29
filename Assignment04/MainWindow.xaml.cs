using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml.Serialization;
using Assignment04.Models;
using Assignment04.Services;

namespace Assignment04
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Song lastSong;
        MediaPlayer musicPlayer = new MediaPlayer();
        PlaylistService songs = new PlaylistService();
        Queue<Song> songQueue = new Queue<Song>();
        List<Song> librarySongs = new List<Song>();
        List<string> playlists = new List<string>();
        
        public bool isPlaying = false;
        public MainWindow()
        {
            InitializeComponent();

            librarySongs = songs.OpenLibrary();
            foreach (Song song in librarySongs)
            {
                dgSongList.Items.Add(song);
            }

            playlists = songs.LoadPlaylists();
            foreach (string playlist in playlists)
            {
                lbPlaylists.Items.Add(playlist);
            }

            musicPlayer.MediaEnded += NextSong;

            btnAddEditPlaylists.Click += OpenPlaylistWindow;
            dgSongList.MouseDoubleClick += AddToQueue;
            dgPlayQueue.MouseDoubleClick += PlayQueue;
            btnPlay.Click += PlayButton;
            btnStop.Click += StopButton;
            btnNext.Click += NextButton;
            btnPrevious.Click += PreviousButton;
            lbPlaylists.MouseDoubleClick += OpenPlaylist;
            btnClearQueue.Click += ClearQueue;
            btnAddToQueue.Click += AddToQueue;
            btnRemoveFromQueue.Click += RemoveFromQueue;
        }

        public void PreviousButton(object sender, EventArgs e)
        {
            if (songQueue.Count < 2)
            {
                musicPlayer.Play();
            }
            else
            {
                Song lastSong = songQueue.Last();

                Song previousSong = songQueue.Dequeue();

                songQueue.Prepend(lastSong);
                songQueue.Enqueue(previousSong);

                musicPlayer.Stop();
                musicPlayer.Open(new Uri(lastSong.Filename));
                musicPlayer.Play();
                txtStatus.Text = $"Now Playing: {lastSong.Title} - {lastSong.Artist}";
                isPlaying = true;

                dgPlayQueue.Items.Clear();
                foreach (Song song in songQueue)
                {
                    dgPlayQueue.Items.Add(song);
                }
            }
            isPlaying = true;
        }
        public void NextButton(object sender, EventArgs e)
        {
            if (songQueue.Count < 2)
            {
                musicPlayer.Play();
            }
            else
            {
                Song previousSong = songQueue.Dequeue();
                Song nextSong = songQueue.Peek();
                songQueue.Enqueue(previousSong);
                musicPlayer.Open(new Uri(nextSong.Filename));
                musicPlayer.Play();
                txtStatus.Text = $"Now Playing: {nextSong.Title} - {nextSong.Artist}";
                dgPlayQueue.Items.Clear();

                foreach (Song song in songQueue)
                {
                    dgPlayQueue.Items.Add(song);
                }
            }
            isPlaying = true;
        }
   
        public void NextSong(object sender, EventArgs e)
        {
            if (songQueue.Count < 2)
            {
                musicPlayer.Stop();
                isPlaying = false;
                btnStop.IsEnabled = false;
                btnPlay.Content = "Play";
            }
            else
            {
                lastSong = songQueue.Dequeue();
                songQueue.Append(lastSong);
                dgPlayQueue.Items.Clear();

                foreach (Song song in songQueue)
                {
                    dgPlayQueue.Items.Add(song);
                }
            }
        }
        public void PlaySong(object sender, EventArgs e)
        {
            dgPlayQueue.Items.Add(dgSongList.SelectedItem);
            Song song = songQueue.Peek();
            songQueue.Enqueue(song);
            musicPlayer.Open(new Uri(song.Filename));
            musicPlayer.Play();
            txtStatus.Text = $"Now Playing: {song.Title} - {song.Artist}";
            btnStop.IsEnabled = true;
            isPlaying = true;
        }
        public void PlayQueue(object sender, EventArgs e)
        {
            if(dgPlayQueue.SelectedItem == null)
            {
                return;
            }
            Song song = (Song)dgPlayQueue.SelectedItem;
            lastSong = song;
            if (isPlaying == true)
            {
                musicPlayer.Stop();
            }
            musicPlayer.Open(new Uri(song.Filename));
            musicPlayer.Play();
            txtStatus.Text = $"Now Playing: {song.Title} - {song.Artist}";

            isPlaying = true;
            btnPlay.Content = "Pause";
            btnStop.IsEnabled = true;

        }
        public void ClearQueue(object sender, EventArgs e)
        {
            musicPlayer.Stop();
            dgPlayQueue.Items.Clear();
            songQueue.Clear();
        }
        public void AddToQueue(object sender, EventArgs e)
        {
            try
            {
                dgPlayQueue.Items.Add(dgSongList.SelectedItem);
                songQueue.Enqueue((Song)dgSongList.SelectedItem);
            
            if(isPlaying == true)
            {
                musicPlayer.Stop();
            }
            lastSong = (Song)dgSongList.SelectedItem;
      
            musicPlayer.Open(new Uri(lastSong.Filename));
            musicPlayer.Play();
            btnPlay.Content = "Pause";
            btnStop.IsEnabled = true;
            txtStatus.Text = $"Now Playing: {lastSong.Title} - {lastSong.Artist}";

            isPlaying = true;
            }
            catch(Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }

        public void RemoveFromQueue(object sender, EventArgs e)
        {
            if (dgPlayQueue.SelectedItem == null)
            {
                MessageBox.Show("Select a song to remove!");
            }
            else if(songQueue.Count == 1)
            {
                dgPlayQueue.Items.Clear();
                songQueue.Clear();
                musicPlayer.Stop();
                btnStop.IsEnabled = false;
                btnPlay.Content = "Play";
                isPlaying = false;
                txtStatus.Text = "Stopped";
            }
            else
            {
                //Song songToRemove = songQueue.ElementAt(dgPlayQueue.SelectedIndex);
                //songQueue = new Queue<Song>(songQueue.Where(x => x != songToRemove));
                dgPlayQueue.Items.Remove(dgPlayQueue.SelectedItem);

            }
        }
        public void OpenPlaylist(object sender, EventArgs e)
        {
            if (lbPlaylists.SelectedItem == lbiLibrary)
            {
                dgPlayQueue.Items.Clear();
                dgSongList.Items.Clear();
                librarySongs = songs.OpenLibrary();
                foreach (Song song in librarySongs)
                {
                    dgSongList.Items.Add(song);
                    songQueue.Enqueue(song);
                }
                foreach(Song song in songQueue)
                {
                    dgPlayQueue.Items.Add(song);
                }
            }
            else
            {
                dgPlayQueue.Items.Clear();
                dgSongList.Items.Clear();
                librarySongs = songs.OpenPlaylist(lbPlaylists.SelectedItem.ToString());
                foreach (Song song in librarySongs)
                {
                    dgSongList.Items.Add(song);
                    dgPlayQueue.Items.Add(song);
                }
            }
        }
       
        public void Play(object sender, EventArgs e)
        {
            if(isPlaying == true)
            {
                musicPlayer.Stop();
            }
            musicPlayer.Open(new Uri(songQueue.Peek().Filename));
            musicPlayer.Play();
            isPlaying = true;
            btnStop.IsEnabled = true;
            btnPlay.Content = "Pause";
        }
        
        public void PlayButton(object sender, EventArgs e)
        {
            if(songQueue.Count == 0)
            {
                btnStop.IsEnabled = false;
                return;
            }
            else if (isPlaying == true)
            {
                musicPlayer.Pause();
                btnPlay.Content = "Play";
                isPlaying = false;
            }
            else
            {
                musicPlayer.Play();
                btnPlay.Content = "Pause";
                isPlaying = true;
            }
   
            btnStop.IsEnabled = true;
        }

        public void OpenPlaylistWindow(object sender, EventArgs e)
        {
            Playlists window = new Playlists();
            window.Show();
        }

        public void StopButton(object sender, EventArgs e)
        {
            musicPlayer.Stop();
            isPlaying = false;
            txtStatus.Text = "Stopped";
            btnPlay.Content = "Play";
        }

        private void sldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            musicPlayer.Volume = sldVolume.Value;
        }
    }
}
