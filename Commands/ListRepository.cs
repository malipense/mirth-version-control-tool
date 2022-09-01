using System;
using System.Collections.Generic;
using NextGen.Cli.Interfaces;
using APIClient;
using version_control_tool.Commands.Exceptions;

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
           return ExceptionMessages.RequiredParameters;
        }

        public string Execute(Dictionary<string, string> parameters)
        {
            string user = null;
            string token = null;

            parameters.TryGetValue("--user", out user);
            parameters.TryGetValue("--token", out token);
            
            if (string.IsNullOrEmpty(token))//does this makes sense?
                token = Environment.GetEnvironmentVariable("GIT_TOKEN");

            if(string.IsNullOrEmpty(token))
                return ExceptionMessages.MissingGitToken;

            GithubClient githubClient = new GithubClient("https://api.github.com", token);
            var output = githubClient.GetAsync($"/repos/{user}");

            return output.Result;
        }
    }
}
