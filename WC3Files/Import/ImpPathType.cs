namespace WC3Files
{
    [Flags]
    public enum ImpFlags : byte
    {
        None = 0x00,

        FullPath = 0x01,
        NullEntry = 0x02,

        Unknown04 = 0x04,
        Unknown08 = 0x08,
        Unknown10 = 0x10
    }
}