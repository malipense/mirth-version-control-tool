using NextGen.Cli.Interfaces;
using System;
using System.Collections.Generic;
using APIClient;
using version_control_tool.Commands.Exceptions;

namespace NextGen.Cli
{
    public class MirthPull : ICommand
    {
        private string[] _parameters = new string[6]
        {
            "--server",
            "--username",
            "--password",
            "--path",
            "--dataType",
            "--resource"
        };
        public string Name => "mirthpull";
        public string Description => "pulls data from remote NextGen/Mirth server. Saves it to the provided path as xml.";
        public string[] Parameters => _parameters;
        public string Execute()
        {
            return ExceptionMessages.RequiredParameters;
        }
        public string Execute(Dictionary<string, string> parameters)
        {
            string output = null;
            string server = null;
            string username = null;
            string password = null;
            string path = null;
            string resource = null;
            
            parameters.TryGetValue("--server", out server);
            parameters.TryGetValue("--username", out username);
            parameters.TryGetValue("--password", out password);
            parameters.TryGetValue("--path", out path);
            parameters.TryGetValue("--resource", out resource);

            MirthHttpsClient mirthHttpClient = new MirthHttpsClient(server, username, password);
            string channels = null;
            string channelGroups = null;
            
            channels = mirthHttpClient.GetAsync(Endpoints.Channels).Result;
            channelGroups = mirthHttpClient.GetAsync(Endpoints.ChannelGroups).Result;

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
