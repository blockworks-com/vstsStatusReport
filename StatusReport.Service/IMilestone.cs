namespace StatusReport
{
    public interface IMilestone
    {
        int Id { get; }
        string Title { get; }
        string TargetDate { get; }
        string ActualDate { get; }
        string Url { get; }
    }
}