using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NextGen.Cli
{
    public class FolderManagerXML : IFolderManager
    {
        private List<GroupWrapper> _channelGroupsWrapper;
        public FolderManagerXML()
        {
            _channelGroupsWrapper = new List<GroupWrapper>();
        }

        public void CreateFolders()
        {
            Directory.CreateDirectory($"../../../remote/Libraries");
            Directory.CreateDirectory($"../../../remote/Channels");
        }

        public void WriteTemplates(string xmlContent)
        {
            XmlDocument codeTemplateDocument = new XmlDocument();
            var xDocFile = XDocument.Parse(xmlContent);
            var xDocElements = xDocFile.Root.Elements().ToArray();

            for (int i = 0; i < xDocElements.Length; i++)
            {
                codeTemplateDocument.LoadXml(xDocElements[i].ToString());
                var name = codeTemplateDocument.GetElementsByTagName("name")[0].InnerText;

                Console.WriteLine($"Saving data on /remote/Libraries/{name}...");

                Directory.CreateDirectory($"../../../remote/Libraries/{name}");
                codeTemplateDocument.Save($"../../../remote/Libraries/{name}/{name}.xml");
            }
        }

        public void WriteChannelGroups(string xmlContent, string path = "../../../remote")
        {
            XmlDocument groupsDocument = new XmlDocument();

            var xDocGroups = XDocument.Parse(xmlContent);
            var xElementsGroups = xDocGroups.Root.Elements().ToArray();

            Console.WriteLine($"Saving data on {path}/Channels...");

            for (int i = 0; i < xElementsGroups.Length; i++)
            {
                groupsDocument.LoadXml(xElementsGroups[i].ToString());
                var folderName = groupsDocument.GetElementsByTagName("name")[0].InnerText;

                Directory.CreateDirectory($"{path}/Channels/{folderName}");
                
                foreach (XmlNode node in groupsDocument.GetElementsByTagName("id"))
                {
                    _channelGroupsWrapper.Add(new GroupWrapper(folderName, node.InnerText));
                }
                
            }
        }

        public void WriteChannels(string xmlContent, string path = "../../../remote")
        {
            XmlDocument channelsDocument = new XmlDocument();

            var xDocChannels = XDocument.Parse(xmlContent);
            var xElementsChannels = xDocChannels.Root.Elements().ToArray();

            for (int i = 0; i < xElementsChannels.Length; i++)
            {
                channelsDocument.LoadXml(xElementsChannels[i].ToString());
                var name = channelsDocument.GetElementsByTagName("name")[0].InnerText;
                channelsDocument.Save($"{path}/Channels/{name}.xml");
            }
        }

        public void OrganizeChannels(string path)
        {
            Console.WriteLine($"Organizing channels...");
            XmlDocument document = new XmlDocument();
            var files = Directory.EnumerateFiles($"{path}/Channels", "*.xml").ToArray();

            for (var i = 0; i < files.Length; i++)
            {
                var fileName = Path.GetFileName(files[i]);
                document.Load(files[i]);
                var channelId = document.GetElementsByTagName("id")[0].InnerText;
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
}
