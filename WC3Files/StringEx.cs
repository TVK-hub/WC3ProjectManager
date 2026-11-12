namespace WC3Files
{
    public static class StringEx
    {
        public static string ToFileName(this string str)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.TrimEnd(' ', '.');
        }
        public static bool IsUpper(this string str)
        {
            bool res = str.All(char.IsUpper);
            return res;
        }
        public static bool IsUpperSub(this string str, int inx, int len)
        {
            bool res = str[inx..len].IsUpper();
            return res;
        }
    }
}
