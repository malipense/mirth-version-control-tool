using System;
using System.Collections.Generic;
using System.IO;
using APIClient;
using NextGen.Cli.Interfaces;
using NextGen.Cli.Commands.Exceptions;

namespace NextGen.Cli.Commands
{
    public class GitCommit : ICommand
    {
        private List<Option> _options = new List<Option>()
        {
            new Option("--user", "github user id"),
            new Option("--token", "github access token"),
            new Option("--message", "github commit message"),
            new Option("--sourcefilepath", "file fullname including the path")
        };

        public string Name => "GITCOMMIT";

        public string Description => "commits changes to a github repository.";

        public List<Option> Options => _options;

        public string Execute()
        {
            return ExceptionMessages.RequiredParameters;
        }

        public string Execute(IDictionary<string, string> parameters)
        {
            string remoteFileFullname;
            
            parameters.TryGetValue("--repo", out string repo);
            parameters.TryGetValue("--user", out string user);
            parameters.TryGetValue("--token", out string token);
            parameters.TryGetValue("--message", out string commitMessage);
            parameters.TryGetValue("--sourcefilepath", out string sourceFilePath);

            if (string.IsNullOrEmpty(token))//does this makes sense?
            {
                Console.WriteLine(ExceptionMessages.GitTokenWarning);
                token = Environment.GetEnvironmentVariable("GIT_TOKEN");
            }
            if (string.IsNullOrEmpty(token))
                return ExceptionMessages.MissingGitToken;

            var bytes = File.ReadAllBytes(sourceFilePath);
            var content = Convert.ToBase64String(bytes);
            remoteFileFullname = Path.GetFileName(sourceFilePath);

            GithubClient githubClient = new GithubClient("https://api.github.com", token);

            var output = githubClient.PutAsync($"/repos/{user}/{repo}/contents/{remoteFileFullname}", 
                new CommitPayload(user, repo, remoteFileFullname, commitMessage, content));

            return output.Result;
        }
    }
}
