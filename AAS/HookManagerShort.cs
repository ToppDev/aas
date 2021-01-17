namespace Hooking
{
    public class KeyboardHook
    {
        static bool ABORT = false;
        static System.Windows.Forms.Keys interruptKey = System.Windows.Forms.Keys.Escape;

        //Declare hook handle as int.
        static int hHook = 0;
        static HookProc KeyboardHookProcedure;

        //Declare keyboard hook constant.
        //For other hook types, you can obtain these values from Winuser.h in Microsoft SDK.
        const int WH_KEYBOARD_LL = 13;

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public class keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        
        //Import for SetWindowsHookEx function.
        //Use this function to install thread-specific hook.
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto,
            CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
        System.IntPtr hInstance, int threadId);

        //Import for UnhookWindowsHookEx.
        //Call this function to uninstall the hook.
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto,
            CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        //Import for CallNextHookEx.
        //Use this function to pass the hook information to next hook procedure in chain.
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto,
            CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode,
        System.IntPtr wParam, System.IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern System.IntPtr LoadLibrary(string lpFileName);

        private delegate int HookProc(int nCode, System.IntPtr wParam, System.IntPtr lParam);

        public static bool Hook()
        {
            ABORT = false;
            KeyboardHookProcedure = new HookProc(KeyboardHookProc);

            hHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, (System.IntPtr)LoadLibrary("User32"), 0);
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

        private static int KeyboardHookProc(int nCode, System.IntPtr wParam, System.IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                if (((int)wParam == 256) || ((int)wParam == 260)) //WM_KEYDOWN || WM_SYSKEYDOWN || WM_KEYUP || WM_SYSKEYUP
                {
                    keyboardHookStruct MyKeyboardHookStruct = (keyboardHookStruct)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(keyboardHookStruct));
                    if ((System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode == System.Windows.Forms.Keys.Escape)
                    {
                        UnHook();
                        ABORT = true;
                        //System.Environment.Exit(0);
                        //System.Windows.Forms.Application.Exit();
                    }
                    System.Windows.Forms.MessageBox.Show(((System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode).ToString());
                }

                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }
    }
}
