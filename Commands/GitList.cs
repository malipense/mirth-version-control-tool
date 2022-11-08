using System;
using System.Collections.Generic;
using NextGen.Cli.Interfaces;
using APIClient;
using NextGen.Cli.Commands.Exceptions;

namespace NextGen.Cli.Commands
{
    internal class GitList: ICommand
    {
        private List<Option> _options = new List<Option>
        {
            new Option("--user", "github user id"),
            new Option("--token", "github access token, required for private repositories")
        };

        public string Name => "GITLIST";

        public string Description => "retrieves a list user's repositories. Requires authentication for private repos.";

        public List<Option> Options => _options;

        public string Execute()
        {
           return ExceptionMessages.RequiredParameters;
        }

        public string Execute(IDictionary<string, string> parameters)
        {
            parameters.TryGetValue("--token", out string token);

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine(ExceptionMessages.GitTokenWarning);
                token = Environment.GetEnvironmentVariable("GIT_TOKEN");
            }

            if (string.IsNullOrEmpty(token))
                return ExceptionMessages.MissingGitToken;

            GithubClient githubClient = new GithubClient("https://api.github.com", token);
            var output = githubClient.GetAsync($"/user/repos");

            return output.Result;
        }
    }
}
