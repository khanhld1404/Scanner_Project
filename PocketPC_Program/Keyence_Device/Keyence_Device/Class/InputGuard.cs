using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;   // <— QUAN TRỌNG
namespace Keyence_Device.Class
{
    // Nếu không có class này thì sẽ bị lỗi con trỏ chuột vẫn còn chưa giải phóng nên có thể nó sẽ mở chức năng
    // 3
    public static class InputGuard
    {
        [DllImport("coredll.dll", SetLastError = false)]
        private static extern bool PeekMessage(
            out MSG lpMsg, IntPtr hWnd,
            uint wMsgFilterMin, uint wMsgFilterMax,
            uint wRemoveMsg);

        private const uint PM_REMOVE = 0x0001;

        // KEYBOARD range
        private const int WM_KEYFIRST = 0x0100;   // WM_KEYDOWN
        private const int WM_KEYLAST = 0x0108;   // WM_KEYLAST (gồm WM_CHAR)

        // MOUSE/STYLUS range
        private const int WM_MOUSEFIRST = 0x0200;   // WM_MOUSEMOVE
        private const int WM_MOUSELAST = 0x020D;   // đến WM_MOUSELEAVE (an toàn)

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public int pt_x;
            public int pt_y;
        }

        private static DateTime _suppressUntilUtc = DateTime.MinValue;

        /// <summary>Xả **toàn bộ** message KEYBOARD & MOUSE đang chờ.</summary>
        public static void DrainInputQueue()
        {
            MSG msg;
            // Xả key trước…
            while (PeekMessage(out msg, IntPtr.Zero, WM_KEYFIRST, WM_KEYLAST, PM_REMOVE)) { }
            // …rồi xả mouse/stylus
            while (PeekMessage(out msg, IntPtr.Zero, WM_MOUSEFIRST, WM_MOUSELAST, PM_REMOVE)) { }
        }

        /// <summary>Khóa xử lý input trong ms mili‑giây (debounce toàn cục).</summary>
        public static void SuppressForMs(int ms)
        {
            if (ms < 0) ms = 0;
            _suppressUntilUtc = DateTime.UtcNow.AddMilliseconds(ms);
        }

        /// <summary>Đang trong thời gian khóa input?</summary>
        public static bool IsSuppressed()
        {
            return DateTime.UtcNow < _suppressUntilUtc;
        }
    }
    
}
