using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Utilities
{
    /// <summary>
    /// ArgumentParser class
    /// Intelligent handling of command line arguments based on Richard Lopes' (GriffonRL's)
    /// class at http://www.codeproject.com/Articles/3111/C-NET-Command-Line-Arguments-Parser
    /// Supports both linux-style and windows-style parameter arguments.
    /// </summary>
    public class ArgumentParser
    {
        // Variables
        private StringDictionary Parameters;

        // Constructor
        public ArgumentParser(string[] Args)
        {
            Parameters = new StringDictionary();
            Regex Spliter = new Regex(@"^-{1,2}|^/|=|:",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string Parameter = null;
            string[] Parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'
            foreach (string Txt in Args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                Parts = Spliter.Split(Txt, 3);

                switch (Parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                Parts[0] =
                                    Remover.Replace(Parts[0], "$1");

                                Parameters.Add(Parameter, Parts[0]);
                            }
                            Parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                                Parameters.Add(Parameter, "true");
                        }
                        Parameter = Parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                                Parameters.Add(Parameter, "true");
                        }

                        Parameter = Parts[1];

                        // Remove possible enclosing characters (",')
                        if (!Parameters.ContainsKey(Parameter))
                        {
                            Parts[2] = Remover.Replace(Parts[2], "$1");
                            Parameters.Add(Parameter, Parts[2]);
                        }

                        Parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (Parameter != null)
            {
                if (!Parameters.ContainsKey(Parameter))
                    Parameters.Add(Parameter, "true");
            }
        }

        // Retrieve a parameter value if it exists 
        // (overriding C# indexer property)
        public string this[string Param]
        {
            get
            {
                return (Parameters[Param]);
            }
        }

        /// <summary>
        /// Retrieve a parameter value if it exists , otherwise the given value
        /// </summary>
        /// <param name="key">The key to retrieve</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>A parameter value or the given default value</returns>
        public string GetOrDefault(string key, string defaultValue)
        {
            if (!Parameters.ContainsKey(key))
            {
                return defaultValue;
            }

            return Parameters[key];
        }

        /// <summary>
        /// Retrieve a parameter value if it exists, otherwise 
        /// retrieve from an environment variable if it exists, otherwise 
        /// retrieve from the app.config file if it exists, otherwise return an empty string.
        /// Ordered by priority: cli > envvars > config > defaults
        /// </summary>
        /// <param name="key">The key to retrieve</param>
        /// <param name="environmentPrefix">A prefix for the key when checking environment variables</param>
        /// <returns>A value or an empty string</returns>
        public string SearchFor(string key, string environmentPrefix)
        {
            string result = string.Empty;
            string envName = environmentPrefix + key;

            // Look for specific arguments values
            if (Parameters.ContainsKey(key))
            {
                result = Parameters[key];
            }
            else if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(envName)))
            {
                result = Environment.GetEnvironmentVariable(envName);
            }
            else if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[key]))
            {
                result = ConfigurationManager.AppSettings[key];
            }

            return result;
        }

    }
}