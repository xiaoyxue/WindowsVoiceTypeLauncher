using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsVoiceTypeLauncher
{
    internal static unsafe partial class VoiceTypeLauncher
    {
        private const uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
        private const int IME_CMODE_ALPHANUMERIC = 0x0000;  // English Mode
        private const int IME_CMODE_NATIVE = 0x0001;        // Chinese, Native
        private const int IME_CMODE_FULLSHAPE = 0x0008;     // FullShape

        public static CultureInfo? GetCurrentInputLanguage()
        {
            try
            {
                IntPtr foregroundWindow = GetForegroundWindow();
                if (foregroundWindow == IntPtr.Zero) return null;

                uint threadId = GetWindowThreadProcessId(foregroundWindow, IntPtr.Zero);
                IntPtr hkl = GetKeyboardLayout(threadId);

                int lcid = hkl.ToInt32() & 0xFFFF;
                return new CultureInfo(lcid);
            }
            catch
            {
                return null;
            }
        }

        public static bool SwitchInputLanguage(string cultureName = "zh-CN")
        {
            try
            {
                IntPtr foregroundWindow = GetForegroundWindow();
                if (foregroundWindow == IntPtr.Zero)
                {
                    return false;
                }

                var culture = new CultureInfo(cultureName);

                IntPtr hkl = new IntPtr(culture.LCID);

                PostMessage(foregroundWindow, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, hkl);

                return true;
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        internal static bool SetNativeMode()
        {
            return SetConversionMode(IME_CMODE_NATIVE);
        }

        internal static bool SetAlphanumericMode()
        {
            return SetConversionMode(IME_CMODE_ALPHANUMERIC);
        }

        internal static bool SetConversionMode(int targetMode)
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero) return false;

            IntPtr hIMC = ImmGetContext(hWnd);
            if (hIMC == IntPtr.Zero) return false;

            try
            {
                int conversion, sentence;
                if (ImmGetConversionStatus(hIMC, out conversion, out sentence))
                {
 
                    conversion &= ~IME_CMODE_ALPHANUMERIC; 
                    conversion |= targetMode;              


                    return ImmSetConversionStatus(hIMC, conversion, sentence);
                }
                return false;
            }
            finally
            {
                if (hIMC != IntPtr.Zero)
                {
                    ImmReleaseContext(hWnd, hIMC);
                }
            }
        }
    }
}
