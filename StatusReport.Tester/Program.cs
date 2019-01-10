using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;
using Utilities;

namespace StatusReport.Tester
{
    class Program
    {
        const string VARIABLE_PREFIX = "StatusReport";

        private static void Dump(MailMessage message)
        {
            // Write to file
            var file = File.Create(Path.Combine(Environment.CurrentDirectory, "output.html"));
            var streamWriter = new StreamWriter(file, Encoding.UTF8);
            streamWriter.Write(message.Body);
            streamWriter.Flush();
            streamWriter.Dispose();
            file.Close();

            Console.WriteLine("Message Body saved to " + file.Name);
        }

        static void Main(string[] args)
        {
            // Command line parsing
            ArgumentParser CommandLine = new ArgumentParser(args);

            #region Display Help
            if (CommandLine["?"] != null && CommandLine["help"] != string.Empty)
            {
                Console.WriteLine("STATUSREPORT.TESTER /TOKEN: /URI: [/PORTFOLIO:] [/PROJECTID:]");
                Console.WriteLine("Generates a status report based on Azure DevOps and the HTML template.");
                Console.WriteLine();
                Console.WriteLine(" /TOKEN        Azure DevOps personal access tokens.");
                Console.WriteLine(" /URI          Azure DevOps URL.");
                Console.WriteLine(" /PORTFOLIO    Azure DevOps Portfolio.");
                Console.WriteLine(" /PROJECTID    ID of Epic representing the Project to report.");
                return;
            }
            #endregion

            #region Handle Token and Uri parameters from command line and config file
            var token = CommandLine.SearchFor("token", VARIABLE_PREFIX);
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Token parameter is required.");
                Console.ReadKey();
                return;
            }

            var uriString = CommandLine.SearchFor("uri", VARIABLE_PREFIX);
            if (string.IsNullOrWhiteSpace(uriString))
            {
                Console.WriteLine("Uri parameter is required.");
                Console.ReadKey();
                return;
            }
            var uri = new Uri(uriString);
            #endregion

            //Create service since we have the token and uri
            var svc = new StatusReport.Service(uri, token);

            #region Handle Project and Portfolio parameters from command line and config file OR prompt if necessary
            //Check for optional parameters
            //Portfolio and Project are optional
            //If we have project then we don't need portfolio
            var projectIdstring = CommandLine.SearchFor("projectid", VARIABLE_PREFIX);
            if (string.IsNullOrWhiteSpace(projectIdstring))
            {
                //project wasn't provided but we can query if we have portfolio
                var portfolioName = CommandLine.SearchFor("portfolio", VARIABLE_PREFIX);
                if (string.IsNullOrWhiteSpace(portfolioName))
                {
                    Console.WriteLine("Porfolio OR project parameter must be provided.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    //we have portfolio so get list of projects from Azure DevOps in the portfolio
                    var projects = svc.GetProjects(portfolioName);
                    foreach (var proj in projects)
                    {
                        Console.WriteLine(proj.Id);
                    }
                    Console.WriteLine();

                    // Select Project
                    Console.WriteLine("Enter the ID of the Project to report.");
                    projectIdstring = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(projectIdstring))
                    {
                        Console.WriteLine("Project parameter is required.");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            int projectId = 0;
            bool success = Int32.TryParse(projectIdstring, out projectId);
            if (!success)
            {
                Console.WriteLine("Project Id must be a number.");
                Console.ReadKey();
                return;
            }
            #endregion

            //Get Project Status
            var projectStatus = svc.GetProjectStatus(projectId);
            if (projectStatus == null)
            {
                Console.WriteLine("Project Status is empty.");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Project: " + projectStatus.Project.Name);

                // Get Status Email Body
                var mailMessage = svc.GetEmail(projectStatus, projectStatus.To, projectStatus.Cc, projectStatus.Project.Name + " Weekly Status Report");
                Dump(mailMessage);
            }

            //end
            return;
        }
    }
}
