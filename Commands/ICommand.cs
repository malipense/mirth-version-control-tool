using System;
using System.Collections.Generic;

namespace NextGen.Cli.Commands
{
    public interface ICommand
    {
        public string Name { get; }
        public string[] Parameters { get; }
        public string CallBack();
        public string CallBack(Dictionary<string, string> parameters);
    }
}
