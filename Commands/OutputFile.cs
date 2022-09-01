using System;
using System.Collections.Generic;
using System.IO;
using NextGen.Cli.Interfaces;
using version_control_tool.Commands.Exceptions;

namespace NextGen.Cli.Commands
{
    public class OutputFile : ICommand
    {
        private string[] _parameters = new string[4]
        {
            "--path",
            "--extension",
            "--name",
            "--data"
        };
        public string Name => "outputfile";
        public string Description => "writes file to the specified directory";
        public string[] Parameters => _parameters;
        public string Execute(Dictionary<string, string> parameters)
        {
            string output = null;
            string path = null;
            string data = null;
            string name = null;

            parameters.TryGetValue("-path", out path);
            parameters.TryGetValue("-data", out data);
            parameters.TryGetValue("-name", out name);
            
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            try
            {
                output = $"Writting file to {path}...";
                File.WriteAllText($"{path}/{name}", data);
            }
            catch(Exception ex)
            {
                output = $"An error ocurred while writting file to {path}\n" +
                    $"InnerException:{ex}";
            }

            return output;
        }
        public string Execute()
        {
            return ExceptionMessages.RequiredParameters;
        }
    }
}
