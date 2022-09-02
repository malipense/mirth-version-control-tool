using NextGen.Cli.Commands;
using NextGen.Cli.Interfaces;

namespace NextGen.Cli
{
    internal static class CommandList
    {
        public static ICommand[] commands =
        {
            new Help(),
            new MirthPull(),
            new Exit(),
            new GitCommit()
        };
    }
}
