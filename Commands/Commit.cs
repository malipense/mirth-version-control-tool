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
            var output = githubClient.GetAsync("/search/repositories?q=user:malipense");
            //repos/malipense/test
            //search/repositories?q=user:malipense
            //user/repos - auth user
            ///repos/malipense/mirth-version-controll-tool = if private repo user needs to be auth
            return output.Result;
        }

        public string Execute(Dictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }

    }
}
