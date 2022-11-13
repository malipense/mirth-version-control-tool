using NextGen.Cli.Commands;
using NextGen.Cli.Interfaces;

namespace NextGen.Cli
{
    internal class CommandList
    {
        public static ICommand[] commands =
        {
            new Help(),
            new PullChannels(),
            new PullLibrary(),
            new Exit(),
            new Clear()
        };
    }
}
