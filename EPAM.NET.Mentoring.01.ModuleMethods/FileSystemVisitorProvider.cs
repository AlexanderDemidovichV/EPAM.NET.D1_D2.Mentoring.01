using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EPAM.NET.Mentoring;

namespace EPAM.NET.Mentoring
{
    public class FileSystemVisitorProvider //: IFileSystemVisitorProvider
    {
        public DirectoryInfo GetDirectoryInfo(string path)
        {
            DirectoryInfo rootDirectoryInfo;
            try
            {
                rootDirectoryInfo = new DirectoryInfo(path);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Not valid path", e);
            }
            return rootDirectoryInfo;
        }

        public FileInfo[] GetFileInfo(DirectoryInfo root)
        {
            return root.GetFiles();
        }
    }
}
