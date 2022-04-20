using System;
using System.Linq;

namespace version_control_tool
{
    class Program
    {
        private static CRUDClient _crud;
        private static IFolderManager _folderManager;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            HelloUser();
            string username = "";
            string password = "";
            string baseURL = "";
            string operation = "";

            
            if (args.Length == 0)
            {
                Console.WriteLine("Empty arguments... rerun the application filling the required parameters.");
                return;
            }

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
                }
            }
            operation = args.Last();
            
            _crud = new CRUDClient(baseURL, username, password);
            _folderManager = new FolderManagerJSON();

            if (operation == "pull")
                await PullDataFromMirthAsync();

            Console.ReadKey();
            
        }

        private static async System.Threading.Tasks.Task PullDataFromMirthAsync()
        {
            Console.WriteLine($"Pulling Data from API - status:");
            
            var codeTemplates = await _crud.Get(Endpoints.CodeTemplates);
            var channelGroups = await _crud.Get(Endpoints.ChannelGroups);
            var channels = await _crud.Get(Endpoints.Channels);

            _folderManager.CreateFolders();
            _folderManager.WriteTemplates(codeTemplates);
            _folderManager.WriteChannelGroups(channelGroups);
            _folderManager.WriteChannels(channels);
            _folderManager.OrganizeChannels();
        }

        #region
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
        */


        #endregion
        private static void HelloUser()
        {
            string message = "";
            message += "********************************************************************************************\n";
            message += "********************************************************************************************\n";
            message += "********************************************************************************************\n";
            message += "********************************************************************************************\n";
            message += "******##*******##**#########**##********#####*******####******##*******##**#########********\n";
            message += "******##**##***##**##*********##*******##***##****##****##****####***####**##***************\n";
            message += "******##*##*##*##**######*****##*******##*******##********##**##*##*##*##**######***********\n";
            message += "******####***####**##*********##*******##***##****##****##****##**##***##**##***************\n";
            message += "******##******##***#########**#######***#####*******####******##*******##**#########********\n";
            message += "********************************************************************************************\n";
            message += "********************************************************************************************\n";
            message += "****************************╔══╗************************************************************\n";
            message += "****************************╚╗╔╝************************************************************\n";
            message += "****************************╔╝( `v´ )*******************************************************\n";
            message += "****************************╚══`.¸.[You!]***************************************************\n";
            message += "********************************************************************************************\n";
            message += "********************************************************************************************\n";

            Console.WriteLine(message);
        }
    }
}
