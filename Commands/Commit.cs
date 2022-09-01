using System;
using System.Collections.Generic;
using System.IO;
using APIClient;
using NextGen.Cli.Interfaces;
using version_control_tool.Commands.Exceptions;

namespace NextGen.Cli.Commands
{
    public class Commit : ICommand
    {
        private string[] _parameters = new string[5]
        {
            "--repo",
            "--user",
            "--token",
            "--message",
            "--sourcefilepath"
        };

        public string Name => "commit";

        public string Description => "commits changes to a github repository.";

        public string[] Parameters => _parameters;

        public string Execute()
        {
            //string token = null;
            //if (string.IsNullOrEmpty(token))//does this makes sense?
            //    token = Environment.GetEnvironmentVariable("GIT_TOKEN");
            //string repo = "testprivate";
            //string user = "malipense";
            ////string token = "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";
            //string filePath = "C:\\dev\\tests";

            //string fileName = "notes/" + Path.GetFileName(Directory.GetFiles(filePath).First());
            
            //var bytes = File.ReadAllBytes(Directory.GetFiles(filePath).First());
            //var content = Convert.ToBase64String(bytes);

            //if (string.IsNullOrEmpty(token))//does this makes sense?
            //    Environment.GetEnvironmentVariable("GIT_TOKEN");

            //GithubClient githubClient = new GithubClient("https://api.github.com", token);

            //var output = githubClient.PutAsync($"/repos/{user}/{repo}/contents/{fileName}", new CommitBody(user, repo, fileName, "test commit", content));

            //return output.Result;

            ////repos/malipense/test
            ////search/repositories?q=user:malipense
            ////user/repos - auth user
            /////repos/malipense/mirth-version-controll-tool = if private repo user needs to be auth


            //*
            // --repo 
            //--user
            // */

            return "This command requires the parameters to be filled, type help to see information.";
        }

        public string Execute(Dictionary<string, string> parameters)
        {
            string repo = null;
            string user = null;
            string token = null;
            string sourceFilePath = null;
            string commitMessage = null;
            string remoteFileFullname = null;
            
            parameters.TryGetValue("--repo", out repo);
            parameters.TryGetValue("--user", out user);
            parameters.TryGetValue("--token", out token);
            parameters.TryGetValue("--message", out commitMessage);
            parameters.TryGetValue("--sourcefilepath", out sourceFilePath);

            if (string.IsNullOrEmpty(token))//does this makes sense?
            {
                Console.WriteLine(ExceptionMessages.MissingGitToken);
                token = Environment.GetEnvironmentVariable("GIT_TOKEN");
            }
            if (string.IsNullOrEmpty(token))
                return ExceptionMessages.MissingGitToken;

            var bytes = File.ReadAllBytes(sourceFilePath);
            var content = Convert.ToBase64String(bytes);

            GithubClient githubClient = new GithubClient("https://api.github.com", token);

            var output = githubClient.PutAsync($"/repos/{user}/{repo}/contents/{remoteFileFullname}", 
                new CommitPayload(user, repo, remoteFileFullname, commitMessage, content));

            return output.Result;
        }
    }
}
