namespace version_control_tool
{
    internal interface IFolderManager
    {
        public void WriteTemplates(string content);
        public void WriteChannelGroups(string content, string path);
        public void WriteChannels(string content, string path);
        public void OrganizeChannels();
        public void CreateFolders();
    }
}
