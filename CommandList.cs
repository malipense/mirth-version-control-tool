using NextGen.Cli.Commands;

namespace NextGen.Cli
{
    internal static class CommandList
    {
        public static ICommand[] commands =
        {
            new Help(),
            new OutputFile(),
            new MirthPull(),
            new Exit(),
            new Commit()
        };
    }
}
