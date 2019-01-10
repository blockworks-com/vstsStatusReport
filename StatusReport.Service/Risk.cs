using System;

namespace StatusReport
{
    public class Risk : IRisk
    {
        public Risk(int id, string title, string description, string severity, long priority, string mitigation, 
            string owner, string url)
        {
            _id = id;
            _title = title;
            _description = description;
            _severity = severity;
            _priority = priority.ToString();
            _mitigation = mitigation;
            _owner = owner;
            _url = url;
        }

        private int _id;
        public int Id { get { return _id; } }

        private string _title;
        public string Title { get { return _title; } }

        private string _description;
        public string Description { get { return _description; } }

        private string _severity;
        public string Severity { get { return _severity; } }

        private string _priority;
        public string Priority { get { return _priority; } }

        private string _mitigation;
        public string Mitigation { get { return _mitigation; } }

        private string _owner;
        public string Owner{ get { return _owner; } }

        private string _url;
        public string Url { get { return _url; } }
    }
}
