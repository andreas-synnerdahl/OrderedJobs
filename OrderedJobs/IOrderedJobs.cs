namespace OrderedJobs
{
    public interface IOrderedJobs
    {
        void Register(char job);

        void Register(char dependentJob, char independentJob);

        string Sort();
    }
}
