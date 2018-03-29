using System.Collections.Generic;

namespace EPAM.NET.Mentoring
{
    public interface IFileSystemVisitor
    {
        IEnumerable<string> Visit(string root);
    }
}
