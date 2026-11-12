using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace WC3Files
{
    public class WC3Writer : BinaryWriter
    {
        //Конструктор
        public WC3Writer(MemoryStream stream) : base(stream)
        {

        }

        //Строка C
        public void WriteFxString(string s, int len)
        {
            if (s.Length > len) { s = s[0..len]; }
            while (s.Length < len) { s += "\0"; }
            byte[] data = Encoding.UTF8.GetBytes(s, 0, s.Length);
            Write(data);
        }
        public void WriteFxString(string s)
        {
            WriteFxString(s, s.Length);
        }
        public void WriteCString(string s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s, 0, s.Length);
            if (data.Length > 0) { Write(data); }
            Write((byte)0);
        }
    }
}
