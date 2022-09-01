using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APIClient;

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
            "--filePath"
        };

        public string Name => "commit";

        public string Description => "Creates and commits changes to a github repository.";

        public string[] Parameters => _parameters;

        public string Execute()
        {
            string token = null;
            if (string.IsNullOrEmpty(token))//does this makes sense?
                token = Environment.GetEnvironmentVariable("GIT_TOKEN");
            string repo = "testprivate";
            string user = "malipense";
            //string token = "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";
            string filePath = "C:\\dev\\tests";

            string fileName = "notes/" + Path.GetFileName(Directory.GetFiles(filePath).First());
            
            var bytes = File.ReadAllBytes(Directory.GetFiles(filePath).First());
            var content = Convert.ToBase64String(bytes);

            if (string.IsNullOrEmpty(token))//does this makes sense?
                Environment.GetEnvironmentVariable("GIT_TOKEN");

            GithubClient githubClient = new GithubClient("https://api.github.com", token);

            var output = githubClient.PutAsync($"/repos/{user}/{repo}/contents/{fileName}", new CommitBody(user, repo, fileName, "test commit", content));

            return output.Result;

            //repos/malipense/test
            //search/repositories?q=user:malipense
            //user/repos - auth user
            ///repos/malipense/mirth-version-controll-tool = if private repo user needs to be auth


            /*
             --repo 
            --user
             */

            return "This command requires the parameters to be filled, type help to see information.";
        }

        public string Execute(Dictionary<string, string> parameters)
        {
            string repo = null;
            string user = null;
            string token = null;
            string filePath = null;
            string fileName = null;

            parameters.TryGetValue("--repo", out repo);
            parameters.TryGetValue("--user", out user);
            parameters.TryGetValue("--token", out token);
            parameters.TryGetValue("--filePath", out filePath);

            if (string.IsNullOrEmpty(token))//does this makes sense?
                token = Environment.GetEnvironmentVariable("GIT_TOKEN");

            GithubClient githubClient = new GithubClient("https://api.github.com", token);

            filePath = "C:/dev/tests";
            fileName = Directory.GetFiles(filePath).First();

            var content = Convert.ToBase64String(Encoding.UTF8.GetBytes(filePath));

            var output = githubClient.PutAsync($"/repos/{user}/{repo}/contents/{fileName}", new CommitBody(user, repo, fileName, "test commit", content));

            return output.Result;
        }
    }
}
