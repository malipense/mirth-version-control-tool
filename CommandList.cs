using NextGen.Cli.Commands;
using NextGen.Cli.Interfaces;

namespace NextGen.Cli
{
    internal class CommandList
    {
        public static ICommand[] commands =
        {
            new Help(),
            new MirthPull(),
            new Exit(),
            new GitCommit(),
            new GitList()
        };
    }
}
