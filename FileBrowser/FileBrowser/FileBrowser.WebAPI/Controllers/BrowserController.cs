using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using FileBrowser.WebAPI.Models;
using System.Web.Cors;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Web.Script.Serialization;
using FileBrowser.WebAPI.FolderInfo;

using System.Web.Http.Cors;

namespace FileBrowser.WebAPI.Controllers
{
  //  [EnableCorsAttribute("http://localhost:64917", "*", "*")]
    [EnableCorsAttribute("*", "*", "*")]
    public class BrowserController : ApiController
    {


        //return file's count from path and subfolders
        [HttpGet]
        [Route("api/statistic")]
        public Statistic Statistic([FromUri] string path)
        {
            {
                path = Uri.UnescapeDataString(path);
                List<FileInfo> filesinfo = new List<FileInfo>();

                //in case when we get root of list of drivers
                if (path == "drivesFolder")
                {
                    var drives = System.IO.DriveInfo.GetDrives().Select(f => f.Name).ToList();
                    Statistic statisticData = new Statistic();
                    foreach (var drive in drives) {
                        try
                        {
                            filesinfo = FileBrowser.WebAPI.FolderInfo.FilesStatistic.GetAllAccessibleFiles2(drive);
                        }
                        catch (Exception ex)
                        { }
                      
                        foreach (var t in filesinfo)
                        {
                            if (t.Length > 1000)
                            { statisticData.bigSize++; }
                            else if (t.Length < 1000 && t.Length > 300)
                            { statisticData.middleSize++; }
                            else
                            { statisticData.smallSize++; }
                        }
                        }

                    statisticData.currentFolder = path;
                    return statisticData;
                }
                else
                {
                    //return count from path and subfolers
                    DirectoryInfo d = new DirectoryInfo(path);
                    if (d.Exists == false)
                    { path = FolderInfo.AssemblyFolder.GetAssemblyFolder(); }

                    filesinfo = FileBrowser.WebAPI.FolderInfo.FilesStatistic.GetAllAccessibleFiles2(path);

                    Statistic statisticData = new Statistic();

                    foreach (var t in filesinfo)
                    {
                        var mb = (t.Length / 1024) / 1024;

                        if (mb >= 100)
                        { statisticData.bigSize++; }
                        else if (mb < 100 && 10 > 300)
                        { statisticData.middleSize++; }
                        else
                        { statisticData.smallSize++; }
                    }

                    
                    statisticData.currentFolder = path;
                    return statisticData;
                }
            }
        }

        //return list of subfolders and list of files from current path
        [HttpGet]
        [Route("api/folderdata")]
        public Folder FolderData([FromUri] string path)
        {
            {
                path = Uri.UnescapeDataString(path);
                Folder currentFolderData = new Folder();

                //in case of root
                if (path == "drivesFolder")
                {
                    var drives = System.IO.DriveInfo.GetDrives().Select(f => f.Name).ToList();
                    currentFolderData.subFolders = drives;
                    currentFolderData.currentFolder = path;
                    currentFolderData.rootFolder = Path.GetDirectoryName(path);
                    return currentFolderData;
                }

                else
                {
                    DirectoryInfo d = new DirectoryInfo(path);

                    if (d.Exists == false && path != "onload")
                    {
                        currentFolderData.currentFolder = path;
                        currentFolderData.error = true;
                        return currentFolderData;
                    }
                    else { 

                    if (d.Exists == false)
                    {
                        path = FolderInfo.AssemblyFolder.GetAssemblyFolder();
                    }

                 

                    if (d.Parent == null)
                    { currentFolderData.rootFolder = "drivesFolder"; }
                    else
                    {
                        try
                        {
                            currentFolderData.rootFolder = Directory.GetParent(path).FullName;
                        }
                        catch (Exception ex)
                        { currentFolderData.rootFolder = path; }
                    }
                    currentFolderData.currentFolder = path;
                    DirectoryInfo di = new DirectoryInfo(path);
                    currentFolderData.subFolders = FolderInfo.readingFilesFromDisk.getSubdirectories(di).Select(f => f.FullName).ToList();
                    currentFolderData.files = FolderInfo.readingFilesFromDisk.getFiles(di).Select(f => f.FullName).ToList();

                    return currentFolderData;
                    }
                }
            }
        }
    }
}
