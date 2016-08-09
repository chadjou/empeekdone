using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using FileBrowser.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Cors;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Web.Script.Serialization;

namespace FileBrowser.WebAPI.FolderInfo
{

    public static class readingFilesFromDisk
    {


        public static IEnumerable<FileInfo> getFiles(DirectoryInfo directory, string pattern = "*.*")
        {
            try
            {
                return directory.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<FileInfo>();
            }
        }

        public static IEnumerable<DirectoryInfo> getSubdirectories(DirectoryInfo directory)
        {
            try
            {
                return directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<DirectoryInfo>();
            }
        }
    }

    //return app's codebase 
    public static class AssemblyFolder
    {
        public static string GetAssemblyFolder()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string assemblypath = Uri.UnescapeDataString(uri.Path);
            assemblypath = Directory.GetParent(assemblypath).FullName;
            assemblypath = Directory.GetParent(assemblypath).FullName;
            assemblypath = Directory.GetParent(assemblypath).FullName; //need to refactor
            return assemblypath;

        }
    }




    //return all accessible files from the path and subfolders 
    public static class FilesStatistic
    {

        public static List<FileInfo> GetAllAccessibleFiles2(string rootPath, List<FileInfo> alreadyFound = null)
        {
            if (alreadyFound == null)
                alreadyFound = new List<FileInfo>();

            DirectoryInfo di = new DirectoryInfo(rootPath);

            var dirs = readingFilesFromDisk.getSubdirectories(di);
            foreach (DirectoryInfo dir in dirs)
            {
                if (!((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden))
                {
                    string test = dir.FullName;
                    alreadyFound = GetAllAccessibleFiles2(test, alreadyFound);
                }
            }

            var directory = new DirectoryInfo(rootPath);
            var fls = readingFilesFromDisk.getFiles(directory);
            foreach (FileInfo s in fls)
            {
                alreadyFound.Add(s);
            }

            return alreadyFound;
        }

    }


}