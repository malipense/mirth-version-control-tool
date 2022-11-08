using System;
using System.Collections.Generic;

namespace NextGen.Cli.Interfaces
{
    public interface ICommand
    {
        public string Name { get; }
        public string Description { get; }
        public List<Option> Options { get; }
        public string Execute();
        public string Execute(IDictionary<string, string> parameters);
    }
}
