using System;
using System.Collections.Generic;

namespace NextGen.Cli.Commands
{
    public class Help : ICommand
    {
        public string Name { get => "help"; }
        public string[] Parameters { get => Array.Empty<string>(); }
        public string CallBack()
        {
            return "List of available commands at the current version (1.0)\n\n" +
                "PULL - pull data from remote server\n" +
                "   parameters [ \n" +
                "   -server = endpoint of the remote server\n" +
                "   -username = user login\n" +
                "   -password = user password\n" +
                "   -path (optional) = location where the files should be saved\n" +
                "   ]\n\n" +
                "OUTPUTFILE - writes file to a specified directory\n" +
                "   parameters [\n" +
                "   -path = location where to output the file\n" +
                "   -name = file name and extension\n" +
                "   ]\n";
        }
        public string CallBack(Dictionary<string, string> parameters)
        {
            return "This command does not take arguments, type help to get detailed information.";
        }
    }
}
