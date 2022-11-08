using System;

namespace NextGen.Cli.Interfaces
{
    public class Option
    {
        public Option(string name, string description)
        {
            Name = name;
            Description = description; 
        }
        public string Name;
        public string Description;
    }
}
