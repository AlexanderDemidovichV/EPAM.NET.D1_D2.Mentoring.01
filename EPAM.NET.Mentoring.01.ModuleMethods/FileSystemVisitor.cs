using System;
using System.Collections.Generic;
using System.IO;

namespace EPAM.NET.Mentoring
{
    public class FileSystemVisitor: IFileSystemVisitor
    {
        private readonly Func<string, bool> _searchPattern = value => value.StartsWith("p");

        private readonly IFileSystemVisitorProvider _fileSystemVisitorProvider;

        public event EventHandler<FileSystemEventArgs> StartWalk;
        public event EventHandler<FileSystemEventArgs> FinishWalk;

        public event EventHandler<FileSystemEventArgs> FileFinded;
        public event EventHandler<FileSystemEventArgs> DirectoryFinded;

        public event EventHandler<FileSystemEventArgs> FilteredFileFinded;
        public event EventHandler<FileSystemEventArgs> FilteredDirectoryFinded;

        public bool Cancel { get; set; }

        public bool SkipNext { get; set; }


        public System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();

        public FileSystemVisitor(IFileSystemVisitorProvider fileSystemVisitorProvider, Func<string, bool> searchPattern)
        {
            _fileSystemVisitorProvider = fileSystemVisitorProvider;
            _searchPattern = searchPattern;
        }

        public IEnumerable<string> Visit(string root)
        {
            OnStartWalk(new FileSystemEventArgs("start"));

            IDirectory rootDirectoryInfo = _fileSystemVisitorProvider.GetDirectoryInfo(root);

            var result = new List<string>();

            foreach (string value in WalkDirectoryTree(rootDirectoryInfo))
            {
                result.Add(value);
            }

            OnFinishWalk(new FileSystemEventArgs("finish"));
            return result;
        }

        protected virtual void OnStartWalk(FileSystemEventArgs args)
        {
            StartWalk?.Invoke(this, args);
        }

        protected virtual void OnFinishWalk(FileSystemEventArgs args)
        {
            FinishWalk?.Invoke(this, args);
        }

        protected virtual void OnFilteredFileFinded(FileSystemEventArgs args)
        {
            FilteredFileFinded?.Invoke(this, args);
        }

        protected virtual void OnFilteredDirectoryFinded(FileSystemEventArgs args)
        {
            FilteredDirectoryFinded?.Invoke(this, args);
        }

        protected virtual void OnFileFinded(FileSystemEventArgs args)
        {
            FileFinded?.Invoke(this, args);
        }

        protected virtual void OnDirectoryFinded(FileSystemEventArgs args)
        {
            DirectoryFinded?.Invoke(this, args);
        }

        private IEnumerable<string> WalkDirectoryTree(IDirectory root)
        {
            IFile[] files = null;

            OnDirectoryFinded(new FileSystemEventArgs(root?.Name));

            if (Cancel)
                yield break;

            if (_searchPattern(root?.Name) && !SkipNext)
            {
                OnFilteredDirectoryFinded(new FileSystemEventArgs(root.Name));

                if (Cancel)
                    yield break;

                if (!SkipNext) {
                    yield return root.FullName;

                    try
                    {
                        files = _fileSystemVisitorProvider.GetFileInfo(root);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        log.Add(e.Message);
                    }

                    catch (DirectoryNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    if (files != null)
                    {
                        foreach (IFile fi in files)
                        {
                            OnFileFinded(new FileSystemEventArgs(fi?.Name));

                            if (Cancel)
                                yield break;

                            if (_searchPattern(fi?.Name) && !SkipNext)
                            {
                                OnFilteredFileFinded(new FileSystemEventArgs(fi?.Name));

                                if (Cancel)
                                    yield break;

                                if (!SkipNext)
                                {
                                    yield return fi?.FullName;
                                }
                                else
                                {
                                    SkipNext = false;
                                }
                            }
                            SkipNext = false;
                        }

                        var subDirs = root.GetDirectories();

                        foreach (IDirectory dirInfo in subDirs)
                        {
                            foreach (var value in WalkDirectoryTree(dirInfo))
                            {
                                yield return value;
                            }
                        }
                    }

                }
                else {
                    SkipNext = false;
                }
            }
            SkipNext = false;
        }
    }
}
