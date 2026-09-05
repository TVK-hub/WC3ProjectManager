using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.MPQ
{
    public class MpqArchive : IDisposable
    {
        //Указатель
        public IntPtr Ptr
        {
            get;
            private set;
        }

        //Открыть
        public MpqArchive(IntPtr ptr)
        {
            Ptr = ptr;
        }
        public static MpqArchive Open(string path)
        {
            IntPtr ptr; StormLib.SFileOpenArchive(path, 0u, 0u, out ptr);
            MpqArchive mpq = new MpqArchive(ptr);
            return mpq;
        }

        //Закрыть
        public void Close()
        {
            StormLib.SFileCloseArchive(Ptr);
        }
        public void Dispose()
        {
            Close();
        }

        //Сжать
        public void Compact()
        {
            StormLib.SFileCompactArchive(Ptr, null, false);
        }

        //Файл
        public MpqFile OpenFile(string path)
        {
            return MpqFile.Open(this, path);
        }
        public void CreateFile(string path, byte[] data)
        {
            MpqFile.Create(this, path, data);
        }
        //public bool RemoveFile(string path)
        //{
        //    return StormLib.SFileRemoveFile(Ptr, path, 0u);
        //}
        //public bool AddFile(string pathInDisk, string pathInMpq)
        //{
        //    return StormLib.SFileAddFileEx(Ptr, pathInDisk, pathInMpq, 0x80000200u, 0x2u, 0x2u);
        //}
    }
}
