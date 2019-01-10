using System;
using System.Net.Mail;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;
using System.Collections.Generic;
using System.Linq;
using AxSoft.TemplateEmail.Net;
using AxSoft.TemplateEmail.Templating;
using AxSoft.TemplateEmail.Xml;
using System.IO;
using System.Text;

namespace StatusReport
{
    public class Service : IService
    {
        private Uri _accountUri;
        private string _personalAccessToken;
        private StreamWriter _streamWriter = null;

        public Service(Uri accountUri, string personalAccessToken, StreamWriter streamWriter = null)
        {
            _accountUri = accountUri;
            _personalAccessToken = personalAccessToken;
            _streamWriter = streamWriter;
        }

        public IEnumerable<IProject> GetProjects(string portfolioName)
        {
            var projects = new List<IProject>();

            //create a wiql object and build our query
            Wiql wiql = new Wiql()
            {
                Query = "Select * " +
                        "From WorkItems " +
                        "Where [Work Item Type] = 'Epic' " +
                        "And [System.TeamProject] = '" + portfolioName + "' " +
                        "And [System.State] <> 'Closed' " +
                        "And [System.State] <> 'Removed' " +
                        "And [System.State] <> 'Resolved' " +
                        "Order By [Title] Desc"
            };

            //build a list of the fields we want to see
            string[] fields = new string[3];
            fields[0] = "System.Id";
            fields[1] = "System.Title";
            fields[2] = "System.Description";

            var workItems = ExecuteQuery(wiql, fields);

            //loop though work items and write to console
            foreach (var workItem in workItems)
            {
                string description = string.Empty;
                try
                {
                    description = (string)workItem.Fields["System.Description"];
                }
                catch { }

                projects.Add(new Project((int)workItem.Id, (string)workItem.Fields["System.Title"], description, workItem.Url));
            }

            return projects;
        }

        public ProjectStatus GetProjectStatus(int projectId)
        {
            ProjectStatus projectStatus = null;

            //create a wiql object and build our query
            Wiql wiql = new Wiql()
            {
                Query = "Select * " +
                        "From WorkItems " +
                        "Where [System.Id] = " + projectId
            };

            //build a list of the fields we want to see
            string[] fields = new string[10];
            fields[0] = "Custom.BusinessOwner";
            fields[1] = "Custom.ProjectManager";
            fields[2] = "Microsoft.VSTS.Scheduling.TargetDate";
            fields[3] = "Custom.Progress";
            fields[4] = "Custom.ChangeRequest";
            fields[5] = "Microsoft.VSTS.CMMI.Escalate";
            fields[6] = "Custom.EmailStatusTo";
            fields[7] = "Custom.EmailStatusCc";
            fields[8] = "System.Title";
            fields[9] = "System.Description";

            var workItems = ExecuteQuery(wiql, fields);

            //there can only be one or it wasn't found
            if (workItems.Count == 1)
            {
                var workItem = workItems[0];

                var project = new Project((int)workItem.Id, workItem.Fields["System.Title"].ToString(), 
                    workItem.Fields["System.Description"].ToString(), workItem.Url);

                //get milestones
                var milestones = GetMilestones();

                //get risks and issues
                var risksAndIssues = GetRisks(project.Id);

                bool escalation = false;
                try
                {
                    escalation = ((string)workItem.Fields["Microsoft.VSTS.CMMI.Escalate"]).ToUpper() == "YES";
                }
                catch { }

                //populate the project status
                projectStatus = new ProjectStatus(project,
                    workItem.Fields["Custom.BusinessOwner"].ToString(),
                    workItem.Fields["Custom.ProjectManager"].ToString() ?? string.Empty,
                    (DateTime)workItem.Fields["Microsoft.VSTS.Scheduling.TargetDate"],
                    workItem.Fields["Custom.Progress"].ToString() ?? string.Empty,
                    milestones,
                    risksAndIssues,
                    (bool)workItem.Fields["Custom.ChangeRequest"],
                    escalation,
                    workItem.Fields["Custom.EmailStatusTo"].ToString() ?? string.Empty,
                    workItem.Fields["Custom.EmailStatusCc"].ToString() ?? string.Empty);
            }

            return projectStatus;
        }

