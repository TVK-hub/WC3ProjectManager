namespace WC3Files.Objects
{
    [Flags]
    public enum W3objTypes
    {
        None         = 0,
        Unit         = 1 << 1,
        Item         = 1 << 2,
        Ability      = 1 << 3,
        Buff         = 1 << 4,
        Upgrade      = 1 << 5,
        Doodad       = 1 << 6,
        Destructable = 1 << 7,
        All = Unit | Ability | Item | Buff | Upgrade | Doodad | Destructable,
    }
    public static class W3objTypesEx
    {
        public static W3objTypes Parse(string name)
        {
            W3objTypes t = W3objTypes.None;
            switch (name)
            {
                case "Unit":
                    t = W3objTypes.Unit;
                    break;
                case "Item":
                    t = W3objTypes.Item;
                    break;
                case "Ability":
                    t = W3objTypes.Ability;
                    break;
                case "Buff":
                    t = W3objTypes.Buff;
                    break;
                case "Upgrade":
                    t = W3objTypes.Upgrade;
                    break;
                case "Doodad":
                    t = W3objTypes.Doodad;
                    break;
                case "Destructable":
                    t = W3objTypes.Destructable;
                    break;
            }
            return t;
        }
        public static string ToString(W3objTypes t)
        {
            string name = "";
            switch (t)
            {
                case W3objTypes.Unit:
                    name = "Unit";
                    break;
                case W3objTypes.Item:
                    name = "Item";
                    break;
                case W3objTypes.Ability:
                    name = "Ability";
                    break;
                case W3objTypes.Buff:
                    name = "Buff";
                    break;
                case W3objTypes.Upgrade:
                    name = "Upgrade";
                    break;
                case W3objTypes.Doodad:
                    name = "Doodad";
                    break;
                case W3objTypes.Destructable:
                    name = "Destructable";
                    break;
            }
            return name;
        }
    }
}
