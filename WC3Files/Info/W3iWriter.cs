using System.Text;
namespace WC3Files.Info
{
    public class W3iWriter
    {
        //Райтер
        WC3Writer Writer;

        //Конструктор
        public W3iWriter(MemoryStream stream)
        {
            Writer = new WC3Writer(stream);
        }

        //Парсинг
        public void WriteFile(W3iFile wct)
        {
        }
    }
}
