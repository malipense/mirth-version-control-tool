using NextGen.Cli.Commands;

namespace NextGen.Cli
{
    internal static class CommandList
    {
        public static ICommand[] commands =
        {
            //new Command("pull", new string[] {"server", "username", "password"}, Do)
            new Help(),
            new OutputFile(),
            new Pull(),
            new Exit()
        };
    }
}
