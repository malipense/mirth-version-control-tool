using System;
using System.Collections.Generic;
using System.IO;
using NextGen.Cli.Interfaces;
using NextGen.Cli.Commands.Exceptions;

namespace NextGen.Cli.Commands
{
    public class OutputFile : ICommand
    {
        private List<Option> _options = new List<Option>
        {
            new Option("--path", "path where to save the file"),
            new Option("--extension", "file extension"),
            new Option("--name", "filename")
        };
        public string Name => "OUTPUTFILE";
        public string Description => "writes file to the specified directory.";
        public List<Option> Options => _options;
        public string Execute(IDictionary<string, string> parameters)
        {
            string output;

            parameters.TryGetValue("-path", out string path);
            parameters.TryGetValue("-data", out string data);
            parameters.TryGetValue("-name", out string name);
            
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
