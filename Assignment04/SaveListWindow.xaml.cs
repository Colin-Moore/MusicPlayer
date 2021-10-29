using Assignment04.Models;
using Assignment04.Services;
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
using System.Windows.Shapes;

namespace Assignment04
{
    /// <summary>
    /// Interaction logic for SaveListWindow.xaml
    /// </summary>
    public partial class SaveListWindow : Window
    {
        public SaveListWindow()
        {
            InitializeComponent();

            btnCancel.Click += CancelClick;
            btnSave.Click += SaveClick;
        }

        
        public string playlistName 
        {
            get { return txtPlaylistName.Text; }
            set { txtPlaylistName.Text = value; }
        }

        private void SaveClick(object sender, EventArgs e)
        {
            DialogResult = true;
        }
        public void CancelClick(object sender, EventArgs e)
        {
            DialogResult = false;
        }
    }
}
