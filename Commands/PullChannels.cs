using NextGen.Cli.Interfaces;
using System;
using System.Collections.Generic;
using NextGen.Cli.Commands.Exceptions;

namespace NextGen.Cli
{
    public class PullChannels : ICommand
    {
        private List<Option> _options = new List<Option>
        {
            new Option("--server", "server's ip adress/hostname and port. Ex: 192.168.10.15:8443"),
            new Option("--username", "a valid mirth user"),
            new Option("--password", "mirth user's password"),
            new Option("--path", "path to save the exported files")
        };
        public string Name => "PULLCHANNELS";
        public string Description => "pulls channels from remote NextGen/Mirth server. Saves it to the provided path as xml.";
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

            var channelsResponse = mirthHttpClient.GetAsync(Endpoints.Channels).Result;
            var channelGroupsResponse = mirthHttpClient.GetAsync(Endpoints.ChannelGroups).Result;

            if (channelsResponse.StatusCode == System.Net.HttpStatusCode.OK &&
                channelGroupsResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var channels = channelsResponse.Content.ReadAsStringAsync().Result;
                var channelGroups = channelGroupsResponse.Content.ReadAsStringAsync().Result;
                try
                {
                    folderManager.WriteChannelGroups(channelGroups, path);
                    folderManager.WriteChannels(channels, path);

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
                return $"Response: {channelsResponse.ReasonPhrase}.";

            return output;
        }
    }
}
