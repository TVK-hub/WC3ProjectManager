using System.Globalization;
using WC3Files.Metadata.Objects;

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
                case "teamColor":
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
