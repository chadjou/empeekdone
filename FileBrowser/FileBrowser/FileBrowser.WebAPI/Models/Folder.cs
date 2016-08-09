using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBrowser.WebAPI.Models
{
    public class Folder
    {
        public List<string> subFolders;

        public List<string> files;

        public string rootFolder;

        public string currentFolder;

        public bool error = false;
    }
}