        private List<Risk> GetRisks(int id)
        {
            var risks = new List<Risk>();

            //==========================================

            VssBasicCredential credentials = new VssBasicCredential("", _personalAccessToken);

            List<WorkItem> workItems2 = null;
            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(_accountUri, credentials))
            {
                WorkItem workItem = workItemTrackingHttpClient.GetWorkItemAsync(id, expand: WorkItemExpand.Relations).Result;

                if (workItem.Relations != null)
                {
                    List<int> list = new List<int>();

                    Console.WriteLine("Getting child work items from parent...");

                    foreach (var relation in workItem.Relations)
                    {
                        //get the child links
                        //if (relation.Rel == "System.LinkTypes.Hierarchy-Forward")
                        {
                            var lastIndex = relation.Url.LastIndexOf("/");
                            var itemId = relation.Url.Substring(lastIndex + 1);
                            list.Add(Convert.ToInt32(itemId));

                            Console.WriteLine("  {0} ", itemId);
                        }
                    }

                    int[] workitemIds = list.ToArray();

                    workItems2 = workItemTrackingHttpClient.GetWorkItemsAsync(workitemIds).Result;
                }

                Console.WriteLine("Getting full work item for each child...");

                foreach (var item in workItems2)
                {
                    if (item.Fields["System.WorkItemType"].ToString() == "Issue" ||
                        item.Fields["System.WorkItemType"].ToString() == "Impediment")
                    {
                        if (((string)item.Fields["Microsoft.VSTS.Common.Severity"] == "1 - Critical" ||
                            (string)item.Fields["Microsoft.VSTS.Common.Severity"] == "2 - High") &&
                            ((long)item.Fields["Microsoft.VSTS.Common.Priority"] == 1 ||
                            (long)item.Fields["Microsoft.VSTS.Common.Priority"] == 2))
                        {
                            string migration = string.Empty;
                            try
                            {
                                if (item.Fields["Microsoft.VSTS.CMMI.MitigationPlan"] != null)
                                {
                                    migration = (string)item.Fields["Microsoft.VSTS.CMMI.MitigationPlan"];
                                }
                                else
                                {
                                    migration = (string)item.Fields["Microsoft.VSTS.Common.Resolution"];
                                }
                            }
                            catch { }

                            risks.Add(new Risk((int)item.Id,
                                (string)item.Fields["System.Title"],
                                (string)item.Fields["System.Description"] ?? string.Empty,
                                (string)item.Fields["Microsoft.VSTS.Common.Severity"] ?? string.Empty,
                                (long)item.Fields["Microsoft.VSTS.Common.Priority"],
                                migration,
                                (string)item.Fields["Custom.ActionOwner"] ?? string.Empty,
                                item.Url));
                        }
                    }
                }
            }

            return risks.OrderBy(risk => risk.Severity).ThenBy(risk => risk.Priority).ThenBy(risk => risk.Id).ToList();
        }

