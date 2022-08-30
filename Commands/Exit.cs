using System;
using System.Collections.Generic;

namespace NextGen.Cli.Commands
{
    public class Exit : ICommand
    {
        public string Name => "exit";
        public string Description => "Exit the application.";
        public string[] Parameters => Array.Empty<string>();

        public string Execute()
        {
            Environment.Exit(1);
            return "Bye";
        }

        public string Execute(Dictionary<string, string> parameters)
        {
            return "This command does not take arguments, type help to get detailed information.";
        }
    }
}
