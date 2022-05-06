using version_control_tool.Commands;

namespace version_control_tool
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
