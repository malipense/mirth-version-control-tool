using System;
using System.Collections.Generic;
using NextGen.Cli.Commands.Exceptions;
using NextGen.Cli.Interfaces;

namespace NextGen.Cli.Commands
{
    public class Exit : ICommand
    {
        public string Name => "EXIT";
        public string Description => "exit the application.";
        public List<Option> Options => null;

        public string Execute()
        {
            Environment.Exit(1);
            return "Bye";
        }

        public string Execute(IDictionary<string, string> parameters)
        {
            return ExceptionMessages.TakesNoParameters;
        }
    }
}
