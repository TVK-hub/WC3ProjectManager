namespace WC3ProjectManager.Project
{
    [Flags]
    public enum WC3DataType
    {
        Config   = 1 << 0,
        Objects  = 1 << 1,
        Triggers = 1 << 2,
        Import   = 1 << 3,
        Strings  = 1 << 4,

        All = Config | Objects | Triggers | Import | Strings
    }
}
