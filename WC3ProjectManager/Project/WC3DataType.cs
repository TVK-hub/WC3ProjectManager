namespace WC3ProjectManager.Project
{
    [Flags]
    public enum WC3DataType
    {
        Objects  = 1 << 0,
        Triggers = 1 << 1,
        Import   = 1 << 2,
        Strings  = 1 << 3,

        All = Objects | Triggers | Import | Strings
    }
}
