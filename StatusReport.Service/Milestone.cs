using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusReport
{
    public class Milestone : IMilestone
    {
        public Milestone(int id, string title, DateTime targetDate, DateTime actualDate, string url)
        {
            _id = id;
            _title = title;
            _url = url;
            if (targetDate == DateTime.MinValue)
            {
                _targetDate = string.Empty;
            }
            else if (targetDate.Year == DateTime.Now.Year)
            {
                _targetDate = targetDate.ToString("MMM dd");
            }
            else
            {
                _targetDate = targetDate.ToString("MMM dd, yyyy");
            }
            if (actualDate == DateTime.MinValue)
            {
                _actualDate = string.Empty;
            }
            else if (actualDate.Year == DateTime.Now.Year)
            {
                _actualDate = actualDate.ToString("MMM dd");
            }
            else
            {
                _actualDate = actualDate.ToString("MMM dd, yyyy");
            }
        }

        private int _id;
        public int Id { get { return _id; } }

        private string _title;
        public string Title { get { return _title; } }

        private string _targetDate;
        public string TargetDate { get { return _targetDate; } }

        private string _actualDate;
        public string ActualDate { get { return _actualDate; } }

        private string _url;
        public string Url { get { return _url; } }
    }
}
