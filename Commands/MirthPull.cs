using NextGen.Cli.Interfaces;
using System;
using System.Collections.Generic;
using APIClient;
using NextGen.Cli.Commands.Exceptions;

namespace NextGen.Cli
{
    public class MirthPull : ICommand
    {
        private List<Option> _options = new List<Option>
        {
            new Option("--server", "server's ip adress or hostname"),
            new Option("--username", "a valid mirth user"),
            new Option("--password", "mirth user's password"),
            new Option("--path", "path to save the exported files")
        };
        public string Name => "MIRTHPULL";
        public string Description => "pulls data from remote NextGen/Mirth server. Saves it to the provided path as xml.";
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
            
            MirthHttpsClient mirthHttpClient = new MirthHttpsClient("https://" + server + "/api", username, password);
            
            string channels = mirthHttpClient.GetAsync(Endpoints.Channels).Result;
            string channelGroups = mirthHttpClient.GetAsync(Endpoints.ChannelGroups).Result;

            var folderManager = new FolderManagerXML();
            try
            {
                folderManager.WriteChannelGroups(channelGroups, path);
                folderManager.WriteChannels(channels, path);

                folderManager.OrganizeChannels(path);
                output = $"Files written to {path}.";
            }
            catch (Exception ex)
            {
                output = $"An error ocurred while writting file to {path}\n" +
                    $"InnerException:{ex}";
            }

            return output;
        }
    }
}
