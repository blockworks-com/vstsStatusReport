using System;
using System.Collections.Generic;

namespace StatusReport
{
    public class ProjectStatus : IProjectStatus
    {
        public ProjectStatus(Project project, string businessOwner, string projectManager, DateTime targetGoLive, string progress,
            IList<IMilestone> milestones, IList<Risk> risksAndIssues, bool changeRequest, bool escalation, string to, string cc)
        {
            _project = new Project(project.Id, project.Name, project.Description.Replace("<div>", "").Replace("</div>", ""), project.Url);
            _businessOwner = businessOwner;
            _projectManager= projectManager;
            if (targetGoLive == DateTime.MinValue)
            {
                _targetGoLive = string.Empty;
            }
            else if (targetGoLive.Year == DateTime.Now.Year)
            {
                _targetGoLive = targetGoLive.ToString("MMM dd");
            }
            else
            {
                _targetGoLive = targetGoLive.ToString("MMM dd, yyyy");
            }
            _progress = progress;
            _milestones = milestones;
            _risksAndIssues = risksAndIssues;
            _changeRequest = changeRequest;
            _escalation = escalation;
            _to = to;
            _cc = cc;
            _date = DateTime.Now.ToString("MMM dd, yyyy");
        }

        private IProject _project;
        public IProject Project { get { return _project; } }

        private string _businessOwner;
        public string BusinessOwner { get { return _businessOwner; } }

        private string _projectManager;
        public string ProjectManager { get { return _projectManager; } }

        private string _targetGoLive;
        public string TargetGoLive { get { return _targetGoLive; } }

        private string _progress;
        public string Progress { get { return _progress; } }

        private IList<IMilestone> _milestones;
        public IList<IMilestone> Milestones { get { return _milestones; } }

        private IList<Risk> _risksAndIssues;
        public IList<Risk> RisksAndIssues { get { return _risksAndIssues; } }

        private bool _changeRequest;
        public bool ChangeRequest { get { return _changeRequest; } }

        private bool _escalation;
        public bool Escalation { get { return _escalation; } }

        private string _to;
        public string To { get { return _to; } }

        private string _cc;
        public string Cc { get { return _cc; } }

        private string _date;
        public string Date { get { return _date; } }
    }
}
