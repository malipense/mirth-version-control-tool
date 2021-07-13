using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace version_control_tool
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            HelloUser();

            string username = "";
            string password = "";
            string baseURL = "";
            string operation = "";
            string id = "";
            /*
            if (args.Length == 0)
                Console.WriteLine("Empty arguments");

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-server":
                        baseURL = args[i + 1];
                        break;
                    case "-username":
                        username = args[i + 1];
                        break;
                    case "-password":
                        password = args[i + 1];
                        break;
                    case "-id":
                        id = args[i + 1];
                        break;
                }
            }
            operation = args.Last();
            */
            CRUD crud = new CRUD();
            await crud.Post();
            await crud.Get();
            Console.ReadKey();

            /*
            var xmlRequest = new XMLWebRequest(baseURL);
            xmlRequest.Authenticate(username, password);
            
            if (operation == "pull")
                PullDataFromMirth(Connection, baseURl);
            else if (operation == "push")
                PushAllChannelsIntoMirth(Connection, baseURl, id);
            */
        }

      /*
        private static void PushAllChannelsIntoMirth(XMLWebRequest request, string baseUrl, string id)
        {
            XmlDocument document = new XmlDocument();
            var rootFiles = Directory.EnumerateFiles("../../../remote/Channels", "*.xml").ToArray();
            var subDirectories = Directory.GetDirectories("../../../remote/Channels");
            foreach(var sub in subDirectories)
            {
                var inFolderFiles = Directory.EnumerateFiles($"{sub}", "*.xml").ToArray();
                rootFiles = rootFiles.Union(inFolderFiles).ToArray();
            }
            for (var i = 0; i < rootFiles.Length; i++)
            {
                document.Load(rootFiles[i]);
                var idNode = document.GetElementsByTagName("id");
                var channelId = idNode[0].InnerText;
                
                var fileName = Path.GetFileName(rootFiles[i]);

                if (string.IsNullOrEmpty(id))
                {
                    Console.WriteLine($"Pushing {fileName} into {baseUrl}...");
                    request.CreateRequest("PUT", baseUrl + Endpoints.Channels + $"/{channelId}?override=true", document.InnerXml);
                }
                else if (id == channelId)
                {
                    Console.WriteLine($"Pushing {fileName} id {id} into {baseUrl}...");
                    request.CreateRequest("PUT", baseUrl + Endpoints.Channels + $"/{channelId}?override=true", document.InnerXml);
                }
            }
        }

        private static void PullDataFromMirth(XMLWebRequest request, string baseUrl)
        {
            Console.WriteLine($"Pulling data from: {baseUrl}...");
            var groups = request.CreateRequest("GET", baseUrl + Endpoints.ChannelGroups, null);
            var groupsXml = new StreamReader(groups.GetResponseStream()).ReadToEnd();

            var channels = request.CreateRequest("GET", baseUrl + Endpoints.Channels, null);
            var channelsXml = new StreamReader(channels.GetResponseStream()).ReadToEnd();

            var codeTemplatesLibrary = request.CreateRequest("GET", baseUrl + Endpoints.CodeTemplatesLibrary + "?includeCodeTemplates=true", null);
            var codeTemplatesLibraryXml = new StreamReader(codeTemplatesLibrary.GetResponseStream()).ReadToEnd();

            var codeTemplates = request.CreateRequest("GET", baseUrl + Endpoints.CodeTemplates, null);
            var codeTemplatesXml = new StreamReader(codeTemplates.GetResponseStream()).ReadToEnd();

            WriteCodeTemplatesLibraries(codeTemplatesLibraryXml);
            WriteCodeTemplates(codeTemplatesXml);
            WriteChannels(channelsXml, groupsXml);
        }

        private static void WriteCodeTemplatesLibraries(string codeTemplateXml)
        {
            var directoryName = "Libraries";
            Directory.CreateDirectory($"../../../remote/{directoryName}");

            XmlDocument codeTemplateLibraryDocument = new XmlDocument();
            var xDocCodeTemplateLibrary = XDocument.Parse(codeTemplateXml);
            var xmlsCodeTemplateLibrary = xDocCodeTemplateLibrary.Root.Elements().ToArray();

            Console.WriteLine($"Saving data on /remote/{directoryName}...");
            for (int i = 0; i < xmlsCodeTemplateLibrary.Length; i++)
            {
                codeTemplateLibraryDocument.LoadXml(xmlsCodeTemplateLibrary[i].ToString());
                var name = codeTemplateLibraryDocument.GetElementsByTagName("name")[0].InnerText;
                codeTemplateLibraryDocument.Save($"../../../remote/{directoryName}/{name}.xml");
            }
        }

        private static void WriteCodeTemplates(string codeTemplateXml)
        {
            XmlDocument codeTemplateDocument = new XmlDocument();
            var xDocCodeTemplate = XDocument.Parse(codeTemplateXml);
            var xmlsCodeTemplate = xDocCodeTemplate.Root.Elements().ToArray();
           
            for (int i = 0; i < xmlsCodeTemplate.Length; i++)
            {
                codeTemplateDocument.LoadXml(xmlsCodeTemplate[i].ToString());
                var name = codeTemplateDocument.GetElementsByTagName("name")[0].InnerText;
                Console.WriteLine($"Saving data on /remote/Libraries/{name}...");

                Directory.CreateDirectory($"../../../remote/Libraries/{name}");
                codeTemplateDocument.Save($"../../../remote/Libraries/{name}/{name}.xml");
            }
        }

        private static void WriteChannels(string channelsXml, string groupsXml)
        {
            var groupChannelsWrapper = new List<Wrapper>();
            var directoryName = "Channels";
            Directory.CreateDirectory($"../../../remote/{directoryName}");

            XmlDocument channelsDocument = new XmlDocument();
            XmlDocument groupsDocument = new XmlDocument();
            var xDocChannels = XDocument.Parse(channelsXml);
            var xmlsChannels = xDocChannels.Root.Elements().ToArray();
            var xDocGroups = XDocument.Parse(groupsXml); 
            var xmlsGroups = xDocGroups.Root.Elements().ToArray();
            
            Console.WriteLine($"Saving data on /remote/{directoryName}...");
            for (int i = 0; i < xmlsGroups.Length; i++)
            {
                groupsDocument.LoadXml(xmlsGroups[i].ToString());
                var folderName = groupsDocument.GetElementsByTagName("name")[0].InnerText;
                Directory.CreateDirectory($"../../../remote/{directoryName}/{folderName}");
                foreach (XmlNode node in groupsDocument.GetElementsByTagName("id"))
                {
                    groupChannelsWrapper.Add(new Wrapper(folderName, node.InnerText));
                }
            }

            for (int j = 0; j < xmlsChannels.Length; j++)
            {
                channelsDocument.LoadXml(xmlsChannels[j].ToString());
                var name = channelsDocument.GetElementsByTagName("name")[0].InnerText;
                channelsDocument.Save($"../../../remote/{directoryName}/{name}.xml");
            }

            OrganizeChannels(groupChannelsWrapper);
            Console.WriteLine($"Finished");
        }

        private static void OrganizeChannels(List<Wrapper> wrappers)
        {
            Console.WriteLine($"Organizing channels...");
            XmlDocument document = new XmlDocument();
            var files = Directory.EnumerateFiles("../../../remote/Channels", "*.xml").ToArray();

            for (var i = 0; i < files.Length; i++)
            {
                var fileName = Path.GetFileName(files[i]);
                document.Load(files[i]);
                var channelId = document.GetElementsByTagName("id")[0].InnerText;
                foreach (var wrapper in wrappers)
                {
                    if(wrapper.id == channelId)
                    {
                        if(!File.Exists($"../../../remote/Channels/{wrapper.groupName}/{fileName}"))
                            File.Move(files[i], $"../../../remote/Channels/{wrapper.groupName}/{fileName}");
                    }
                }
            }
        }
      */
        private static void HelloUser()
        {
            string message = "";
            message += "**********************************************************************\n";
            message += "**********************************************************************\n";
            message += "*************************╔══╗*****************************************\n";
            message += "*************************╚╗╔╝*****************************************\n";
            message += "*************************╔╝( `v´ )************************************\n";
            message += "*************************╚══`.¸.[You!]********************************\n";
            message += "**********************************************************************\n";
            message += "**********************************************************************\n";
            message += "**********************************************************************\n";

            Console.WriteLine(message);
        }
    }

    sealed class Wrapper
    {
        public Wrapper(string groupName, string id)
        {
            this.groupName = groupName;
            this.id = id;
        }
        public string groupName { get; } 
        public string id { get; }
    }
}
