using System;
using System.Collections.Generic;
using System.Text;

namespace APIClient
{
    public class CommitBody
    {
        public CommitBody(string owner, string repo, string path, string message, string content)
        {
            Owner = owner;
            Repo = repo;
            Path = path;
            Message = message;
            Content = content;
        }
        public string Owner { get; set; }
        public string Repo { get; set; }
        public string Path { get; set; }
        public string Message { get; set; }
        public string Content { get; set; }
    }
}
