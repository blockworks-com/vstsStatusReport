using Microsoft.TeamFoundation.Core.WebApi;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace StatusReport
{
    public interface IService
    {
        MailMessage GetEmail(ProjectStatus projectStatus, string to, string cc, string subject);
        ProjectStatus GetProjectStatus(int projectName);
        IEnumerable<IProject> GetProjects(string portfolioName);
    }
}