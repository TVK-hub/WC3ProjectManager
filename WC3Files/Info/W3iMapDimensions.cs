using System.Drawing;
namespace WC3Files.Info
{
    public class W3iMapDimensions
    {
        public PointF[] CameraBounds
        {
            set; get;
        } = new PointF[4];
        public int[] CameraBoundsComplements
        {
            set; get;
        } = new int[4];
        public int PlayableWidth
        {
            set; get;
        }
        public int PlayableHeight
        {
            set; get;
        }
        public int Flags
        {
            set; get;
        }
        public char Tileset
        {
            set; get;
        }
    }
}
