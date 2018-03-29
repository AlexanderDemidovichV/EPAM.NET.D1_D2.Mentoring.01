using System.Collections;
using System.Collections.Generic;

namespace EPAM.NET.Mentoring._01.ModuleCollectionsAndGenerics.Task2
{
    class GenericCollection<T>: IEnumerable<T>
    {
        public IEnumerable<T> List { get; }

        public GenericCollection(IEnumerable<T> list)
        {
            this.List = list;
        }

        public IEnumerable<bool> Compare(GenericCollection<T> collection, object o, IComparer<T> comparer)
        {
            foreach (var value in collection)
            {
                yield return comparer.Compare(value, o);
                //yield return value.Equals(o);
            }
        }

        public IEnumerable<bool> Compare(GenericCollection<T> collection, object o, Comparison<T> comparison)
        {
            foreach (var value in collection)
            {
                yield return new ComparisonAdaptor<T>(comparison).Compare(value, o);
                //yield return value.Equals(o);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
