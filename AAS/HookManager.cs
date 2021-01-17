using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using AAS;

namespace Hooking
{
    public class HookManager
    {
        #region Fields

        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A
        }

        public enum MouseActionAdresses
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        public static POINT emptyPoint;
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct mouseHookStruct
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        private static Form1 mainWindow_;

        #endregion

        public static bool Hook(Form1 mainWindow)
        {
            mainWindow_ = mainWindow;
            if (MouseHook.Hook() && KeyboardHook.Hook())
                return true;
            else
            {
                MouseHook.UnHook();
                KeyboardHook.UnHook();
                return false;
            }
        }

        public static bool UnHook()
        {
            if (MouseHook.UnHook() && KeyboardHook.UnHook())
                return true;
            else
                return false;
        }

        public static Color GetPixelColor(int x, int y)
        {
            return MouseHook.GetPixelColor(x, y);
        }

        private class MouseHook
        {
            #region Fields

            private static LowLevelMouseProc _proc = MouseHookCallback;
            private static IntPtr _hookID = IntPtr.Zero;

            private const int WH_MOUSE_LL = 14;

            #endregion

            #region Windows functions

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook,
                LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
                IntPtr wParam, IntPtr lParam);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr GetModuleHandle(string lpModuleName);

            [DllImport("user32.dll")]
            static extern IntPtr GetDC(IntPtr hwnd);

            [DllImport("user32.dll")]
            static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

            [DllImport("gdi32.dll")]
            static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

            #endregion

            public static bool Hook()
            {
                _hookID = SetHook(_proc);
                if (_hookID != IntPtr.Zero)
                    return true;
                else
                    return false;
            }

            public static bool UnHook()
            {
                return UnhookWindowsHookEx(_hookID);
            }

            private static IntPtr SetHook(LowLevelMouseProc proc)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_MOUSE_LL, proc,
                        GetModuleHandle(curModule.ModuleName), 0);
                }
            }

            private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

            public static Color GetPixelColor(int x, int y)
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                uint pixel = GetPixel(hdc, x, y);
                ReleaseDC(IntPtr.Zero, hdc);
                Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                             (int)(pixel & 0x0000FF00) >> 8,
                             (int)(pixel & 0x00FF0000) >> 16);
                return color;
            }

            private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
            {
                if (nCode >= 0)
                {
                    ((Form1)mainWindow_).MouseHookProc(wParam, lParam);
                }
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }
        }

        private class KeyboardHook
        {
            #region Fields

            //Declare hook handle as int.
            static int hHook = 0;
            static HookProc KeyboardHookProcedure;

            //Declare keyboard hook constant.
            //For other hook types, you can obtain these values from Winuser.h in Microsoft SDK.
            const int WH_KEYBOARD_LL = 13;

            #endregion

            #region Windows functions

            //Import for SetWindowsHookEx function.
            //Use this function to install thread-specific hook.
            [DllImport("user32.dll", CharSet = CharSet.Auto,
             CallingConvention = CallingConvention.StdCall)]
            private static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
            IntPtr hInstance, int threadId);

            //Import for UnhookWindowsHookEx.
            //Call this function to uninstall the hook.
            [DllImport("user32.dll", CharSet = CharSet.Auto,
             CallingConvention = CallingConvention.StdCall)]
            private static extern bool UnhookWindowsHookEx(int idHook);

            //Import for CallNextHookEx.
            //Use this function to pass the hook information to next hook procedure in chain.
            [DllImport("user32.dll", CharSet = CharSet.Auto,
             CallingConvention = CallingConvention.StdCall)]
            private static extern int CallNextHookEx(int idHook, int nCode,
            IntPtr wParam, IntPtr lParam);

            [DllImport("kernel32.dll")]
            static extern IntPtr LoadLibrary(string lpFileName);

            #endregion

            private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

            public static bool Hook()
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);

                hHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, (IntPtr)LoadLibrary("User32"), 0);
                if (hHook != 0)
                    return true;
                else
                    return false;
            }

            public static bool UnHook()
            {
                bool ret = UnhookWindowsHookEx(hHook);
                if (ret)
                    hHook = 0;
                return ret;
            }

            private static int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
            {
                if (nCode < 0)
                {
                    return CallNextHookEx(hHook, nCode, wParam, lParam);
                }
                else
                {
                    if (!mainWindow_.ContainsFocus)
                    {
                        if (((int)wParam == 256) || ((int)wParam == 260) || ((int)wParam == 257) || ((int)wParam == 261)) //WM_KEYDOWN || WM_SYSKEYDOWN || WM_KEYUP || WM_SYSKEYUP
                            ((Form1)mainWindow_).KeyboardHookProc(wParam, lParam);

                        return CallNextHookEx(hHook, nCode, wParam, lParam);
                    }
                    else if (mainWindow_.select_key)
                    {
                        if (((int)wParam == 256) || ((int)wParam == 260) || ((int)wParam == 257) || ((int)wParam == 261)) //WM_KEYDOWN || WM_SYSKEYDOWN || WM_KEYUP || WM_SYSKEYUP
                            ((Form1)mainWindow_).KeyboardHookProc(wParam, lParam);

                        return 1;
                    }
                    else
                        return CallNextHookEx(hHook, nCode, wParam, lParam);
                }
            }
        }
    }
}
