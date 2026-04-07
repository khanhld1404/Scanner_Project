using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Keyence_Device
{
    public static class Sound_Function
    {
        // ---- P/Invoke cho CE (Unicode) ----
        [DllImport("coredll.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool MessageBeep(uint uType);

        [DllImport("coredll.dll", SetLastError = true, EntryPoint = "waveOutSetVolume")]
        private static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume); // 0 = MMSYSERR_NOERROR

        [DllImport("coredll.dll", SetLastError = true, EntryPoint = "waveOutGetVolume")]
        private static extern int waveOutGetVolume(IntPtr hwo, out uint pdwVolume);

        // ---- Flags PlaySound ----
        private const uint SND_SYNC        = 0x0000;       // chặn
        private const uint SND_ASYNC       = 0x0001;       // không chặn
        private const uint SND_NODEFAULT   = 0x0002;       // không beep mặc định khi lỗi
        private const uint SND_FILENAME    = 0x00020000;   // pszSound là path

        // ---- Helper: thư mục .exe ----
        public static string GetAppDir()
        {
            // CE thường trả đường dẫn tuyệt đối (không có "file:\")
            // Nhưng để chắc chắn, ta loại bỏ prefix nếu có:
            string codeBase = Assembly.GetExecutingAssembly().GetName().CodeBase ?? string.Empty;

            // Xử lý "file:\"
            if (codeBase.StartsWith("file:\\", StringComparison.OrdinalIgnoreCase))
                codeBase = codeBase.Substring("file:\\".Length);

            string dir = Path.GetDirectoryName(codeBase);
            return string.IsNullOrEmpty(dir) ? "\\" : dir; // fallback root nếu null
        }

        public static string InApp(string fileName)
        {
            return Path.Combine(GetAppDir(), fileName);
        }

        // ---- Set volume tối đa (nếu driver hỗ trợ) ----
        public static bool TrySetMaxVolume()
        {
            try
            {
                // Mỗi kênh 0..0xFFFF. 0xFFFFFFFF = full L/R
                int res = waveOutSetVolume(IntPtr.Zero, 0xFFFFFFFF);
                return res == 0;
            }
            catch { return false; }
        }

        public static bool TryGetVolume(out ushort left, out ushort right)
        {
            left = right = 0;
            try
            {
                uint v;
                int res = waveOutGetVolume(IntPtr.Zero, out v);
                if (res != 0) return false;
                left = (ushort)(v & 0xFFFF);
                right = (ushort)((v >> 16) & 0xFFFF);
                return true;
            }
            catch { return false; }
        }

        // ---- Phát WAV từ path ----
        public static bool PlayFromFile(string filePath, bool async, bool suppressDefaultBeep, out int lastError)
        {
            lastError = 0;

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                lastError = 2; // như "file not found"
                return false;
            }

            uint flags = SND_FILENAME | (async ? SND_ASYNC : SND_SYNC);
            if (suppressDefaultBeep)
                flags |= SND_NODEFAULT;

            bool ok = PlaySound(filePath, IntPtr.Zero, flags);
            if (!ok)
                lastError = Marshal.GetLastWin32Error();

            return ok;
        }

        // ---- Phát WAV trong thư mục app ----
        public static bool PlayInAppFolder(string fileName, bool async, bool suppressDefaultBeep, out int lastError)
        {
            string full = InApp(fileName);
            return PlayFromFile(full, async, suppressDefaultBeep, out lastError);
        }

        // ---- Dừng phát ----
        public static void Stop()
        {
            PlaySound(null, IntPtr.Zero, 0);
        }

        // ---- Test beep hệ thống ----
        public static bool TryBeep()
        {
            try { return MessageBeep(0); }
            catch { return false; }
        }
    }
}
