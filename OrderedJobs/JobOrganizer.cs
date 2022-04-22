using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderedJobs.Test
{
    public class JobOrganizer : IOrderedJobs
    {
        private readonly ISet<char> _jobs;
        private readonly RelationshipComparer<char> _comparer;

        public JobOrganizer()
        {
            _jobs = new SortedSet<char>(Comparer<char>.Default);
            _comparer = new RelationshipComparer<char>();
        }

        public void Register(char job)
        {
            _jobs.Add(job);
        }

        public void Register(char dependentJob, char independentJob)
        {
            _jobs.Add(dependentJob);

            if (independentJob == dependentJob)
                return;

            _jobs.Add(independentJob);

            _comparer.Add(independentJob, dependentJob);
        }

        public string Sort()
        {
            var temp = _jobs.ToArray();

            Array.Sort(temp, _comparer);

            return new string(temp);
        }
    }
}