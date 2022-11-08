using System;
using System.Collections.Generic;
using System.Text;
using NextGen.Cli.Commands.Exceptions;
using NextGen.Cli.Interfaces;

namespace NextGen.Cli.Commands
{
    public class Help : ICommand
    {
        public string Name => "HELP";
        public string Description => "list all the available commands in the current version.";
        public List<Option> Options => null;
        public string Execute()
        {
            StringBuilder manual = new StringBuilder("List of available commands at the current version (1.0.1).\n");
       
            foreach(var cmd in CommandList.commands)
            {
                manual.Append("\n"+cmd.Name + " - " + cmd.Description);

                if ((cmd.Options != null) && cmd.Options.Count > 0)
                {
                    manual.Append("\n     options [ \n");
                    foreach (var opt in cmd.Options)
                        manual.Append($"         {opt.Name} - {opt.Description}\n");
                    manual.Append("     ]\n");
                }
            }
            return manual.ToString();
        }
        public string Execute(IDictionary<string, string> parameters)
        {
            return ExceptionMessages.TakesNoParameters;
        }
    }
}
