using System;
using System.Collections.Generic;
using System.Text;
using APIClient;

namespace NextGen.Cli.Commands
{
    internal class ListRepository: ICommand
    {
        private string[] _parameters = new string[2]
        {
            "--user",
            "--token"
        };

        public string Name => "commit";

        public string Description => "Creates and commits changes to a github repository.";

        public string[] Parameters => _parameters;

        public string Execute()
        {
            //repos/malipense/test
            //search/repositories?q=user:malipense
            //user/repos - auth user
            ///repos/malipense/mirth-version-controll-tool = if private repo user needs to be auth

            return "This command requires the parameters to be filled, type help to see information.";
        }

        public string Execute(Dictionary<string, string> parameters)
        {
            var localToken = "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";

            string user = null;
            string token = null;

            parameters.TryGetValue("--user", out user);
            parameters.TryGetValue("--token", out token);
            
            if (string.IsNullOrEmpty(token))//does this makes sense?
                Environment.GetEnvironmentVariable("GIT_TOKEN");

            GithubClient githubClient = new GithubClient("https://api.github.com", localToken);
            var output = githubClient.GetAsync($"/repos/{user}");

            return output.Result;
        }
    }
}
