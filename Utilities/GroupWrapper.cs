namespace NextGen.Cli
{
    public class GroupWrapper
    {
        public GroupWrapper(string groupName, string id)
        {
            this.groupName = groupName;
            this.id = id;
        }
        public string groupName { get; }
        public string id { get; }
    }
}
