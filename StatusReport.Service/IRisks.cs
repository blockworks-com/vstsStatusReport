namespace StatusReport
{
    public interface IRisk
    {
        int Id { get; }
        string Title { get; }
        string Description { get; }
        string Severity { get; }
        string Priority { get; }
        string Mitigation { get; }
        string Owner { get; }
        string Url { get; }
    }
}