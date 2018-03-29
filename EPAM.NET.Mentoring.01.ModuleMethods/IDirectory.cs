namespace EPAM.NET.Mentoring
{
    public interface IDirectory
    {
        string Name { get; set; }

        string FullName { get; set; }

        IFile[] GetFiles();

        IDirectory[] GetDirectories();
    }
}
