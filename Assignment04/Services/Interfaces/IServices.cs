using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment04.Services.Interfaces
{
    public interface IServices<T>
    {
        List<string> LoadPlaylists();
        string Save(T playlist);
        T OpenPlaylist(string playlist);
    }
}
