namespace WC3Files
{
    public class ImpEntry
    {
        public string Path
        {
            get;
            set;
        } = "";
        public ImpFlags Flags
        {
            get;
            set;
        }
        public string GetFullPath()
        {
            string path = Path;
            if (!Flags.HasFlag(ImpFlags.FullPath))
            {
                path = "war3mapImported\\" + path;
            }
            return path;
        }
        public void SetFullPath(string path)
        {
            string defaultDir = "war3mapImported\\";
            if (path.StartsWith(defaultDir))
            {
                path = path.Substring(defaultDir.Length);
                Flags &= ~ImpFlags.FullPath;
            }
            else
            {
                Flags |= ImpFlags.FullPath;
            }
            Path = path;
        }
    }
}
