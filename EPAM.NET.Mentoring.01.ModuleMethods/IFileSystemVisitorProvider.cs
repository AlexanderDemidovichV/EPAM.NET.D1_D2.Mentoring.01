namespace EPAM.NET.Mentoring
{
    public interface IFileSystemVisitorProvider
    {
        IDirectory GetDirectoryInfo(string path);

        IFile[] GetFileInfo(IDirectory root);
    }
}
