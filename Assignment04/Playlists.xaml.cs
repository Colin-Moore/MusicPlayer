using Assignment04.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml.Serialization;
using Assignment04.Services;
using Microsoft.Win32;
using Id3;

namespace Assignment04
{
    /// <summary>
    /// Interaction logic for Playlists.xaml
    /// </summary>
    public partial class Playlists : Window
    {
        public List<Song> library = new List<Song>();
        public List<Song> playlist = new List<Song>();
        PlaylistService songs = new PlaylistService();
        List<string> playlists = new List<string>();
        public Playlists()
        {
            InitializeComponent();
            
            playlists = songs.LoadPlaylists();

            foreach (string playlist in playlists)
            {
                lbxPlaylistName.Items.Add(playlist);
            }

            library = songs.OpenLibrary();

            foreach (Song song in library)
            {
                dgLibrarySongs.Items.Add(song);
            }

            btnAddNew.Click += AddToLibrary;
            btnAddToPlaylist.Click += AddToPlaylist;
            btnSavePlaylist.Click += SavePlaylist;
            btnRemoveFromPlaylist.Click += RemoveFromPlaylist;
            lbxPlaylistName.MouseDoubleClick += ChangePlayList;
            btnNewPlaylist.Click += NewPlaylist;
            dgLibrarySongs.MouseDoubleClick += AddToPlaylist;
            dgPlaylistSongs.MouseDoubleClick += Error;
        }


        public void Error(object sender, EventArgs e)
        {
            MessageBox.Show("Can't play from here!");
        }
        public void NewPlaylist(object sender, EventArgs e)
        {
            dgPlaylistSongs.Items.Clear();
        }

        public void AddToLibrary(object sender, EventArgs e)
        {

            List<Song> songsToAdd = new List<Song>();
            songsToAdd = songs.AddNewSong(library);
            foreach (Song song in songsToAdd)
            {
                dgLibrarySongs.Items.Add(song);
            }
        }

        public void AddToPlaylist(object sender, EventArgs e)
        {
            if (dgPlaylistSongs.Items.Contains(dgLibrarySongs.SelectedItem))
            {
                MessageBox.Show("Song already exists on playlist!");
            }
            else
            {
                dgPlaylistSongs.Items.Add(dgLibrarySongs.SelectedItem);
                playlist.Add((Song)dgLibrarySongs.SelectedItem);
            }
        }

        public void SavePlaylist(object sender, EventArgs e)
        {
            if (dgPlaylistSongs.Items.Count == 0)
            {
                MessageBox.Show("Nothing to save!");
            }
            else
            {
                var playlistService = new PlaylistService();

                string newList = playlistService.Save(playlist);
                string listName = System.IO.Path.GetFileNameWithoutExtension(newList);

                lbxPlaylistName.Items.Add(listName);

                (Application.Current.MainWindow as MainWindow).lbPlaylists.Items.Add(listName);
                if (newList == null)
                {
                    lbxPlaylistName.Items.Remove(listName);
                    (Application.Current.MainWindow as MainWindow).lbPlaylists.Items.Remove(listName);
                }
            }
        }

        public void RemoveFromPlaylist(object sender, EventArgs e)
        {
            if(dgPlaylistSongs.Items.Count == 0)
            {
                MessageBox.Show("Nothing to remove!");
            }

            dgPlaylistSongs.Items.Remove(dgPlaylistSongs.SelectedItem);
        }

        public void ChangePlayList(object sender, EventArgs e)
        {
            dgPlaylistSongs.Items.Clear();

            playlist = songs.OpenPlaylist(lbxPlaylistName.SelectedItem.ToString());

            foreach (Song song in playlist)
            {
                dgPlaylistSongs.Items.Add(song);
            }
        }
    }
}