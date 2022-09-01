using System;
using System.Collections.Generic;
using System.Text;

namespace version_control_tool.Commands.Exceptions
{
    public static class ExceptionMessages
    {
        /// <summary>
        /// Token not provided as a parameter, and neither was found in the environment variables
        /// </summary>
        public static readonly string MissingGitToken = "No GIT_TOKEN environment variable found, re-run the application filling the --token parameter or add GIT_TOKEN to user environment variables.";
        /// <summary>
        /// The required parameters were not provided
        /// </summary>
        public static readonly string RequiredParameters = "This command requires the parameters to be filled, type help to see information.";
        /// <summary>
        /// The --token parameter was not provided
        /// </summary>
        public static readonly string GitTokenWarning = "No token provided, attempting to retrieve from environment variable GIT_TOKEN...";
        /// <summary>
        /// This command takes no parameter
        /// </summary>
        public static readonly string NoParameters = "This command does not take arguments, type help to get detailed information.";
    }
}
