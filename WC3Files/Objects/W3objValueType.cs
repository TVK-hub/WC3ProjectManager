namespace WC3Files.Objects
{
    public enum W3objValueType
    {
        Int    = 0,
        Real   = 1,
        Unreal = 2,
        String = 3
    }
    public static class W3objValueTypeEx
    {
        public static W3objValueType FromMetaType(string metaType)
        {
            W3objValueType valueType;
            switch (metaType)
            {
                case "int":
                case "bool":
                case "attackBits":
                case "teamColor":
                case "deathType":
                case "detectionType":
                case "defenseTypeInt":
                case "channelType":
                case "channelFlags":
                case "interactionFlags":
                case "morphFlags":
                case "stackFlags":
                case "silenceFlags":
                case "fullFlags":
                case "pickFlags":
                    valueType = W3objValueType.Int;
                    break;
                case "real":
                    valueType = W3objValueType.Real;
                    break;
                case "unreal":
                    valueType = W3objValueType.Unreal;
                    break;
                default:
                    valueType = W3objValueType.String;
                    break;
            }
            return valueType;
        }
    }
}
