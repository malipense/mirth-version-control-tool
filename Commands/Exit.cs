using System;
using System.Collections.Generic;

namespace NextGen.Cli.Commands
{
    public class Exit : ICommand
    {
        public string Name => "exit";

        public string[] Parameters => Array.Empty<string>();

        public string CallBack()
        {
            Environment.Exit(1);
            return "Bye";
        }

        public string CallBack(Dictionary<string, string> parameters)
        {
            return "This command does not take arguments, type help to get detailed information.";
        }
    }
}
