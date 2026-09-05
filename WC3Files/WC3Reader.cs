using System.Text;
namespace WC3Files
{
    public class WC3Reader : BinaryReader
    {
        //Конструктор
        public WC3Reader(byte[] data) : base(new MemoryStream(data))
        {

        }

        //Строка C
        public string ReadFxString(int len)
        {
            byte[] data = ReadBytes(len);
            return Encoding.UTF8.GetString(data);
        }
        public string ReadCString()
        {
            List<byte> bytes = new();
            byte b;
            while ((b = ReadByte()) != 0)
            {
                bytes.Add(b);
            }
            return Encoding.UTF8.GetString(bytes.ToArray());
        }
    }
}
