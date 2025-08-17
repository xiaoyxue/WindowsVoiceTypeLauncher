using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsVoiceTypeLauncher
{
    internal unsafe partial class VoiceTypeLauncher
    {
        #region Win32 API Structures and Constants
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public InputUnion u;
        }

        // On 64-bit, the INPUT union has a size of 24 bytes
        [StructLayout(LayoutKind.Explicit, Size = 24)]
        private struct InputUnion
        {
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        private const int INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const ushort VK_LWIN = 0x5B;
        private const ushort VK_H = 0x48;
        #endregion

        #region P/Invoke Imports   
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("imm32.dll")]
        private static extern bool ImmGetConversionStatus(IntPtr hIMC, out int lpdwConversion, out int lpdwSentence);

        [DllImport("imm32.dll")]
        private static extern bool ImmSetConversionStatus(IntPtr hIMC, int fdwConversion, int fdwSentence);

        [DllImport("imm32.dll")]
        private static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT* pInputs, int cbSize);
        #endregion
    }
}
