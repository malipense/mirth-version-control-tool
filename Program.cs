using System;
using System.Collections.Generic;
using System.Linq;
using NextGen.Cli.Commands;

namespace NextGen.Cli
{
    class Program
    {
        private static IFolderManager _folderManager;
        static void Main(string[] args)
        {
            HelloUser();

            while(true)
            {
                string[] inputCommandBlocks = Console.ReadLine().Trim().Split('|');
                if (inputCommandBlocks.Length == 0)
                {
                    //no commands
                    Console.WriteLine("Empty arguments... rerun the application filling the required parameters.");
                    return;
                }
                else if (inputCommandBlocks.Length == 1)
                {
                    //one block on execution
                    var executionResult = TryExecuteCommand(inputCommandBlocks[0]);

                    Console.WriteLine("\n");
                    Console.WriteLine(executionResult);
                }
                else
                {
                    //two or more blocks of execution
                    string chainedResult = null;

                    for (int i = 0; i < inputCommandBlocks.Length; i++)
                    {
                        Console.Clear();
                        if (chainedResult == null)
                        {
                            chainedResult = TryExecuteCommand(inputCommandBlocks[0].ToLower());
                            continue;
                        }
                        chainedResult = TryExecuteCommand(inputCommandBlocks[i].ToLower(), chainedResult);

                        Console.WriteLine("\n");
                        Console.WriteLine(chainedResult);
                    }
                }
            }
        }

        private static bool ValidateArguments(string[] args, string[] targetArgs)
        {
            bool succedeed = false;
            bool found = false;
            var validations = new List<bool>();
            foreach (var arg in args)
            {
                foreach (var target in targetArgs)
                {
                    if (arg == target)
                    {
                        found = true;
                        break;
                    }
                    else
                        found = false;
                }
                if(found)
                    validations.Add(true);
                else
                    validations.Add(false);
            }

            foreach (var items in validations)
            {
                if (!items)
                {
                    succedeed = false;
                    break;
                }
                else
                    succedeed = true;
            }

            return succedeed;
        }
        private static string TryExecuteCommand(string inputCommand, string data = null)
        {
            ICommand command = null;
            Dictionary<string,string> keyValuePairs = new Dictionary<string,string>();
            
            List<string> args = new List<string>();
            List<string> values = new List<string>();

            var commandParametersAndValueList = inputCommand.Trim().Split(' ');

            string commandName = commandParametersAndValueList[0];
            command = CommandList.commands.FirstOrDefault(c => c.Name == commandParametersAndValueList[0].ToLower());
            if(command == null)
                return "Command not found";

            if(commandParametersAndValueList.Length > 1)
            {
                if(data != null)
                {
                    args.Add("-data");
                    values.Add(data);
                }

                for (int i = 1; i < commandParametersAndValueList.Length; i++)
                {
                    if (commandParametersAndValueList[i].StartsWith('-'))
                        args.Add(commandParametersAndValueList[i]);
                    else
                        values.Add(commandParametersAndValueList[i]);
                }
                if (values.Count() == 0 && args.Count() > 0)
                    return "No value provided for the parameter.";

                var valid = ValidateArguments(args.ToArray(), command.Parameters);
                if (!valid)
                    return "Parameter not found";

                for (int i = 0; i < args.Count; i++)
                {
                    keyValuePairs.Add(args[i].ToLower(), values[i].ToLower());
                }

                return command.CallBack(keyValuePairs);
            }
            else
            {
                return command.CallBack();
            }
        }
        
        private static async System.Threading.Tasks.Task PullDataFromMirthAsync()
        {
            //Console.WriteLine($"Pulling Data from API - status:");
            
            //var codeTemplates = await _crud.Get(Endpoints.CodeTemplates);
            //var channelGroups = await _crud.Get(Endpoints.ChannelGroups);
            //var channels = await _crud.Get(Endpoints.Channels);

            //_folderManager.CreateFolders();
            //_folderManager.WriteTemplates(codeTemplates);
            //_folderManager.WriteChannelGroups(channelGroups);
            //_folderManager.WriteChannels(channels);
            //_folderManager.OrganizeChannels();
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
