using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderedJobs
{ 
    public class RelationshipComparer<T>
        : IComparer<T>
    {
        private readonly IComparer<T> _comparer;
        private readonly IDictionary<T, ISet<T>> _relationships;

        public RelationshipComparer()
            : this(Comparer<T>.Default)
        { }

        public RelationshipComparer(IComparer<T> comparer)
        {
            _comparer = comparer;
            _relationships = new SortedDictionary<T, ISet<T>>(comparer);
        }

        public bool Add(T ancestor, T descendant)
        {
            if (_comparer.Compare(ancestor, descendant) == 0)
                throw new ArgumentException("Not possible to be descendant to self!", nameof(descendant));

            if (IsDecendent(descendant, ancestor))
                throw new ArgumentException("This would cause an circular reference!", nameof(descendant));

            if (_relationships.TryGetValue(ancestor, out var descendants))
            {
                return descendants.Add(descendant);
            }

            _relationships.Add(ancestor, new HashSet<T>() { descendant });

            return true;
        }

        public bool Remove(T ancestor, T descendant)
        {
            if (!_relationships.TryGetValue(ancestor, out var descendants))
            {
                return false;
            }

            return descendants.Remove(descendant);
        }

        int IComparer<T>.Compare(T ancestor, T descendant)
        {
            var visited = new HashSet<T>();

            if (IsDecendent(ancestor, descendant, visited))
                return -1;
            
            if (IsDecendent(descendant, ancestor, visited))
                return 1;

            return _comparer.Compare(ancestor, descendant);
        }

        private bool IsDecendent(T ancestor, T descendant) =>
            IsDecendent(ancestor, descendant, new HashSet<T>());

        private bool IsDecendent(T ancestor, T descendant, ISet<T> visited)
        {
            if (!visited.Add(ancestor))
                return false;

            return
                _relationships.TryGetValue(ancestor, out var descendants) &&
                (
                    descendants.Contains(descendant) ||
                    descendants.Any(d => IsDecendent(d, descendant, visited))
                );
        }
    }
}