using System;

namespace NextGen.Cli.Commands.Exceptions
{
    public class ExceptionMessages
    {
        public static readonly string MissingGitToken = "No GIT_TOKEN environment variable found, re-run the application filling the --token parameter or add GIT_TOKEN to user environment variables.";
        public static readonly string RequiredParameters = "This command requires the parameters to be filled, type help to see information.";
        public static readonly string GitTokenWarning = "No token provided, attempting to retrieve from environment variable GIT_TOKEN...";
        public static readonly string TakesNoParameters = "This command does not take arguments, type help to get detailed information.";
        public static readonly string InvalidParameters = "The parameters are incorrect.";
        public static readonly string CommandNotFound = "Command not found.";
    }
}
