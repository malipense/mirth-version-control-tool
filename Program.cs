using System;
using System.Collections.Generic;
using System.Linq;
using NextGen.Cli.Commands.Exceptions;
using NextGen.Cli.Interfaces;

namespace NextGen.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            HelloUser();

            while(true)
            {
                string[] input = Console.ReadLine().Trim().Split('|');

                if (string.IsNullOrEmpty(input[0]))
                    Console.WriteLine("Type HELP to get a list of available commands.");

                else if (input.Length == 1)
                {
                    var executionResult = TryExecuteCommand(input[0]);

                    Console.WriteLine(executionResult);
                }
                else
                {
                    string chainedResult = null;

                    for (int i = 0; i < input.Length; i++)
                    {
                        if (chainedResult == null)
                        {
                            chainedResult = TryExecuteCommand(input[0].ToLower());
                            continue;
                        }
                        chainedResult = TryExecuteCommand(input[i].ToLower(), chainedResult);
                        Console.WriteLine(chainedResult);
                    }
                }
            }
        }

        private static bool TryParseArgs(List<Option> options, IEnumerable<string> args, out IDictionary<string, string> namedArgs)
        {
            namedArgs = new Dictionary<string, string>();
            var argsQueue = new Queue<string>(args);

            if (options.Count() != args.Where(a => a.StartsWith("--")).Count())
                return false;

            while (argsQueue.Any() && argsQueue.Peek().StartsWith("--"))
            {
                var name = argsQueue.Dequeue();
                if (options.FirstOrDefault(o => o.Name == name) is null)
                    return false;

                namedArgs.Add(name, argsQueue.Dequeue());
            }

            return argsQueue.Count() == 0;
        }

        private static string TryExecuteCommand(string input, string previousCommandResult = null)
        {
            var name = input.Trim().Split(' ').First();
            var parameters = input.Trim().Split(' ').Skip(1);
            
            var command = CommandList.commands.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            if(command == null)
                return ExceptionMessages.CommandNotFound;

            if (parameters.Any())
            {
                var succedeed = TryParseArgs(command.Options, parameters, out IDictionary<string, string> namedArgs);
                if (succedeed)
                    return command.Execute(namedArgs);
                else
                    return ExceptionMessages.InvalidParameters;
            }
            else
                return command.Execute();
        }

        private static void HelloUser()
        {
            string message = "";
            message += "********************************************************************************************\n";
            message += "*****************************************WELCOME********************************************\n";
            message += "********************************************************************************************\n";
            message += "********************************************************************************************\n\n";
            message += "Nextgen CLI\n";
            message += "This is a very basic tool created to export resources out of nextgen connect (mirth connect)\n";
            message += "through its REST API.\n";
            message += "The goal is just to make it easier to backup your channels and libraries.\n";
            message += "Currently only GET operations are implemented. You can't use this tool to create/update resources yet.\n";
            message += "I was working on creating commands to commit files to a git repository\n";
            message += "but I decided to leave it aside for the moment.\n";
            message += "\nFeel free to modify it and reuse it as it suits you.\n";
            message += "Source code avaliable on github malipense/nextgencli.                                 - Cuco\n";
            Console.WriteLine(message);
        }
    }
}
