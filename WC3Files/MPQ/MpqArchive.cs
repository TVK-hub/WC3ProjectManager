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
        public bool Opened
        {
            get;
            private set;
        } = false;
        public static MpqArchive Load(string path)
        {
            MpqArchive arc = new MpqArchive();
            if (!arc.TryOpen(path))
            {
                throw new Exception($"Не удались открыть файл \"{path}\".");
            }
            return arc;
        }
        public bool TryOpen(string path)
        {
            Close();
            IntPtr ptr;
            Opened = StormLib.SFileOpenArchive(path, 0u, 0u, out ptr);
            Ptr = ptr;
            return Opened;
        }

        //Закрыть
        public void Close()
        {
            if (Opened) { StormLib.SFileCloseArchive(Ptr); }
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
        public bool DeleteFile(string path)
        {
            return StormLib.SFileRemoveFile(Ptr, path, 0u);
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
