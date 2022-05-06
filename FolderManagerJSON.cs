using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace version_control_tool
{
    public class FolderManagerJSON : IFolderManager
    {
        private List<GroupWrapper> _channelGroupsWrapper;
        public FolderManagerJSON()
        {
            _channelGroupsWrapper = new List<GroupWrapper>();
        }

        public void CreateFolders()
        {
            throw new NotImplementedException();
            //Directory.CreateDirectory($"../../../remote/Libraries");
            //Directory.CreateDirectory($"../../../remote/Channels");
        }

        public void WriteTemplates(string jsonContent)
        {
            //XmlDocument codeTemplateDocument = new XmlDocument();
            //var xDocFile = XDocument.Parse(xmlContent);
            var jsonObject = JObject.Parse(jsonContent);
            var listValue = jsonObject["list"].Value<string>();
            var test = jsonObject["list"];
            if (listValue == null)
            {
                Console.WriteLine("Skipping creation of templates");
                return;
            }
            //var xDocElements = xDocFile.Root.Elements().ToArray();
            


            //for (int i = 0; i < xDocElements.Length; i++)
            //{
            //    codeTemplateDocument.LoadXml(xDocElements[i].ToString());
            //    var name = codeTemplateDocument.GetElementsByTagName("name")[0].InnerText;

            //    Console.WriteLine($"Saving data on /remote/Libraries/{name}...");

            //    Directory.CreateDirectory($"../../../remote/Libraries/{name}");
            //    codeTemplateDocument.Save($"../../../remote/Libraries/{name}/{name}.xml");
            //}
        }

        public void WriteChannelGroups(string jsonContent, string path = "../../../remote/Channels")
        {
            ChannelGroupObject? channelGroupListObject = null;
            SingleChannelGroupObject? singleChannelGroupObject = null;

            try
            {
                channelGroupListObject = JsonSerializer.Deserialize<ChannelGroupObject>(jsonContent);
            }
            catch(Exception ex)
            {
                Console.WriteLine("There is only a single channel group - object is not a list.");
                singleChannelGroupObject = JsonSerializer.Deserialize<SingleChannelGroupObject>(jsonContent);
            }

            if(channelGroupListObject is null)
            {
                var channelName = singleChannelGroupObject.List.channelGroup.Name;
                var channelId = singleChannelGroupObject.List.channelGroup.Channels.Channel.Id;
                Directory.CreateDirectory($"{path}/Channels/{channelName}");
                _channelGroupsWrapper.Add(new GroupWrapper(channelName, channelId));
            }
            else
            {
                foreach(var channelGroup in channelGroupListObject.List.channelGroupList)
                {
                    var channelName = channelGroup.Name;
                    var channelId = channelGroup.Channels.Channel.Id;
                    Directory.CreateDirectory($"{path}/Channels/{channelName}");
                    _channelGroupsWrapper.Add(new GroupWrapper(channelName, channelId));
                }
            }
            
            Console.WriteLine($"Saving data on {path}...");
        }

        public void WriteChannels(string jsonContent, string path = "../../../remote/Channels")
        {
            var jsonObject = JObject.Parse(jsonContent);
            var channelJsonObject = jsonObject["list"].Children().First();
            var channelChildList = channelJsonObject.Children().First();

            string channelJson = "";
            if (channelChildList.Type.ToString() == "Array")
            {
                foreach (var item in channelChildList)
                {
                    var name = item.Value<string>("name");
                    channelJson = item.ToString();
                    File.WriteAllText($"{path}/Channels/{name}.json", channelJson);
                }
            }
            else
                channelJson = channelChildList.ToString();
        }

        public void OrganizeChannels(string path)
        {
            Console.WriteLine($"Organizing channels...");
            
            var files = Directory.EnumerateFiles($"{path}/Channels", "*.json").ToArray();
            
            for (var i = 0; i < files.Length; i++)
            {
                var fileName = Path.GetFileName(files[i]);
                JObject jFile = JObject.Parse(File.ReadAllText($"{path}/Channels/{fileName}"));
                var channelId = jFile.Value<string>("id");
                foreach (var wrapper in _channelGroupsWrapper)
                {
                    if (wrapper.id == channelId)
                    {
                        if (!File.Exists($"{path}/Channels/{wrapper.groupName}/{fileName}"))
                            File.Move(files[i], $"{path}/Channels/{wrapper.groupName}/{fileName}");
                    }
                }
            }
        }

        public void OrganizeChannels()
        {
            throw new NotImplementedException();
        }
    }

    public class ChannelGroupObject
    {
        [JsonPropertyName("list")]
        public ListWithMultipleGroups List { get; set; }
    }
    public class ListWithMultipleGroups
    {
        [JsonPropertyName("channelGroup")]
        public ChannelGroup[] channelGroupList { get; set; }
    }

    public class SingleChannelGroupObject
    {
        [JsonPropertyName("list")]
        public ListWithSingleGroups List { get; set; }
    }
    public class ListWithSingleGroups
    {
        public ChannelGroup channelGroup { get; set; }
    }

    public class ChannelGroup
    {
        [JsonPropertyName("id")]
        public string GroupId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("channels")]
        public Channels Channels { get; set; }
    }
    public class Channels
    {
        [JsonPropertyName("channel")]
        public Channel Channel { get; set; }
    }
    public class Channel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
