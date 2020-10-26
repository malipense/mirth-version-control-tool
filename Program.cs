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
        static void Main(string[] args)
        {
            string username = "";
            string password = "";
            string baseUrl = "";
            string operation = "";
            string id = "";
            string fileName = "";

            if (args.Length == 0)
                Console.WriteLine("Empty arguments");

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-server":
                        baseUrl = args[i + 1];
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
                    case "-channel":
                        fileName = args[i + 1];
                        break;
                }
            }
            operation = args.Last();

            Request request = new Request();
            string encodedLogin = $"username={username}&password={password}";
            request.CreateRequest("POST", baseUrl + URL.Login, encodedLogin, false);

            if (operation == "pull")
                PullDataFromMirth(request, baseUrl);
            else if (operation == "push")
                PushAllChannelsIntoMirth(request, baseUrl, id, fileName);
        }

        private static void PushAllChannelsIntoMirth(Request request, string baseUrl, string id, string name)
        {
            XmlDocument document = new XmlDocument();

            var files = Directory.EnumerateFiles("../../../remote/Channels", "*.xml").ToArray();
            
            for (var i = 0; i < files.Length; i++)
            {
                document.Load(files[i]);
                var idNode = document.GetElementsByTagName("id");
                var channelId = idNode[0].InnerText;
                
                var fileName = files[i];

                if (String.IsNullOrEmpty(id) && String.IsNullOrEmpty(name))
                    request.CreateRequest("PUT", baseUrl + URL.Channels + $"/{channelId}?override=true", document.InnerXml, false);
                else if (id == channelId)
                    request.CreateRequest("PUT", baseUrl + URL.Channels + $"/{channelId}?override=true", document.InnerXml, false);
                else if (fileName.Contains(name))
                    request.CreateRequest("POST", baseUrl + URL.Channels, document.InnerXml, true);

            }
        }

        private static void PullDataFromMirth(Request request, string baseUrl)
        {
            Console.WriteLine($"Pulling data from: {baseUrl}...");
            var groups = request.CreateRequest("GET", baseUrl + URL.ChannelGroups, null, false);
            var groupsXml = new StreamReader(groups.GetResponseStream()).ReadToEnd();

            var channels = request.CreateRequest("GET", baseUrl + URL.Channels, null, false);
            var channelsXml = new StreamReader(channels.GetResponseStream()).ReadToEnd();

            var codeTemplates = request.CreateRequest("GET", baseUrl + URL.CodeTemplates, null, false);
            var codeTemplatesXml = new StreamReader(codeTemplates.GetResponseStream()).ReadToEnd();

         
            WriteChannels(channelsXml);
        }

        //write code templates
        static void WriteFileToDirectory(string xml)
        {
            string fileName = "index";
            string directoryName = "";
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            XmlNodeList xmlNodeList = document.GetElementsByTagName("name");

            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                directoryName = xmlNodeList[i].InnerXml;
            }

            Directory.CreateDirectory($"../../../remote/{directoryName}");
            document.Save($"../../../remote/{directoryName}/{fileName}.xml");
        }

        static void WriteChannels(string xml)
        {
            string directoryName = "Channels";
            Directory.CreateDirectory($"../../../remote/{directoryName}");
            
            XmlDocument document = new XmlDocument();

            var xDoc = XDocument.Parse(xml); // loading source xml
            var xmls = xDoc.Root.Elements().ToArray(); // split into elements

            Console.WriteLine($"Saving data on /remote/{directoryName}...");

            for (int i = 0; i < xmls.Length; i++)
            {
                
                document.LoadXml(xmls[i].ToString());

                var name = document.GetElementsByTagName("name");

                document.Save($"../../../remote/{directoryName}/{name[0].InnerText}.xml");
            }
            Console.WriteLine($"Finished");

        }
    }

}