        public MailMessage GetEmail(ProjectStatus projectStatus, string to, string cc, string subject)
        {
            MailMessage mailMessage = null;

            // Status Variables
            //var variables = new
            //{
            //    FirstName = "Axel",
            //    LastName = "Zarate",
            //    Username = "azarate",
            //    Ignored = default(object),
            //    Logo = "http://www.logotree.com/images/single-logo-design/logo-design-sample-14.jpg",
            //    ActivationLink = "http://localhost/Account/Activate/azarate",
            //    Benefits = new[]
            //    {
            //            "Free support",
            //            "Great discounts",
            //            "Unlimited access"
            //        },
            //    IsPremiumUser = true
            //};

            var templateCompiler = new TemplateCompiler();
            var xmlSerializer = new CustomXmlSerializer();

//            var emailSender = new DummyEmailSender(message => _streamWriter.Write(message.Body)); // or new DummyEmailSender(Dump)
//            var emailSender = new MailGunEmailSender();
            var emailSender = new NullEmailSender();

            var templateDirectory = Path.Combine(Environment.CurrentDirectory, "Templates");
            var layoutFile = Path.Combine(templateDirectory, "layout.html");
            var xsltFilePath = Path.Combine(templateDirectory, "ActivateAccount.xslt");
            var templateEmailSender = new TemplateEmailSender(emailSender, templateCompiler, xmlSerializer)
            {
                LayoutFilePath = layoutFile
            };

            mailMessage = templateEmailSender.ConstructMailMessage(xsltFilePath, projectStatus, to, cc, subject);

//            templateEmailSender.Send(xsltFilePath, projectStatus, to, cc, subject);

            return mailMessage;
        }

        private List<IMilestone> GetMilestones()
        {
            List<IMilestone> milestones = new List<IMilestone>();

            List<IMilestone> milestones2 = new List<IMilestone>();
            milestones2.Add(new Milestone(18, "Power BI Work stream Kickoff", DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-6), "18"));
            milestones2.Add(new Milestone(4, "Cross-Team project Setup Discussion", DateTime.Now.AddDays(-14), DateTime.Now.AddDays(-14), "4"));
            milestones2.Add(new Milestone(19, "Project Kickoff with the Business", DateTime.Now.AddDays(14), DateTime.MinValue, "19"));
            milestones2.Add(new Milestone(21, "Vendor Onboarding Complete", DateTime.Now.AddDays(14), DateTime.MinValue, "21"));
            milestones2.Add(new Milestone(26, "Planning Sprint 0 Begins", DateTime.Now.AddDays(28), DateTime.MinValue, "26"));
            milestones2.Add(new Milestone(31, "Requirements and Design Complete", DateTime.Now.AddMonths(1), DateTime.MinValue, "31"));
            milestones2.Add(new Milestone(76, "Go Live", DateTime.Now.AddMonths(5), DateTime.MinValue, "76"));

            foreach (var milestone in milestones2)
            {
                if (milestone.ActualDate == string.Empty ||
                    (DateTime.Parse(milestone.ActualDate) - DateTime.Now).TotalDays >= -7)
                {
                    milestones.Add(milestone);
                }
            }

            return milestones.OrderBy(milestone => milestone.TargetDate).ThenBy(milestone => milestone.Id).ToList();
        }

        private IList<WorkItem> ExecuteQuery(Wiql wiql, string[] fields)
        {
            var workItems = new List<WorkItem>();

            //create uri and VssBasicCredential variables
            VssBasicCredential credentials = new VssBasicCredential("", _personalAccessToken);
            //VssConnection connection = new VssConnection(_accountUri, credentials);

            //create instance of work item tracking http client
            using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(_accountUri, credentials))
            {
                //execute the query to get the list of work items in the results
                WorkItemQueryResult workItemQueryResult = workItemTrackingHttpClient.QueryByWiqlAsync(wiql).Result;

                //some error handling                
                if (workItemQueryResult.WorkItems.Count() != 0)
                {
                    //need to get the list of our work items and put them into an array
                    List<int> list = new List<int>();
                    foreach (var item in workItemQueryResult.WorkItems)
                    {
                        list.Add(item.Id);
                    }
                    int[] arr = list.ToArray();

                    //get work items for the ids found in query
                    workItems = workItemTrackingHttpClient.GetWorkItemsAsync(arr, fields, workItemQueryResult.AsOf).Result;
                    foreach (var f in workItems[0].Fields)
                    {
                        Console.WriteLine(f.Key);
                    }

                }
            }

            return workItems;
        }
    }
}
