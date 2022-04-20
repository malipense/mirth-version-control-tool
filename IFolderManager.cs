namespace version_control_tool
{
    internal interface IFolderManager
    {
        public void WriteTemplates(string content);
        public void WriteChannelGroups(string content);
        public void WriteChannels(string content);
        public void OrganizeChannels();
        public void CreateFolders();
    }
}
