using System;
using System.Collections.Generic;
using version_control_tool.Commands.Exceptions;
using NextGen.Cli.Interfaces;

namespace NextGen.Cli.Commands
{
    public class Exit : ICommand
    {
        public string Name => "EXIT";
        public string Description => "exit the application.";
        public string[] Parameters => Array.Empty<string>();

        public string Execute()
        {
            Environment.Exit(1);
            return "Bye";
        }

        public string Execute(Dictionary<string, string> parameters)
        {
            return ExceptionMessages.NoParameters;
        }
    }
}
