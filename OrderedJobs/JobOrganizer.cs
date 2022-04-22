using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderedJobs.Test
{
    public class JobOrganizer
        : IOrderedJobs
        , IComparer<char>
    {
        private readonly IComparer<char> _comparer;
        private readonly IDictionary<char, ICollection<char>> _jobs;

        public JobOrganizer()
            : this(Comparer<char>.Default)
        { }

        public JobOrganizer(IComparer<char> comparer)
        {
            _comparer = comparer;
            _jobs = new Dictionary<char, ICollection<char>>();
            //new SortedDictionary<char, ICollection<char>>(comparer);
        }

        public void Register(char job)
        {
            if (_jobs.TryGetValue(job, out _))
                return;

            _jobs.Add(job, new HashSet<char>());
        }

        public void Register(char dependentJob, char independentJob)
        {
            Register(dependentJob);
            Register(independentJob);

            _jobs[dependentJob].Add(independentJob);
        }

        public string Sort()
        {
            var jobs = _jobs.Keys.ToArray();

            Array.Sort(jobs, this);

            return new string(jobs);
        }

        int IComparer<char>.Compare(char x, char y)
        {
            if (HasDependency(x, y))
                return 1;
            
            if (HasDependency(y, x))
                return -1;

            return _comparer.Compare(x, y);
        }

        private bool HasDependency(char x, char y)
        {
            return HasDependency(x, y, new HashSet<(char, char)>());
        }

        private bool HasDependency(char x, char y, ISet<(char, char)> tested)
        {
            if (!tested.Add(new(x, y)))
                return false;

            var dependencies = _jobs[x];

            return
                dependencies.Contains(y) ||
                dependencies.Any(z => HasDependency(z, y, tested));
        }
    }
}