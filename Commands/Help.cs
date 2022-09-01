using System;
using System.Collections.Generic;
using System.Text;
using version_control_tool.Commands.Exceptions;
using NextGen.Cli.Interfaces;

namespace NextGen.Cli.Commands
{
    public class Help : ICommand
    {
        public string Name => "help";
        public string Description => "List all the available commands in the current version.\n";
        public string[] Parameters => Array.Empty<string>();
        public string Execute()
        {
            StringBuilder manual = new StringBuilder("List of available commands at the current version (1.0)\n\n");
       
            foreach(var cmd in CommandList.commands)
            {
                manual.Append("\n" + cmd.Name + " - " + cmd.Description);

                if (cmd.Parameters.Length > 0)
                {
                    manual.Append("\nparameters [ \n");
                    foreach (var param in cmd.Parameters)
                        manual.Append("     " + param.ToString() + "\n");
                    manual.Append("]\n");
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
