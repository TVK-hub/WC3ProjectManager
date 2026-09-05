using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files
{
    public class WC3Writer : BinaryWriter
    {
        //Конструктор
        public WC3Writer(MemoryStream stream) : base(stream)
        {

        }

        //Строка C
        public void WriteFxString(string s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s, 0, s.Length);
            Write(data);
        }
        public void WriteCString(string s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s, 0, s.Length);
            Write(data);
            Write((byte)0);
        }
    }
}
