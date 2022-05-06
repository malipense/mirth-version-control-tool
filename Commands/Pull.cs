using System;
using System.Collections.Generic;

namespace version_control_tool.Commands
{
    public class Pull : ICommand
    {
        private string[] _parameters = new string[6]
        {
            "-server",
            "-username",
            "-password",
            "-path",
            "-dataType",
            "-resource"
        };
        public string Name => "pull";
        public string[] Parameters => _parameters;
        public string CallBack()
        {
            return "This command requires parameters";
        }
        public string CallBack(Dictionary<string, string> parameters)
        {
            string output = null;
            string server = null;
            string username = null;
            string password = null;
            string path = null;
            string resource = null;
            
            parameters.TryGetValue("-server", out server);
            parameters.TryGetValue("-username", out username);
            parameters.TryGetValue("-password", out password);
            parameters.TryGetValue("-path", out path);
            parameters.TryGetValue("-resource", out resource);

            ApiHttpClient apiHttpClient = new ApiHttpClient(server, username, password);
            string channels = null;
            string channelGroups = null;
            
            channels = apiHttpClient.GetAsync(Endpoints.Channels).Result;
            channelGroups = apiHttpClient.GetAsync(Endpoints.ChannelGroups).Result;

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
