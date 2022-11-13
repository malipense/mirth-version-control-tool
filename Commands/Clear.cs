using NextGen.Cli.Commands.Exceptions;
using NextGen.Cli.Interfaces;
using System;
using System.Collections.Generic;

namespace NextGen.Cli.Commands
{
    internal class Clear : ICommand
    {
        public string Name => "CLEAR";
        public string Description => "clear console screen.";
        public List<Option> Options => null;

        public string Execute()
        {
            Console.Clear();
            return "";
        }

        public string Execute(IDictionary<string, string> parameters)
        {
            return ExceptionMessages.TakesNoParameters;
        }
    }
}
