namespace version_control_tool
{
    public class Wrapper
    {
        public Wrapper(string groupName, string id)
        {
            this.groupName = groupName;
            this.id = id;
        }
        public string groupName { get; }
        public string id { get; }
    }
}
