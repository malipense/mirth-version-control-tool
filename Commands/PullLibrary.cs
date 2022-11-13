using NextGen.Cli.Interfaces;
using System;
using System.Collections.Generic;
using APIClient;
using NextGen.Cli.Commands.Exceptions;

namespace NextGen.Cli
{
    public class PullLibrary : ICommand
    {
        private List<Option> _options = new List<Option>
        {
            new Option("--server", "server's ip adress/hostname and port. Ex: 192.168.10.15:8443"),
            new Option("--username", "a valid mirth user"),
            new Option("--password", "mirth user's password"),
            new Option("--path", "path to save the exported files")
        };
        public string Name => "PULLLIB";
        public string Description => "pulls libraries from remote NextGen/Mirth server. Saves it to the provided path as xml.";
        public List<Option> Options => _options;
        public string Execute()
        {
            return ExceptionMessages.RequiredParameters;
        }
        public string Execute(IDictionary<string, string> parameters)
        {
            string output;
            parameters.TryGetValue("--server", out string server);
            parameters.TryGetValue("--username", out string username);
            parameters.TryGetValue("--password", out string password);
            parameters.TryGetValue("--path", out string path);

            MirthHttpClient mirthHttpClient = new MirthHttpClient("https://" + server + "/api", username, password);
            FolderManagerXML folderManager = new FolderManagerXML();

            var codeTemplatesResponse = mirthHttpClient.GetAsync(Endpoints.CodeTemplates).Result;
            var librariesResponse = mirthHttpClient.GetAsync(Endpoints.CodeTemplatesLibrary).Result;

            if (codeTemplatesResponse.StatusCode == System.Net.HttpStatusCode.OK &&
                librariesResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var codeTemplates = codeTemplatesResponse.Content.ReadAsStringAsync().Result;
                var libraries = librariesResponse.Content.ReadAsStringAsync().Result;
                try
                {
                    folderManager.WriteTemplates(codeTemplates, path);
                    folderManager.WriteLibraries(libraries, path);

                    folderManager.OrganizeFolder(path);
                    output = $"Files written to {path}.";
                }
                catch (Exception ex)
                {
                    output = $"An error ocurred while writting file to {path}\n" +
                        $"InnerException:{ex}";
                }
            }
            else
                return $"Response: {codeTemplatesResponse.ReasonPhrase}.";

            return output;
        }
    }
}
