using System.Runtime.InteropServices;
namespace WC3Files.MPQ
{
    static class StormLib
    {
        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileOpenArchive([MarshalAs(UnmanagedType.LPTStr)] string szMpqName, uint dwPriority, uint dwFlags, out IntPtr phMpq);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileCloseArchive(IntPtr hMpq);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileOpenFileEx(IntPtr hMpq, [MarshalAs(UnmanagedType.LPStr)] string szFileName, uint dwSearchScope, out IntPtr phFile);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern uint SFileGetFileSize(IntPtr hFile, uint pdwFileSizeHigh);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileReadFile(IntPtr hFile, IntPtr lpBuffer, uint dwToRead, uint pdwRead, IntPtr lpOverlapped);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileCloseFile(IntPtr hFile);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileRenameFile(IntPtr hMpq, [MarshalAs(UnmanagedType.LPStr)] string szOldFileName, [MarshalAs(UnmanagedType.LPStr)] string szNewFileName);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileCreateFile(IntPtr hMpq, [MarshalAs(UnmanagedType.LPStr)] string szArchiveName, ulong fileTime, uint dwFileSize, uint lcLocale, uint dwFlags, out IntPtr phFile);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileWriteFile(IntPtr hFile, IntPtr pvData, uint dwSize, uint dwCompression);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileFinishFile(IntPtr hFile);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileAddFileEx(IntPtr hMpq, [MarshalAs(UnmanagedType.LPTStr)] string szFileName, [MarshalAs(UnmanagedType.LPStr)] string szArchivedName, uint dwFlags, uint dwCompression, uint dwCompressionNext);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileRemoveFile(IntPtr hMpq, [MarshalAs(UnmanagedType.LPStr)] string szFileName, uint dwSearchScope);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileCompactArchive(IntPtr hMpq, [MarshalAs(UnmanagedType.LPStr)] string szListFile, bool bReserved);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileExtractFile(IntPtr hMpq, [MarshalAs(UnmanagedType.LPStr)] string szToExtract, [MarshalAs(UnmanagedType.LPTStr)] string szExtracted, uint dwSearchScope);

        [DllImport("stormlib.dll", ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern bool SFileSetMaxFileCount(IntPtr hMpq, uint dwMaxFileCount);
    }
}
