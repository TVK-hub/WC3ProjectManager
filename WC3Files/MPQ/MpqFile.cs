using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WC3Files.MPQ
{
    /// <summary>
    /// Файл внутри MPQ архива
    /// </summary>
    public class MpqFile : IDisposable
    {
        //Указатель
        public IntPtr Ptr
        {
            get;
            private set;
        }

        //Конструктор
        public MpqFile(IntPtr ptr)
        {
            Ptr = ptr;
        }
        public static MpqFile Open(MpqArchive arch, string path)
        {
            IntPtr ptr; StormLib.SFileOpenFileEx(arch.Ptr, path, 0u, out ptr);
            MpqFile f = new MpqFile(ptr);
            return f;
        }
        public static void Create(MpqArchive arch, string path, byte[] data)
        {
            IntPtr ptr; StormLib.SFileCreateFile(arch.Ptr, path, 0ul, (uint)data.Length, 0u, 0x80000200u, out ptr);
            MpqFile f = new MpqFile(ptr);
            f.Write(data);
            StormLib.SFileFinishFile(f.Ptr);
        }
        public void Close()
        {
            StormLib.SFileCloseFile(Ptr);
        }
        public void Dispose()
        {
            Close();
        }

        //Размер
        public uint GetSize()
        {
            return StormLib.SFileGetFileSize(Ptr, 0u);
        }

        //Чтение
        public byte[] Read()
        {
            //Чтение
            uint size = GetSize();
            var buffer = Marshal.AllocHGlobal((int)size);
            if (!StormLib.SFileReadFile(Ptr, buffer, size, 0u, IntPtr.Zero))
            {
                throw new Exception("Не удалось открыть файл в MPQ!");
            }

            //Данные
            byte[] data = new byte[size];
            Marshal.Copy(buffer, data, 0, (int)size);
            Marshal.FreeHGlobal(buffer);
            return data;
        }

        //Запись
        public bool Write(byte[] data)
        {
            var buffer = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, buffer, data.Length);
            bool res = StormLib.SFileWriteFile(Ptr, buffer, (uint)data.Length, 0x2u);
            Marshal.FreeHGlobal(buffer);
            return res;
        }
    }
}