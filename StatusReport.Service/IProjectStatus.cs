using System.Collections.Generic;

namespace StatusReport
{
    public interface IProjectStatus
    {
        IProject Project { get; }
        string BusinessOwner { get; }
        string ProjectManager { get; }
        string TargetGoLive { get; }
        string Progress { get; }
        IList<IMilestone> Milestones { get; }
        IList<Risk> RisksAndIssues { get; }
        bool ChangeRequest { get; }
        bool Escalation { get; }
        string To { get; }
        string Cc { get; }
        string Date { get; }
    }
}