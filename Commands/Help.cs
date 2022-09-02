using System;
using System.Collections.Generic;
using System.Text;
using version_control_tool.Commands.Exceptions;
using NextGen.Cli.Interfaces;

namespace NextGen.Cli.Commands
{
    public class Help : ICommand
    {
        public string Name => "HELP";
        public string Description => "list all the available commands in the current version.";
        public string[] Parameters => Array.Empty<string>();
        public string Execute()
        {
            StringBuilder manual = new StringBuilder("List of available commands at the current version (1.0).\n");
       
            foreach(var cmd in CommandList.commands)
            {
                manual.Append("\n"+cmd.Name + " - " + cmd.Description);

                if (cmd.Parameters.Length > 0)
                {
                    manual.Append("\n     parameters [ \n");
                    foreach (var param in cmd.Parameters)
                        manual.Append("         " + param.ToString() + "\n");
                    manual.Append("     ]\n");
                }
            }
            return manual.ToString();
        }
        public string Execute(Dictionary<string, string> parameters)
        {
            return ExceptionMessages.NoParameters;
        }
    }
}
