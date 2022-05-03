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
            Directory.CreateDirectory($"../../../remote/Libraries");
            Directory.CreateDirectory($"../../../remote/Channels");
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

        public void WriteChannelGroups(string jsonContent)
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
                var channelId = singleChannelGroupObject.List.channelGroup.Id;
                Directory.CreateDirectory($"../../../remote/Channels/{channelName}");
                _channelGroupsWrapper.Add(new GroupWrapper(channelName, channelId));
            }
            else
            {
                foreach(var channelGroup in channelGroupListObject.List.channelGroupList)
                {
                    var channelName = channelGroup.Name;
                    var channelId = channelGroup.Id;
                    Directory.CreateDirectory($"../../../remote/Channels/{channelName}");
                    _channelGroupsWrapper.Add(new GroupWrapper(channelName, channelId));
                }
            }
            
            Console.WriteLine($"Saving data on /remote/Channels...");
        }

        public void WriteChannels(string jsonContent)
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
                    File.WriteAllText($"../../../remote/Channels/{name}.json", channelJson);
                }
            }
            else
                channelJson = channelChildList.ToString();
        }

        public void OrganizeChannels()
        {
            //Console.WriteLine($"Organizing channels...");
            //XmlDocument document = new XmlDocument();
            //var files = Directory.EnumerateFiles("../../../remote/Channels", "*.xml").ToArray();

            //for (var i = 0; i < files.Length; i++)
            //{
            //    var fileName = Path.GetFileName(files[i]);
            //    document.Load(files[i]);
            //    var channelId = document.GetElementsByTagName("id")[0].InnerText;
            //    foreach (var wrapper in _channelGroupsWrapper)
            //    {
            //        if (wrapper.id == channelId)
            //        {
            //            if (!File.Exists($"../../../remote/Channels/{wrapper.groupName}/{fileName}"))
            //                File.Move(files[i], $"../../../remote/Channels/{wrapper.groupName}/{fileName}");
            //        }
            //    }
            //}
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
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
