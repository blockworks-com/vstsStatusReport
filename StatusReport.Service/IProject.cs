using System;

namespace StatusReport
{
    public interface IProject
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        string Url { get; }
    }
}