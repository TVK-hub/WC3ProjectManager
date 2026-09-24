namespace WC3ProjectManager
{
    [Flags]
    public enum WC3DataType
    {
        Objects  = 1 << 0,
        Triggers = 1 << 1,
        Import   = 1 << 2,

        All = Objects | Triggers | Import
    }
}
