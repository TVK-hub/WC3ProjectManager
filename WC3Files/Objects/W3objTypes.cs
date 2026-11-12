namespace WC3Files.Objects
{
    [Flags]
    public enum W3objTypes
    {
        Unit         = 1 << 1,
        Item         = 1 << 2,
        Ability      = 1 << 3,
        Buff         = 1 << 4,
        Upgrade      = 1 << 5,
        Doodad       = 1 << 6,
        Destructable = 1 << 7,
        All = Unit | Ability | Item | Buff | Upgrade | Doodad | Destructable,
    }
}
