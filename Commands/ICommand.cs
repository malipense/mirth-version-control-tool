using System;
using System.Collections.Generic;

namespace version_control_tool.Commands
{
    public interface ICommand
    {
        public string Name { get; }
        public string[] Parameters { get; }
        public string CallBack();
        public string CallBack(Dictionary<string, string> parameters);
    }
}
