namespace StatusReport
{
    public class Project : IProject
    {
        public Project(int id, string name, string description, string url)
        {
            _id = id;
            _name = name;
            _description = description;
            _url = url;
        }

        private int _id;
        public int Id { get { return _id; } }

        private string _name;
        public string Name { get { return _name; } }

        private string _description;
        public string Description { get { return _description; } }

        private string _url;
        public string Url { get { return _url; } }
    }
}
