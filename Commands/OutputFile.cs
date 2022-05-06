using System;
using System.Collections.Generic;
using System.IO;

namespace version_control_tool.Commands
{
    public class OutputFile : ICommand
    {
        private string[] _parameters = new string[4]
        {
            "-path",
            "-extension",
            "-name",
            "-data"
        };
        public string Name { get => "outputfile"; }
        public string[] Parameters { get => _parameters; }
        public string CallBack(Dictionary<string, string> parameters)
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
        public string CallBack()
        {
            throw new NotImplementedException();
        }
    }
}
