using System.IO;
using System.Xml;

namespace version_control_tool
{
    class Program
    {
        static void Main(string[] args)
        {
            //should receive these parameters
            string username = "admin";
            string password = "admin";
            string baseUrl = "https://localhost:8443/api";
            string operation = "pull";

            //login
            Request request = new Request();
            string encodedLogin = $"username={username}&password={password}";
            request.CreateRequest("POST", baseUrl + URL.Login, encodedLogin);

            if (operation == "pull")
                PullDataFromMirth(request, baseUrl);
            else if (operation == "push")
                PushDataIntoMirth();

        }

        private static void PushDataIntoMirth()
        {
            throw new System.NotImplementedException();
        }

        private static void PullDataFromMirth(Request request, string baseUrl)
        {
            var groups = request.CreateRequest("GET", baseUrl + URL.ChannelGroups, null);
            var groupsXml = new StreamReader(groups.GetResponseStream()).ReadToEnd();

            var channels = request.CreateRequest("GET", baseUrl + URL.ChannelGroups, null);
            var channelsXml = new StreamReader(channels.GetResponseStream()).ReadToEnd();

            //Write to directory
            WriteFileToDirectory(groupsXml);
            WriteFileToDirectory(channelsXml);
        }
        static void WriteFileToDirectory(string xml)
        {
            string fileName = "";
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            Directory.CreateDirectory("../../../remote");
            document.Save($"../../../remote/{fileName}.xml");
        }
    }

}
