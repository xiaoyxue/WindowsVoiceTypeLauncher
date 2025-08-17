using System;
using System.Runtime.InteropServices;

namespace WindowsVoiceTypeLauncher
{
    internal static unsafe partial class VoiceTypeLauncher
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
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT* pInputs, int cbSize);
        #endregion
    }

    internal static unsafe partial class VoiceTypeLauncher
    {
        private static void LaunchVoiceType()
        {
            const int inputCount = 4;
            INPUT* inputs = stackalloc INPUT[inputCount];

            inputs[0] = new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new InputUnion { ki = new KEYBDINPUT { wVk = VK_LWIN, wScan = 0, dwFlags = 0, time = 0, dwExtraInfo = IntPtr.Zero } }
            };
            inputs[1] = new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new InputUnion { ki = new KEYBDINPUT { wVk = VK_H, wScan = 0, dwFlags = 0, time = 0, dwExtraInfo = IntPtr.Zero } }
            };
            inputs[2] = new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new InputUnion { ki = new KEYBDINPUT { wVk = VK_H, wScan = 0, dwFlags = KEYEVENTF_KEYUP, time = 0, dwExtraInfo = IntPtr.Zero } }
            };
            inputs[3] = new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new InputUnion { ki = new KEYBDINPUT { wVk = VK_LWIN, wScan = 0, dwFlags = KEYEVENTF_KEYUP, time = 0, dwExtraInfo = IntPtr.Zero } }
            };

            int cbSize = Marshal.SizeOf(typeof(INPUT));
            uint result = SendInput(inputCount, inputs, cbSize);
        }

        static void Main()
        {

            SwitchInputLanguage("zh-CN");
            //var success = SetAlphanumericMode();
            SetNativeMode();
            LaunchVoiceType();
        }
    }
}