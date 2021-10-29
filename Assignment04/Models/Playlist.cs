using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment04.Models
{
    public class Playlist
    {
       
        public string FullPath { get; set; } 
        public string PlaylistName { 
            get
            {
                string name = Path.GetFileNameWithoutExtension(FullPath);
                return name;
            }
        }
        public Playlist()
        { 

        }
        public Playlist(string fullPath)
        {
            this.FullPath = fullPath;
        }

        public override string ToString()
        {
            return $"{FullPath}\n" +
                $"{PlaylistName}";
        }
    }
}
