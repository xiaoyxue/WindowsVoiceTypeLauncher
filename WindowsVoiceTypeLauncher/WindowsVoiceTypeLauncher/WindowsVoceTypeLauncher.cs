using System;
using System.Runtime.InteropServices;

namespace WindowsVoiceTypeLauncher
{
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