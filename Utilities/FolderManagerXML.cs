using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NextGen.Cli
{
    public class FolderManagerXML
    {
        private List<Wrapper> _wrapper;
        public FolderManagerXML()
        {
            _wrapper = new List<Wrapper>();
        }
        public void WriteLibraries(string xmlContent, string path)
        {
            XmlDocument librariesDocument = new XmlDocument();
            var xDocLibraries = XDocument.Parse(xmlContent);
            var xElementsLibraries = xDocLibraries.Root.Elements().ToArray();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Console.WriteLine($"Saving data on {path}...");
            
            for (int i = 0; i < xElementsLibraries.Length; i++)
            {
                librariesDocument.LoadXml(xElementsLibraries[i].ToString());
                var libraryName = librariesDocument.GetElementsByTagName("name")[0].InnerText;

                Directory.CreateDirectory($"{path}/{libraryName}");
                librariesDocument.Save($"{path}/{libraryName}.xml");

                foreach (XmlNode node in librariesDocument.GetElementsByTagName("id"))
                {
                    _wrapper.Add(new Wrapper(libraryName, node.InnerText));
                }
            }
        }
        public void WriteTemplates(string xmlContent, string path)
        {
            XmlDocument codeTemplateDocument = new XmlDocument();
            var xDocTemplates = XDocument.Parse(xmlContent);
            var xElementsTemplates = xDocTemplates.Root.Elements().ToArray();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            for (int i = 0; i < xElementsTemplates.Length; i++)
            {
                codeTemplateDocument.LoadXml(xElementsTemplates[i].ToString());
                var name = codeTemplateDocument.GetElementsByTagName("name")[0].InnerText;
                codeTemplateDocument.Save($"{path}/{name}.xml");
            }
        }

        public void WriteChannelGroups(string xmlContent, string path)
        {
            XmlDocument groupsDocument = new XmlDocument();
            var xDocGroups = XDocument.Parse(xmlContent);
            var xElementsGroups = xDocGroups.Root.Elements().ToArray();

            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            Console.WriteLine($"Saving data on {path}...");

            for (int i = 0; i < xElementsGroups.Length; i++)
            {
                groupsDocument.LoadXml(xElementsGroups[i].ToString());
                var groupName = groupsDocument.GetElementsByTagName("name")[0].InnerText;

                Directory.CreateDirectory($"{path}/{groupName}");
                groupsDocument.Save($"{path}/{groupName}.xml");

                foreach (XmlNode node in groupsDocument.GetElementsByTagName("id"))
                {
                    _wrapper.Add(new Wrapper(groupName, node.InnerText));
                }       
            }
        }

        public void WriteChannels(string xmlContent, string path)
        {
            XmlDocument channelsDocument = new XmlDocument();
            var xDocChannels = XDocument.Parse(xmlContent);
            var xElementsChannels = xDocChannels.Root.Elements().ToArray();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            for (int i = 0; i < xElementsChannels.Length; i++)
            {
                channelsDocument.LoadXml(xElementsChannels[i].ToString());
                var name = channelsDocument.GetElementsByTagName("name")[0].InnerText;
                channelsDocument.Save($"{path}/{name}.xml");
            }
        }

        public void OrganizeFolder(string path)
        {
            if (_wrapper.Count > 0)
            {
                Console.WriteLine($"Organizing channels...");
                XmlDocument document = new XmlDocument();
                var files = Directory.EnumerateFiles($"{path}", "*.xml").ToArray();

                for (var i = 0; i < files.Length; i++)
                {
                    var fileName = Path.GetFileName(files[i]);
                    document.Load(files[i]);
                    var channelId = document.GetElementsByTagName("id")[0].InnerText;
                    foreach (var wrapper in _wrapper)
                    {
                        if (wrapper.Id == channelId)
                        {
                            if (!File.Exists($"{path}/{wrapper.Name}/{fileName}"))
                                File.Move(files[i], $"{path}/{wrapper.Name}/{fileName}");
                        }
                    }
                }
            }
        }
    }
}
