using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace MediaPlayerApp
{
    public class Mp3
    {
        
        private string title = "";
        private string[] artists = {};
        private string album = "";
        private uint year = 0;

        public Mp3()
        {

        }
       

        public void setProperties(TagLib.File? currentFile)
        {
            if(currentFile != null)
            {
                title = currentFile.Tag.Title;
                artists = currentFile.Tag.AlbumArtists;
                album = currentFile.Tag.Album;
                year = currentFile.Tag.Year;
            }
            else
            {
                MessageBox.Show("Error!, current File maybe null");
            }
            
        }

        
        public string Title { get { return title; } set { title = value; } }
        public string[] Artists { get { return artists; } set { artists = value; } }
        public string Album { get { return album; } set { album = value; } }
        public uint Year { get { return year; } set { year = value; } }
    
    }
}
