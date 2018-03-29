using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.NET.Mentoring._01.ModuleCollectionsAndGenerics.Task2
{
    public interface IComparer<in T>
    {
        bool Compare(T lhs, object rhs);
    }
}
