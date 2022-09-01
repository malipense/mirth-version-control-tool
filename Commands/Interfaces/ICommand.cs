using System;
using System.Collections.Generic;

namespace NextGen.Cli.Interfaces
{
    public interface ICommand
    {
        public string Name { get; }
        public string Description { get; }
        public string[] Parameters { get; }
        public string Execute();
        public string Execute(Dictionary<string, string> parameters);
    }
}
