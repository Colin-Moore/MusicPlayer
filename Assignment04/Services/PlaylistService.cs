using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Assignment04.Models;
using System.Xml.Serialization;
using Assignment04.Services.Interfaces;
using System.Windows;
using System.Net;
using Id3;
using System.Xml;
using System.Xml.Linq;

namespace Assignment04.Services
{

    public class PlaylistService : IServices<List<Song>>
    {
        List<Playlist> playlistLocations = new List<Playlist>();
        List<string> playlists = new List<string>();
        Playlist newPlaylist = new Playlist();
        List<Song> librarySongs = new List<Song>();

        public List<string> LoadPlaylists()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            String mainDirectory = Directory.GetCurrentDirectory();
            var ext = new List<string> { "xml" };

            var fileList = Directory
                .EnumerateFiles(mainDirectory + "\\playlist\\", "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => ext.Contains(System.IO.Path.GetExtension(s).TrimStart('.').ToLowerInvariant()))
                .Select(y => Path.GetFileNameWithoutExtension(y));

            foreach (var item in fileList)
            {
                playlists.Add(item);
            }
            if (playlists.Count == 0)
            {
                playlists.Add("No Playlists");
            }
            foreach (string list in playlists)
            {
                Path.GetFileName(list);
            }

            return playlists;
        }

        public List<Song> NewPlaylist()
        {
            List<Song> emptyPlaylist = new List<Song>();
            return emptyPlaylist;
        }

        public List<Song> OpenPlaylist(string playlist)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            String mainDirectory = Directory.GetCurrentDirectory();

            string filename = mainDirectory + "\\playlist\\" + playlist + ".xml";

            var serializer = new XmlSerializer(typeof(List<Song>));
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                librarySongs = (List<Song>)serializer.Deserialize(stream);
            }

            return librarySongs;
        }

        public List<Song> OpenLibrary()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            String mainDirectory = Directory.GetCurrentDirectory();

            string filename = mainDirectory + "\\" + "library.xml";

            var serializer = new XmlSerializer(typeof(List<Song>));
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                librarySongs = (List<Song>)serializer.Deserialize(stream);
            }

            return librarySongs;
        }

        public string Save(List<Song> playlist)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            String mainDirectory = Directory.GetCurrentDirectory();

            SaveListWindow saveListWindow = new SaveListWindow();
            string playlistName = null;

            if (saveListWindow.ShowDialog() == true)
            {
                playlistName = saveListWindow.txtPlaylistName.Text;

                if (playlistName.Length == 0)
                {
                    MessageBox.Show("Name cannot be blank!");
                    return null;
                }

                string playlistFileLocation = mainDirectory + "\\playlist\\" + playlistName + ".xml";

                if (File.Exists(playlistFileLocation))
                {
                    File.Delete(playlistFileLocation);
                }

                var serializer = new XmlSerializer(typeof(List<Song>));
                using (var stream = new FileStream(playlistFileLocation, FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(stream, playlist);
                }

                MessageBox.Show("Playlist Saved");

                return playlistFileLocation;
            }

            return null;

        }
        public List<Song> AddNewSong(List<Song> library)
        {

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            String mainDirectory = Directory.GetCurrentDirectory();

            string libraryFile = mainDirectory + "\\library.xml";


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    using (var mp3 = new Mp3(filename))
                    {
                        Song song = new Song();
                        Id3Tag tag = mp3.GetTag(Id3TagFamily.Version2X);
                        song.Title = tag.Title;
                        song.Artist = tag.Artists;
                        song.Album = tag.Album;
                        song.Genre = tag.Genre;
                        song.Filename = filename;

                        library.Add(song);
                    }
                }
            }

            var serializer = new XmlSerializer(typeof(List<Song>));

            using (var stream = new FileStream(libraryFile, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(stream, library);
            }

            return library;
        }

    }
}
