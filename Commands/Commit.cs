using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APIClient;

namespace NextGen.Cli.Commands
{
    internal class Commit : ICommand
    {
        public string Name => "commit";

        public string Description => "Creates and commits changes to a github repository.";

        public string[] Parameters => Array.Empty<string>();

        public string Execute()
        {
            GithubClient githubClient = new GithubClient("https://api.github.com", "eduardo.malipense@gmail.com", "gih@38x1*/WDS451xc");
            var output = githubClient.GetAsync("/repositories");
            
            return output.Result;
        }

        public string Execute(Dictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }

    }
}
