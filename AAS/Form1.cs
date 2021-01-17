using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Hooking;
using System.IO;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace AAS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Text = Application.ProductName + " v" + Application.ProductVersion + " by " + Application.CompanyName;

            new ScintillaExtender(scintilla, this).configure();

            autocompleteMenu1.TargetControlWrapper = new ScintillaWrapper(scintilla);

            Form1_SizeChanged(null, null);

            scintilla.CurrentPosition = 100;
            scintilla.AnchorPosition = 100;

            doubleClickTime = GetDoubleClickTime();

            addNewLineStrings();
            addHookedKeys();

            setDefaultTexts();
        }

        #region Fields
                
        CompilerResults compilerResults;

        string cryptPassword = "vgdbfs§\"$\"sasdas4453@#+";
        public string code, references, hookmanager, assemblynames, credits;
        int tabControleSelectedIndex = 0;
        uint compiled;

        Dictionary<string, string> hookedKeys = new Dictionary<string, string>();
        List<string> newLineStrings = new List<string>();

        public bool select_key;
        bool waitSelect;
        bool recording, select_color, select_coordinate, select_window;
        string shift = "", ctrl = "", alt = "";
        string input = "";

        HookManager.POINT lastDown, lastClick;
        long lastClickTime;
        uint doubleClickTime;

        #endregion

        #region Enums

        enum IDC_STANDARD_CURSORS : int
        {
            IDC_ARROW = 32512,
            IDC_IBEAM = 32513,
            IDC_WAIT = 32514,
            IDC_CROSS = 32515,
            IDC_UPARROW = 32516,
            IDC_SIZE = 32640,
            IDC_ICON = 32641,
            IDC_SIZENWSE = 32642,
            IDC_SIZENESW = 32643,
            IDC_SIZEWE = 32644,
            IDC_SIZENS = 32645,
            IDC_SIZEALL = 32646,
            IDC_NO = 32648,
            IDC_HAND = 32649,
            IDC_APPSTARTING = 32650,
            IDC_HELP = 32651
        }

        enum OCR_SYSTEM_CURSORS : uint
        {
            /// <summary>
            /// Standard arrow and small hourglass
            /// </summary>
            OCR_APPSTARTING = 32650,
            /// <summary>
            /// Standard arrow
            /// </summary>
            OCR_NORMAL = 32512,
            /// <summary>
            /// Crosshair
            /// </summary>
            OCR_CROSS = 32515,
            /// <summary>
            /// Windows 2000/XP: Hand
            /// </summary>
            OCR_HAND = 32649,
            /// <summary>
            /// Arrow and question mark
            /// </summary>
            OCR_HELP = 32651,
            /// <summary>
            /// I-beam
            /// </summary>
            OCR_IBEAM = 32513,
            /// <summary>
            /// Slashed circle
            /// </summary>
            OCR_NO = 32648,
            /// <summary>
            /// Four-pointed arrow pointing north, south, east, and west
            /// </summary>
            OCR_SIZEALL = 32646,
            /// <summary>
            /// Double-pointed arrow pointing northeast and southwest
            /// </summary>
            OCR_SIZENESW = 32643,
            /// <summary>
            /// Double-pointed arrow pointing north and south
            /// </summary>
            OCR_SIZENS = 32645,
            /// <summary>
            /// Double-pointed arrow pointing northwest and southeast
            /// </summary>
            OCR_SIZENWSE = 32642,
            /// <summary>
            /// Double-pointed arrow pointing west and east
            /// </summary>
            OCR_SIZEWE = 32644,
            /// <summary>
            /// Vertical arrow
            /// </summary>
            OCR_UP = 32516,
            /// <summary>
            /// Hourglass
            /// </summary>
            OCR_WAIT = 32514
        }

        public enum SystemParametersInfoAction : uint
        {
            SPI_GETBEEP = 0x0001,
            SPI_SETBEEP = 0x0002,
            SPI_GETMOUSE = 0x0003,
            SPI_SETMOUSE = 0x0004,
            SPI_GETBORDER = 0x0005,
            SPI_SETBORDER = 0x0006,
            SPI_GETKEYBOARDSPEED = 0x000A,
            SPI_SETKEYBOARDSPEED = 0x000B,
            SPI_LANGDRIVER = 0x000C,
            SPI_ICONHORIZONTALSPACING = 0x000D,
            SPI_GETSCREENSAVETIMEOUT = 0x000E,
            SPI_SETSCREENSAVETIMEOUT = 0x000F,
            SPI_GETSCREENSAVEACTIVE = 0x0010,
            SPI_SETSCREENSAVEACTIVE = 0x0011,
            SPI_GETGRIDGRANULARITY = 0x0012,
            SPI_SETGRIDGRANULARITY = 0x0013,
            SPI_SETDESKWALLPAPER = 0x0014,
            SPI_SETDESKPATTERN = 0x0015,
            SPI_GETKEYBOARDDELAY = 0x0016,
            SPI_SETKEYBOARDDELAY = 0x0017,
            SPI_ICONVERTICALSPACING = 0x0018,
            SPI_GETICONTITLEWRAP = 0x0019,
            SPI_SETICONTITLEWRAP = 0x001A,
            SPI_GETMENUDROPALIGNMENT = 0x001B,
            SPI_SETMENUDROPALIGNMENT = 0x001C,
            SPI_SETDOUBLECLKWIDTH = 0x001D,
            SPI_SETDOUBLECLKHEIGHT = 0x001E,
            SPI_GETICONTITLELOGFONT = 0x001F,
            SPI_SETDOUBLECLICKTIME = 0x0020,
            SPI_SETMOUSEBUTTONSWAP = 0x0021,
            SPI_SETICONTITLELOGFONT = 0x0022,
            SPI_GETFASTTASKSWITCH = 0x0023,
            SPI_SETFASTTASKSWITCH = 0x0024,
            SPI_SETDRAGFULLWINDOWS = 0x0025,
            SPI_GETDRAGFULLWINDOWS = 0x0026,
            SPI_GETNONCLIENTMETRICS = 0x0029,
            SPI_SETNONCLIENTMETRICS = 0x002A,
            SPI_GETMINIMIZEDMETRICS = 0x002B,
            SPI_SETMINIMIZEDMETRICS = 0x002C,
            SPI_GETICONMETRICS = 0x002D,
            SPI_SETICONMETRICS = 0x002E,
            SPI_SETWORKAREA = 0x002F,
            SPI_GETWORKAREA = 0x0030,
            SPI_SETPENWINDOWS = 0x0031,
            SPI_GETHIGHCONTRAST = 0x0042,
            SPI_SETHIGHCONTRAST = 0x0043,
            SPI_GETKEYBOARDPREF = 0x0044,
            SPI_SETKEYBOARDPREF = 0x0045,
            SPI_GETSCREENREADER = 0x0046,
            SPI_SETSCREENREADER = 0x0047,
            SPI_GETANIMATION = 0x0048,
            SPI_SETANIMATION = 0x0049,
            SPI_GETFONTSMOOTHING = 0x004A,
            SPI_SETFONTSMOOTHING = 0x004B,
            SPI_SETDRAGWIDTH = 0x004C,
            SPI_SETDRAGHEIGHT = 0x004D,
            SPI_SETHANDHELD = 0x004E,
            SPI_GETLOWPOWERTIMEOUT = 0x004F,
            SPI_GETPOWEROFFTIMEOUT = 0x0050,
            SPI_SETLOWPOWERTIMEOUT = 0x0051,
            SPI_SETPOWEROFFTIMEOUT = 0x0052,
            SPI_GETLOWPOWERACTIVE = 0x0053,
            SPI_GETPOWEROFFACTIVE = 0x0054,
            SPI_SETLOWPOWERACTIVE = 0x0055,
            SPI_SETPOWEROFFACTIVE = 0x0056,
            SPI_SETCURSORS = 0x0057,
            SPI_SETICONS = 0x0058,
            SPI_GETDEFAULTINPUTLANG = 0x0059,
            SPI_SETDEFAULTINPUTLANG = 0x005A,
            SPI_SETLANGTOGGLE = 0x005B,
            SPI_GETWINDOWSEXTENSION = 0x005C,
            SPI_SETMOUSETRAILS = 0x005D,
            SPI_GETMOUSETRAILS = 0x005E,
            SPI_SETSCREENSAVERRUNNING = 0x0061,
            SPI_SCREENSAVERRUNNING = SPI_SETSCREENSAVERRUNNING,
            SPI_GETFILTERKEYS = 0x0032,
            SPI_SETFILTERKEYS = 0x0033,
            SPI_GETTOGGLEKEYS = 0x0034,
            SPI_SETTOGGLEKEYS = 0x0035,
            SPI_GETMOUSEKEYS = 0x0036,
            SPI_SETMOUSEKEYS = 0x0037,
            SPI_GETSHOWSOUNDS = 0x0038,
            SPI_SETSHOWSOUNDS = 0x0039,
            SPI_GETSTICKYKEYS = 0x003A,
            SPI_SETSTICKYKEYS = 0x003B,
            SPI_GETACCESSTIMEOUT = 0x003C,
            SPI_SETACCESSTIMEOUT = 0x003D,
            SPI_GETSERIALKEYS = 0x003E,
            SPI_SETSERIALKEYS = 0x003F,
            SPI_GETSOUNDSENTRY = 0x0040,
            SPI_SETSOUNDSENTRY = 0x0041,
            SPI_GETSNAPTODEFBUTTON = 0x005F,
            SPI_SETSNAPTODEFBUTTON = 0x0060,
            SPI_GETMOUSEHOVERWIDTH = 0x0062,
            SPI_SETMOUSEHOVERWIDTH = 0x0063,
            SPI_GETMOUSEHOVERHEIGHT = 0x0064,
            SPI_SETMOUSEHOVERHEIGHT = 0x0065,
            SPI_GETMOUSEHOVERTIME = 0x0066,
            SPI_SETMOUSEHOVERTIME = 0x0067,
            SPI_GETWHEELSCROLLLINES = 0x0068,
            SPI_SETWHEELSCROLLLINES = 0x0069,
            SPI_GETMENUSHOWDELAY = 0x006A,
            SPI_SETMENUSHOWDELAY = 0x006B,
            SPI_GETWHEELSCROLLCHARS = 0x006C,
            SPI_SETWHEELSCROLLCHARS = 0x006D,
            SPI_GETSHOWIMEUI = 0x006E,
            SPI_SETSHOWIMEUI = 0x006F,
            SPI_GETMOUSESPEED = 0x0070,
            SPI_SETMOUSESPEED = 0x0071,
            SPI_GETSCREENSAVERRUNNING = 0x0072,
            SPI_GETDESKWALLPAPER = 0x0073,
            SPI_GETAUDIODESCRIPTION = 0x0074,
            SPI_SETAUDIODESCRIPTION = 0x0075,
            SPI_GETSCREENSAVESECURE = 0x0076,
            SPI_SETSCREENSAVESECURE = 0x0077,
            SPI_GETHUNGAPPTIMEOUT = 0x0078,
            SPI_SETHUNGAPPTIMEOUT = 0x0079,
            SPI_GETWAITTOKILLTIMEOUT = 0x007A,
            SPI_SETWAITTOKILLTIMEOUT = 0x007B,
            SPI_GETWAITTOKILLSERVICETIMEOUT = 0x007C,
            SPI_SETWAITTOKILLSERVICETIMEOUT = 0x007D,
            SPI_GETMOUSEDOCKTHRESHOLD = 0x007E,
            SPI_SETMOUSEDOCKTHRESHOLD = 0x007F,
            SPI_GETPENDOCKTHRESHOLD = 0x0080,
            SPI_SETPENDOCKTHRESHOLD = 0x0081,
            SPI_GETWINARRANGING = 0x0082,
            SPI_SETWINARRANGING = 0x0083,
            SPI_GETMOUSEDRAGOUTTHRESHOLD = 0x0084,
            SPI_SETMOUSEDRAGOUTTHRESHOLD = 0x0085,
            SPI_GETPENDRAGOUTTHRESHOLD = 0x0086,
            SPI_SETPENDRAGOUTTHRESHOLD = 0x0087,
            SPI_GETMOUSESIDEMOVETHRESHOLD = 0x0088,
            SPI_SETMOUSESIDEMOVETHRESHOLD = 0x0089,
            SPI_GETPENSIDEMOVETHRESHOLD = 0x008A,
            SPI_SETPENSIDEMOVETHRESHOLD = 0x008B,
            SPI_GETDRAGFROMMAXIMIZE = 0x008C,
            SPI_SETDRAGFROMMAXIMIZE = 0x008D,
            SPI_GETSNAPSIZING = 0x008E,
            SPI_SETSNAPSIZING = 0x008F,
            SPI_GETDOCKMOVING = 0x0090,
            SPI_SETDOCKMOVING = 0x0091,
            SPI_GETACTIVEWINDOWTRACKING = 0x1000,
            SPI_SETACTIVEWINDOWTRACKING = 0x1001,
            SPI_GETMENUANIMATION = 0x1002,
            SPI_SETMENUANIMATION = 0x1003,
            SPI_GETCOMBOBOXANIMATION = 0x1004,
            SPI_SETCOMBOBOXANIMATION = 0x1005,
            SPI_GETLISTBOXSMOOTHSCROLLING = 0x1006,
            SPI_SETLISTBOXSMOOTHSCROLLING = 0x1007,
            SPI_GETGRADIENTCAPTIONS = 0x1008,
            SPI_SETGRADIENTCAPTIONS = 0x1009,
            SPI_GETKEYBOARDCUES = 0x100A,
            SPI_SETKEYBOARDCUES = 0x100B,
            SPI_GETMENUUNDERLINES = SPI_GETKEYBOARDCUES,
            SPI_SETMENUUNDERLINES = SPI_SETKEYBOARDCUES,
            SPI_GETACTIVEWNDTRKZORDER = 0x100C,
            SPI_SETACTIVEWNDTRKZORDER = 0x100D,
            SPI_GETHOTTRACKING = 0x100E,
            SPI_SETHOTTRACKING = 0x100F,
            SPI_GETMENUFADE = 0x1012,
            SPI_SETMENUFADE = 0x1013,
            SPI_GETSELECTIONFADE = 0x1014,
            SPI_SETSELECTIONFADE = 0x1015,
            SPI_GETTOOLTIPANIMATION = 0x1016,
            SPI_SETTOOLTIPANIMATION = 0x1017,
            SPI_GETTOOLTIPFADE = 0x1018,
            SPI_SETTOOLTIPFADE = 0x1019,
            SPI_GETCURSORSHADOW = 0x101A,
            SPI_SETCURSORSHADOW = 0x101B,
            SPI_GETMOUSESONAR = 0x101C,
            SPI_SETMOUSESONAR = 0x101D,
            SPI_GETMOUSECLICKLOCK = 0x101E,
            SPI_SETMOUSECLICKLOCK = 0x101F,
            SPI_GETMOUSEVANISH = 0x1020,
            SPI_SETMOUSEVANISH = 0x1021,
            SPI_GETFLATMENU = 0x1022,
            SPI_SETFLATMENU = 0x1023,
            SPI_GETDROPSHADOW = 0x1024,
            SPI_SETDROPSHADOW = 0x1025,
            SPI_GETBLOCKSENDINPUTRESETS = 0x1026,
            SPI_SETBLOCKSENDINPUTRESETS = 0x1027,
            SPI_GETUIEFFECTS = 0x103E,
            SPI_SETUIEFFECTS = 0x103F,
            SPI_GETDISABLEOVERLAPPEDCONTENT = 0x1040,
            SPI_SETDISABLEOVERLAPPEDCONTENT = 0x1041,
            SPI_GETCLIENTAREAANIMATION = 0x1042,
            SPI_SETCLIENTAREAANIMATION = 0x1043,
            SPI_GETCLEARTYPE = 0x1048,
            SPI_SETCLEARTYPE = 0x1049,
            SPI_GETSPEECHRECOGNITION = 0x104A,
            SPI_SETSPEECHRECOGNITION = 0x104B,
            SPI_GETFOREGROUNDLOCKTIMEOUT = 0x2000,
            SPI_SETFOREGROUNDLOCKTIMEOUT = 0x2001,
            SPI_GETACTIVEWNDTRKTIMEOUT = 0x2002,
            SPI_SETACTIVEWNDTRKTIMEOUT = 0x2003,
            SPI_GETFOREGROUNDFLASHCOUNT = 0x2004,
            SPI_SETFOREGROUNDFLASHCOUNT = 0x2005,
            SPI_GETCARETWIDTH = 0x2006,
            SPI_SETCARETWIDTH = 0x2007,
            SPI_GETMOUSECLICKLOCKTIME = 0x2008,
            SPI_SETMOUSECLICKLOCKTIME = 0x2009,
            SPI_GETFONTSMOOTHINGTYPE = 0x200A,
            SPI_SETFONTSMOOTHINGTYPE = 0x200B,
            SPI_GETFONTSMOOTHINGCONTRAST = 0x200C,
            SPI_SETFONTSMOOTHINGCONTRAST = 0x200D,
            SPI_GETFOCUSBORDERWIDTH = 0x200E,
            SPI_SETFOCUSBORDERWIDTH = 0x200F,
            SPI_GETFOCUSBORDERHEIGHT = 0x2010,
            SPI_SETFOCUSBORDERHEIGHT = 0x2011,
            SPI_GETFONTSMOOTHINGORIENTATION = 0x2012,
            SPI_SETFONTSMOOTHINGORIENTATION = 0x2013,
            SPI_GETMINIMUMHITRADIUS = 0x2014,
            SPI_SETMINIMUMHITRADIUS = 0x2015,
            SPI_GETMESSAGEDURATION = 0x2016,
            SPI_SETMESSAGEDURATION = 0x2017
        }

        #endregion

        #region Dll Imports

        [DllImport("user32.dll")]
        private static extern uint GetDoubleClickTime();

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32", EntryPoint = "LoadCursorFromFile")]
        public static extern IntPtr LoadCursorFromFile(string lpFileName);
        [DllImport("user32.dll")]
        static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
        [DllImport("user32.dll")]
        static extern bool SetSystemCursor(IntPtr hcur, uint id);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int pvParam, uint fWinIni); // T = any type

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        #endregion

        #region Methods

        public void resetCompiler()
        {
            if (compiled != 2)
            {
                toolTip1.SetToolTip(pibo_compile, "Compile");
                pibo_compile.Image = Properties.Resources.StepForwardNormalBlue;
                compilerResults = null;
                compiled = 0;
            }
        }

        private void addNewLineStrings()
        {
            newLineStrings.Add("{ESC}");
            newLineStrings.Add("{F1}");
            newLineStrings.Add("{F2}");
            newLineStrings.Add("{F3}");
            newLineStrings.Add("{F4}");
            newLineStrings.Add("{F5}");
            newLineStrings.Add("{F6}");
            newLineStrings.Add("{F7}");
            newLineStrings.Add("{F8}");
            newLineStrings.Add("{F9}");
            newLineStrings.Add("{F10}");
            newLineStrings.Add("{F11}");
            newLineStrings.Add("{F12}");
            newLineStrings.Add("{INSERT}");
            newLineStrings.Add("{PRTSC}");
            newLineStrings.Add("{BREAK}");
            newLineStrings.Add("{BACKSPACE}");
            newLineStrings.Add("{TAB}");
            newLineStrings.Add("{ENTER}");
            newLineStrings.Add("{CAPSLOCK}");
            newLineStrings.Add("{HOME}");
            newLineStrings.Add("{END}");
            newLineStrings.Add("{DELETE}");
            newLineStrings.Add("{PGUP}");
            newLineStrings.Add("{PGDN}");
            newLineStrings.Add("{LEFT}");
            newLineStrings.Add("{UP}");
            newLineStrings.Add("{DOWN}");
            newLineStrings.Add("{RIGHT}");
            newLineStrings.Add("{NUMLOCK}");
        }

        private void addHookedKeys()
        {
            // + Shift		^ Ctrl		% Alt
            #region row 1
            hookedKeys.Add("Escape", "{ESC}");
            hookedKeys.Add("F1", "{F1}");
            hookedKeys.Add("F2", "{F2}");
            hookedKeys.Add("F3", "{F3}");
            hookedKeys.Add("F4", "{F4}");
            hookedKeys.Add("F5", "{F5}");
            hookedKeys.Add("F6", "{F6}");
            hookedKeys.Add("F7", "{F7}");
            hookedKeys.Add("F8", "{F8}");
            hookedKeys.Add("F9", "{F9}");
            hookedKeys.Add("F10", "{F10}");
            hookedKeys.Add("F11", "{F11}");
            hookedKeys.Add("F12", "{F12}");
            hookedKeys.Add("Insert", "{INSERT}");
            hookedKeys.Add("PrintScreen", "{PRTSC}");
            hookedKeys.Add("Pause", "{BREAK}");
            #endregion

            #region row 2
            hookedKeys.Add("Oem5", "{^}");
            hookedKeys.Add("+Oem5", "°");

            hookedKeys.Add("D1", "1");
            hookedKeys.Add("+D1", "!");

            hookedKeys.Add("D2", "2");
            hookedKeys.Add("+D2", "\"");
            hookedKeys.Add("^%D2", "²");

            hookedKeys.Add("D3", "3");
            hookedKeys.Add("+D3", "§");
            hookedKeys.Add("^%D3", "³");

            hookedKeys.Add("D4", "4");
            hookedKeys.Add("+D4", "$");

            hookedKeys.Add("D5", "5");
            hookedKeys.Add("+D5", "{%}");

            hookedKeys.Add("D6", "6");
            hookedKeys.Add("+D6", "&");

            hookedKeys.Add("D7", "7");
            hookedKeys.Add("+D7", "/");
            hookedKeys.Add("^%D7", "{{}");

            hookedKeys.Add("D8", "8");
            hookedKeys.Add("+D8", "{(}");
            hookedKeys.Add("^%D8", "{[}");

            hookedKeys.Add("D9", "9");
            hookedKeys.Add("+D9", "{)}");
            hookedKeys.Add("^%D9", "{]}");

            hookedKeys.Add("D0", "0");
            hookedKeys.Add("+D0", "=");
            hookedKeys.Add("^%D0", "{}}");

            hookedKeys.Add("OemOpenBrackets", "ß");
            hookedKeys.Add("+OemOpenBrackets", "?");
            hookedKeys.Add("^%OemOpenBrackets", "\\");

            hookedKeys.Add("Oem6", "´");
            hookedKeys.Add("+Oem6", "`");

            hookedKeys.Add("Back", "{BACKSPACE}");
            #endregion

            #region row 3
            hookedKeys.Add("Tab", "{TAB}");

            hookedKeys.Add("Q", "q");
            hookedKeys.Add("+Q", "Q");
            hookedKeys.Add("^%Q", "@");

            hookedKeys.Add("W", "w");
            hookedKeys.Add("+W", "W");

            hookedKeys.Add("E", "e");
            hookedKeys.Add("+E", "E");
            hookedKeys.Add("^%E", "€");

            hookedKeys.Add("R", "r");
            hookedKeys.Add("+R", "R");

            hookedKeys.Add("T", "t");
            hookedKeys.Add("+T", "T");

            hookedKeys.Add("Z", "z");
            hookedKeys.Add("+Z", "Z");

            hookedKeys.Add("U", "u");
            hookedKeys.Add("+U", "U");

            hookedKeys.Add("I", "i");
            hookedKeys.Add("+I", "I");

            hookedKeys.Add("O", "o");
            hookedKeys.Add("+O", "O");

            hookedKeys.Add("P", "p");
            hookedKeys.Add("+P", "P");

            hookedKeys.Add("Oem1", "ü");
            hookedKeys.Add("+Oem1", "Ü");

            hookedKeys.Add("Oemplus", "{+}");
            hookedKeys.Add("+Oemplus", "*");
            hookedKeys.Add("^%Oemplus", "{~}");

            hookedKeys.Add("Return", "{ENTER}");
            #endregion

            #region row 4
            hookedKeys.Add("Capital", "{CAPSLOCK}");

            hookedKeys.Add("A", "a");
            hookedKeys.Add("+A", "A");

            hookedKeys.Add("S", "s");
            hookedKeys.Add("+S", "S");

            hookedKeys.Add("D", "d");
            hookedKeys.Add("+D", "D");

            hookedKeys.Add("F", "f");
            hookedKeys.Add("+F", "F");

            hookedKeys.Add("G", "g");
            hookedKeys.Add("+G", "G");

            hookedKeys.Add("H", "h");
            hookedKeys.Add("+H", "H");

            hookedKeys.Add("J", "j");
            hookedKeys.Add("+J", "J");

            hookedKeys.Add("K", "k");
            hookedKeys.Add("+K", "K");

            hookedKeys.Add("L", "l");
            hookedKeys.Add("+L", "L");

            hookedKeys.Add("Oemtilde", "ö");
            hookedKeys.Add("+Oemtilde", "Ö");

            hookedKeys.Add("Oem7", "ä");
            hookedKeys.Add("+Oem7", "Ä");

            hookedKeys.Add("OemQuestion", "#");
            hookedKeys.Add("+OemQuestion", "'");
            #endregion

            #region row 5
            hookedKeys.Add("OemBackslash", "<");
            hookedKeys.Add("+OemBackslash", ">");
            hookedKeys.Add("^%OemBackslash", "|");

            hookedKeys.Add("Y", "y");
            hookedKeys.Add("+Y", "Y");

            hookedKeys.Add("X", "x");
            hookedKeys.Add("+X", "X");

            hookedKeys.Add("C", "c");
            hookedKeys.Add("+C", "C");

            hookedKeys.Add("V", "v");
            hookedKeys.Add("+V", "V");

            hookedKeys.Add("B", "b");
            hookedKeys.Add("+B", "B");

            hookedKeys.Add("N", "n");
            hookedKeys.Add("+N", "N");

            hookedKeys.Add("M", "m");
            hookedKeys.Add("+M", "M");
            hookedKeys.Add("^%M", "µ");

            hookedKeys.Add("Oemcomma", ",");
            hookedKeys.Add("+Oemcomma", ";");

            hookedKeys.Add("OemPeriod", ".");
            hookedKeys.Add("+OemPeriod", ":");

            hookedKeys.Add("OemMinus", "-");
            hookedKeys.Add("+OemMinus", "_");
            #endregion

            #region Others
            //hookedKeys.Add("LWin", "{HOME}");
            //hookedKeys.Add("RWin", "{HOME}");
            //hookedKeys.Add("Apps", "");
            hookedKeys.Add("Space", " ");

            hookedKeys.Add("Home", "{HOME}"); //POS1
            hookedKeys.Add("End", "{END}"); //ENDE
            hookedKeys.Add("Delete", "{DELETE}");
            hookedKeys.Add("PageUp", "{PGUP}"); //BILD-AUF
            hookedKeys.Add("Next", "{PGDN}"); //BILD-AB

            hookedKeys.Add("Left", "{LEFT}");
            hookedKeys.Add("Up", "{UP}");
            hookedKeys.Add("Down", "{DOWN}");
            hookedKeys.Add("Right", "{RIGHT}");
            #endregion

            #region NumPad
            hookedKeys.Add("NumLock", "{NUMLOCK}");

            hookedKeys.Add("NumPad0", "0");
            hookedKeys.Add("+NumPad0", "{INSERT}");

            hookedKeys.Add("NumPad1", "1");
            hookedKeys.Add("+NumPad1", "{END}");

            hookedKeys.Add("NumPad2", "2");
            hookedKeys.Add("+NumPad2", "{DOWN}");

            hookedKeys.Add("NumPad3", "3");
            hookedKeys.Add("+NumPad3", "{PGDN}");

            hookedKeys.Add("NumPad4", "4");
            hookedKeys.Add("+NumPad4", "{LEFT}");

            hookedKeys.Add("NumPad5", "5");

            hookedKeys.Add("NumPad6", "6");
            hookedKeys.Add("+NumPad6", "{RIGHT}");

            hookedKeys.Add("NumPad7", "7");
            hookedKeys.Add("+NumPad7", "{HOME}");

            hookedKeys.Add("NumPad8", "8");
            hookedKeys.Add("+NumPad8", "{UP}");

            hookedKeys.Add("NumPad9", "9");
            hookedKeys.Add("+NumPad9", "{PGUP}");

            hookedKeys.Add("Decimal", ",");
            hookedKeys.Add("+Decimal", "{DELETE}");

            hookedKeys.Add("Add", "{ADD}");
            hookedKeys.Add("Substract", "{SUBTRACT}");
            hookedKeys.Add("Multiply", "{MULTIPLY}");
            hookedKeys.Add("Divide", "{DIVIDE}");
            #endregion
        }
            
        private void setDefaultTexts()
        {
            credits = "Copyright © " + Application.CompanyName + " 2016-" + DateTime.Now.Year + "\n"
                + "\nEmail:    topasios92@gmail.com"
                + "\nHomepage: http://topas.square7.ch/";

            assemblynames = "mscorlib.dll\n"
                + "System.dll\n"
                + "System.Core.dll\n"
                + "System.Windows.Forms.dll\n"
                + "System.Drawing.dll";

            setDefaultReferenceText();
            setDefaultHookManagerText();
        }

        private void setDefaultReferenceText()
        {
            #region Globals

            references = @"// This Text will be added to the code at the position right before 'public void Main('
// Edit with caution.

[System.Runtime.InteropServices.DllImport(""user32.dll"")]
static extern void mouse_event(System.Int32 dwFlags, System.Int32 dx, System.Int32 dy, System.Int32 dwData, System.Int32 dwExtraInfo);

[System.Runtime.InteropServices.DllImport(""user32.dll"")]
static extern System.IntPtr GetForegroundWindow();

[System.Runtime.InteropServices.DllImport(""user32.dll"")]
static extern System.Boolean SetForegroundWindow(System.IntPtr hwnd);

[System.Runtime.InteropServices.DllImport(""user32.dll"")]
static extern System.Int32 GetWindowText(System.IntPtr hWnd, System.Text.StringBuilder text, System.Int32 count);

[System.Runtime.InteropServices.DllImport(""user32.dll"")]
static extern System.IntPtr GetDC(System.IntPtr hwnd);

[System.Runtime.InteropServices.DllImport(""user32.dll"")]
static extern System.Int32 ReleaseDC(System.IntPtr hwnd, System.IntPtr hdc);

[System.Runtime.InteropServices.DllImport(""gdi32.dll"")]
static extern System.UInt32 GetPixel(System.IntPtr hdc, System.Int32 nXPos, System.Int32 nYPos);

System.UInt32 doubleClickTime = " + doubleClickTime + @";

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

";

            #endregion

            #region General

            references += @"static System.String GetActiveWindowTitle()
{
    var handle = System.IntPtr.Zero;
    var buffer = new System.Text.StringBuilder(256);
    handle = GetForegroundWindow();
    if (GetWindowText(handle, buffer, 256) > 0)
    {
        return buffer.ToString();
    }
    return null;
}

static System.Drawing.Color GetPixelColor(System.Int32 x, System.Int32 y)
{
    System.IntPtr hdc = GetDC(System.IntPtr.Zero);
    System.UInt32 pixel = GetPixel(hdc, x, y);
    ReleaseDC(System.IntPtr.Zero, hdc);
    System.Drawing.Color color = System.Drawing.Color.FromArgb((System.Int32)(pixel & 0x000000FF),
                    (System.Int32)(pixel & 0x0000FF00) >> 8,
                    (System.Int32)(pixel & 0x00FF0000) >> 16);
    return color;
}

public System.Boolean MakeWindowActive(System.String windowTitle)
{
    System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();

    foreach (System.Diagnostics.Process p in processList)
    {
        if (p.MainWindowTitle == windowTitle)
        {
            SetForegroundWindow(p.MainWindowHandle);
            return true;
        }
    }
    return false;
}

";

            #endregion

            #region Wait

            references += @"System.Boolean go = false;
System.Int32 ms;
public void Wait(System.Int32 milliseconds)
{
    ms = milliseconds;
    go = false;
    System.Threading.Thread T = new System.Threading.Thread(new System.Threading.ThreadStart(Thread_Wait));
    T.Start();
    do
    {
        System.Windows.Forms.Application.DoEvents();
    } while (go == false && !ABORT);
    T.Abort();
}

void Thread_Wait()
{
    System.Threading.Thread.Sleep(ms);
    go = true;
}

public System.Boolean WaitForColor(System.Int32 r, System.Int32 g, System.Int32 b, System.Int32 x, System.Int32 y)
{
    System.Int32 time = 0;
    System.Int32 timeout = 0;
    System.Int32 intervall = 1000;
    System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
    while (GetPixelColor(x, y) != color && (time < timeout || timeout == 0) && !ABORT)
    {
        Wait(intervall);
        time += intervall;
    }

    Wait(100);
    if (time >= timeout && timeout != 0)
        return false;
    else
        return true;
}

public System.Boolean WaitForColor(System.Int32 r, System.Int32 g, System.Int32 b, System.Int32 x, System.Int32 y, System.Int32 timeout, System.Int32 intervall)
{
    System.Int32 time = 0;
    System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
    while (GetPixelColor(x, y) != color && (time < timeout || timeout == 0) && !ABORT)
    {
        Wait(intervall);
        time += intervall;
    }

    Wait(100);
    if (time >= timeout && timeout != 0)
        return false;
    else
        return true;
}

public System.Boolean WaitForActiveWindow(System.String title)
{
    System.Int32 time = 0;
    System.Int32 timeout = 0;
    System.Int32 intervall = 1000;
    while (GetActiveWindowTitle() != title && (time < timeout || timeout == 0) && !ABORT)
    {
        Wait(intervall);
        time += intervall;
    }

    Wait(100);
    if (time >= timeout && timeout != 0)
        return false;
    else
        return true;
}

public System.Boolean WaitForActiveWindow(System.String title, System.Int32 timeout, System.Int32 intervall)
{
    System.Int32 time = 0;
    while (GetActiveWindowTitle() != title && (time < timeout || timeout == 0) && !ABORT)
    {
        Wait(intervall);
        time += intervall;
    }

    Wait(100);
    if (time >= timeout && timeout != 0)
        return false;
    else
        return true;
}


private System.Boolean WaitForKey(System.String key)
{
    keyToWaitFor = key;
    System.Int32 time = 0;
    System.Int32 timeout = 0;
    System.Int32 intervall = 1000;
    while (lastKey != key && (time < timeout || timeout == 0) && !ABORT)
    {
        Wait(intervall);
        time += intervall;
    }

    Wait(100);
    keyToWaitFor = System.String.Empty;
    lastKey = System.String.Empty;
    if (time >= timeout && timeout != 0)
        return false;
    else
        return true;
}

private System.Boolean WaitForKey(System.String key, System.Int32 timeout, System.Int32 intervall)
{
    keyToWaitFor = key;
    System.Int32 time = 0;
    while (lastKey != key && (time < timeout || timeout == 0) && !ABORT)
    {
        Wait(intervall);
        time += intervall;
    }

    Wait(100);
    keyToWaitFor = System.String.Empty;
    lastKey = System.String.Empty;
    if (time >= timeout && timeout != 0)
        return false;
    else
        return true;
}

";

            #endregion

            #region KeyBoard

            references += @"static bool sendKey = false;

void KeyBoard(System.String text)
{

    if (!ABORT)
    {
        sendKey = true;
        System.Windows.Forms.SendKeys.SendWait(text);
        Wait(20);
        sendKey = false;
    }
}

";

            #endregion

            #region Click

            references += @"void LClick(System.Int32 x, System.Int32 y)
{
    if (!ABORT)
    {
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.LEFTDOWN), 0, 0, 0, 0);
        Wait(50);
        mouse_event((System.Int32)(MouseActionAdresses.LEFTUP), 0, 0, 0, 0);
        Wait((System.Int32)doubleClickTime - 50);
    }
}
        
void RClick(System.Int32 x, System.Int32 y)
{
    if (!ABORT)
    {
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.RIGHTDOWN), 0, 0, 0, 0);
        Wait(50);
        mouse_event((System.Int32)(MouseActionAdresses.RIGHTUP), 0, 0, 0, 0);
        Wait((System.Int32)doubleClickTime - 50);
    }
}

void MClick(System.Int32 x, System.Int32 y)
{
    if (!ABORT)
    {
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.MIDDLEDOWN), 0, 0, 0, 0);
        Wait(50);
        mouse_event((System.Int32)(MouseActionAdresses.MIDDLEUP), 0, 0, 0, 0);
        Wait((System.Int32)doubleClickTime - 50);
    }
}

";

            #endregion

            #region Hold

            references += @"void LHold(System.Int32 xDown, System.Int32 yDown, System.Int32 xUp, System.Int32 yUp)
{
    if (!ABORT)
    {
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(xDown, yDown);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.LEFTDOWN), 0, 0, 0, 0);
        Wait(50);
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(xUp, yUp);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.LEFTUP), 0, 0, 0, 0);
        Wait((System.Int32)doubleClickTime - 150);
    }
}

void RHold(System.Int32 xDown, System.Int32 yDown, System.Int32 xUp, System.Int32 yUp)
{
    if (!ABORT)
    {
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(xDown, yDown);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.RIGHTDOWN), 0, 0, 0, 0);
        Wait(50);
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(xUp, yUp);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.RIGHTUP), 0, 0, 0, 0);
        Wait((System.Int32)doubleClickTime - 150);
    }
}

void MHold(System.Int32 xDown, System.Int32 yDown, System.Int32 xUp, System.Int32 yUp)
{
    if (!ABORT)
    {
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(xDown, yDown);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.MIDDLEDOWN), 0, 0, 0, 0);
        Wait(50);
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(xUp, yUp);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.MIDDLEUP), 0, 0, 0, 0);
        Wait((System.Int32)doubleClickTime - 150);
    }
}

";

            #endregion

            #region DoubleClick

            references += @"void LDoubleClick(System.Int32 x, System.Int32 y)
{
    if (!ABORT)
    {
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.LEFTDOWN), 0, 0, 0, 0);
        Wait(50);
        mouse_event((System.Int32)(MouseActionAdresses.LEFTUP), 0, 0, 0, 0);
        Wait((System.Int32)doubleClickTime / 2);
        mouse_event((System.Int32)(MouseActionAdresses.LEFTDOWN), 0, 0, 0, 0);
        Wait(50);
        mouse_event((System.Int32)(MouseActionAdresses.LEFTUP), 0, 0, 0, 0);
        Wait(50);
    }
}

void RDoubleClick(System.Int32 x, System.Int32 y)
{
    if (!ABORT)
    {
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.RIGHTDOWN), 0, 0, 0, 0);
        Wait(50);
        mouse_event((System.Int32)(MouseActionAdresses.RIGHTUP), 0, 0, 0, 0);
        Wait((System.Int32)doubleClickTime / 2);
        mouse_event((System.Int32)(MouseActionAdresses.RIGHTDOWN), 0, 0, 0, 0);
        Wait(50);
        mouse_event((System.Int32)(MouseActionAdresses.RIGHTUP), 0, 0, 0, 0);
        Wait(50);
    }
}

void MDoubleClick(System.Int32 x, System.Int32 y)
{
    if (!ABORT)
    {
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
        Wait(100);
        mouse_event((System.Int32)(MouseActionAdresses.MIDDLEDOWN), 0, 0, 0, 0);
        Wait(50);
        mouse_event((System.Int32)(MouseActionAdresses.MIDDLEUP), 0, 0, 0, 0);
        Wait((System.Int32)doubleClickTime / 2);
        mouse_event((System.Int32)(MouseActionAdresses.MIDDLEDOWN), 0, 0, 0, 0);
        Wait(50);
        mouse_event((System.Int32)(MouseActionAdresses.MIDDLEUP), 0, 0, 0, 0);
        Wait(50);
    }
}";

            #endregion
        }

        private void setDefaultHookManagerText()
        {
            hookmanager = @"// This Text will be added to the code at the position right before 'public void Main('
// Edit with caution.

static bool ABORT = false;
static System.Windows.Forms.Keys interruptKey = System.Windows.Forms.Keys.Escape;
static string lastKey, keyToWaitFor;

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
[System.Runtime.InteropServices.DllImport(""user32.dll"", CharSet = System.Runtime.InteropServices.CharSet.Auto,
    CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
private static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
System.IntPtr hInstance, int threadId);

//Import for UnhookWindowsHookEx.
//Call this function to uninstall the hook.
[System.Runtime.InteropServices.DllImport(""user32.dll"", CharSet = System.Runtime.InteropServices.CharSet.Auto,
    CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
private static extern bool UnhookWindowsHookEx(int idHook);

//Import for CallNextHookEx.
//Use this function to pass the hook information to next hook procedure in chain.
[System.Runtime.InteropServices.DllImport(""user32.dll"", CharSet = System.Runtime.InteropServices.CharSet.Auto,
    CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
private static extern int CallNextHookEx(int idHook, int nCode,
System.IntPtr wParam, System.IntPtr lParam);

[System.Runtime.InteropServices.DllImport(""kernel32.dll"")]
static extern System.IntPtr LoadLibrary(string lpFileName);

private delegate int HookProc(int nCode, System.IntPtr wParam, System.IntPtr lParam);

public static bool Hook()
{
    ABORT = false;
    KeyboardHookProcedure = new HookProc(KeyboardHookProc);

    hHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, (System.IntPtr)LoadLibrary(""User32""), 0);
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
        if (((int)wParam == 256) || ((int)wParam == 260)) //WM_KEYDOWN || WM_SYSKEYDOWN
        {
            if (!sendKey)
            {
                keyboardHookStruct MyKeyboardHookStruct = (keyboardHookStruct)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(keyboardHookStruct));
                if ((System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode == interruptKey)
                {
                    UnHook();
                    ABORT = true;                
                    //System.Environment.Exit(0);
                    //System.Windows.Forms.Application.Exit();
                }
                else
                    lastKey = ((System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode).ToString();

                //System.Windows.Forms.MessageBox.Show(((System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode).ToString());
            }
        }

        if (System.String.IsNullOrEmpty(keyToWaitFor) || lastKey != keyToWaitFor)
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        else
            return 1;
    }
}";

        }

        #region Wait()
        bool go = false;
        int ms;
        /// <summary>
        /// Method for sleep without program stop
        /// </summary>
        /// <param name="ms">Time to wait in milliseconds</param>
        private void Wait(int milliseconds)
        {
            ms = milliseconds;
            go = false;
            System.Threading.Thread T = new System.Threading.Thread(new System.Threading.ThreadStart(Thread_Wait));
            T.Start();
            do
            {
                System.Windows.Forms.Application.DoEvents();
            } while (go == false);
        }

        string lastKey;
        private bool WaitForKey(string key, int timeout = 0, int intervall = 1000)
        {
            int time = 0;
            while (lastKey != key && (time < timeout || timeout == 0))
            {
                Wait(intervall);
                time += intervall;
            }

            Wait(100);
            if (time >= timeout && timeout != 0)
                return false;
            else
                return true;
        }

        private bool WaitForColor(int r, int g, int b, int x, int y, int timeout = 0, int intervall = 1000)
        {
            int time = 0;
            System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
            while (HookManager.GetPixelColor(x, y) != color && (time < timeout || timeout == 0))
            {
                Wait(intervall);
                time += intervall;
            }

            Wait(100);
            if (time >= timeout && timeout != 0)
                return false;
            else
                return true;
        }

        private bool WaitForActiveWindow(string title, int timeout = 0, int intervall = 1000)
        {
            int time = 0;
            while (GetActiveWindowTitle() != title && (time < timeout || timeout == 0))
            {
                Wait(intervall);
                time += intervall;
            }

            Wait(100);
            if (time >= timeout && timeout != 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Wait Thread, called by Wait(int milliseconds)
        /// </summary>
        private void Thread_Wait()
        {
            Thread.Sleep(ms);
            go = true;
        }
        #endregion

        private string GetActiveWindowTitle()
        {
            var handle = System.IntPtr.Zero;
            var buffer = new System.Text.StringBuilder(256);
            handle = GetForegroundWindow();
            if (GetWindowText(handle, buffer, 256) > 0)
            {
                return buffer.ToString();
            }
            return null;
        }

        public void MouseHookProc(IntPtr wParam, IntPtr lParam)
        {
            if (recording || select_color || select_coordinate || select_window)
            {
                HookManager.mouseHookStruct hookStruct = (HookManager.mouseHookStruct)Marshal.PtrToStructure(lParam, typeof(HookManager.mouseHookStruct));

                if ((hookStruct.pt.x < this.Location.X || hookStruct.pt.x >= this.Location.X + this.Size.Width) || (hookStruct.pt.y < this.Location.Y || hookStruct.pt.y >= this.Location.Y + this.Size.Height) || !this.ContainsFocus)
                {
                    switch ((HookManager.MouseMessages)wParam)
                    {
                        case HookManager.MouseMessages.WM_LBUTTONDOWN:
                            if (!select_color && !select_coordinate && !select_window)
                                lastDown = hookStruct.pt;
                            break;
                        case HookManager.MouseMessages.WM_LBUTTONUP:
                            if (select_color)
                            {
                                Color c = HookManager.GetPixelColor(hookStruct.pt.x, hookStruct.pt.y);

                                if (waitSelect)
                                    ScintillaWriteLine(c.R + ", " + c.G + ", " + c.B + ", " + hookStruct.pt.x + ", " + hookStruct.pt.y + ");");
                                else
                                    ScintillaWrite(c.R + ", " + c.G + ", " + c.B + ", " + hookStruct.pt.x + ", " + hookStruct.pt.y);

                                waitSelect = false;
                                pibo_select_color_Click(null, null);
                            }
                            else if (select_coordinate)
                            {
                                ScintillaWrite(hookStruct.pt.x + ", " + hookStruct.pt.y);
                                pibo_select_coordinate_Click(null, null);
                            }
                            else if (select_window)
                            {
                                if (waitSelect)
                                    ScintillaWriteLine(GetActiveWindowTitle() + "\");");
                                else
                                    ScintillaWrite(GetActiveWindowTitle());

                                waitSelect = false;
                                pibo_select_window_Click(null, null);
                            }
                            else
                                ScintillaWriteClickLine("LClick", DateTime.Now.ToFileTime(), hookStruct.pt);
                            break;
                        case HookManager.MouseMessages.WM_RBUTTONDOWN:
                            if (!select_color && !select_coordinate && !select_window)
                                lastDown = hookStruct.pt;
                            break;
                        case HookManager.MouseMessages.WM_RBUTTONUP:
                            if (!select_color && !select_coordinate && !select_window)
                                ScintillaWriteClickLine("RClick", DateTime.Now.ToFileTime(), hookStruct.pt);
                            break;
                        case HookManager.MouseMessages.WM_MBUTTONDOWN:
                            if (!select_color && !select_coordinate && !select_window)
                                lastDown = hookStruct.pt;
                            break;
                        case HookManager.MouseMessages.WM_MBUTTONUP:
                            if (!select_color && !select_coordinate && !select_window)
                                ScintillaWriteClickLine("MClick", DateTime.Now.ToFileTime(), hookStruct.pt);
                            break;
                        case HookManager.MouseMessages.WM_MOUSEWHEEL:
                            break;
                        case HookManager.MouseMessages.WM_MOUSEMOVE:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void KeyboardHookProc(IntPtr wParam, IntPtr lParam)
        {
            if (((int)wParam == 256) || ((int)wParam == 260)) //WM_KEYDOWN || WM_SYSKEYDOWN
            {
                HookManager.keyboardHookStruct MyKeyboardHookStruct = (HookManager.keyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(HookManager.keyboardHookStruct));
                if (((Keys)MyKeyboardHookStruct.vkCode).ToString() == "LShiftKey" |
                    ((Keys)MyKeyboardHookStruct.vkCode).ToString() == "RShiftKey")
                {
                    shift = "+";
                }
                else if (((Keys)MyKeyboardHookStruct.vkCode).ToString() == "LControlKey" |
                        ((Keys)MyKeyboardHookStruct.vkCode).ToString() == "RControlKey")
                {
                    ctrl = "^";
                }
                else if (((Keys)MyKeyboardHookStruct.vkCode).ToString() == "LMenu" |
                        ((Keys)MyKeyboardHookStruct.vkCode).ToString() == "RMenu")
                {
                    alt = "%";
                }
                else if (recording || select_key)
                {
                    if (hookedKeys.ContainsKey(((Keys)MyKeyboardHookStruct.vkCode).ToString()))
                    {
                        if (!string.IsNullOrEmpty(input) && !select_key && newLineStrings.Contains(hookedKeys[((Keys)MyKeyboardHookStruct.vkCode).ToString()]))
                        {
                            ScintillaWriteKeyboardLine(input);
                            input = "";
                        }

                        if (select_key)
                            ScintillaWrite(((Keys)MyKeyboardHookStruct.vkCode).ToString());
                        else if (newLineStrings.Contains(hookedKeys[((Keys)MyKeyboardHookStruct.vkCode).ToString()]))
                            ScintillaWriteKeyboardLine(shift + ctrl + alt + hookedKeys[((Keys)MyKeyboardHookStruct.vkCode).ToString()]);
                        else
                            input += shift + ctrl + alt + hookedKeys[((Keys)MyKeyboardHookStruct.vkCode).ToString()];
                    }
                    else
                        ScintillaWriteLine("// Key not in hookedKeys dictionary: " + shift + ctrl + alt + " " + ((Keys)MyKeyboardHookStruct.vkCode).ToString());

                    if (select_key)
                    {
                        HookManager.UnHook();
                        shift = "";
                        ctrl = "";
                        alt = "";

                        select_key = false;
                        ScintillaWriteLine("\");");

                        pibo_compile.Visible = true;
                        pibo_open.Visible = true;
                        pibo_save.Visible = true;
                        pibo_record.Visible = true;
                        pibo_select_coordinate.Visible = true;
                        pibo_select_color.Visible = true;
                        pibo_select_window.Visible = true;
                        pibo_wait_time.Visible = true;
                        pibo_wait_color.Visible = true;
                        pibo_wait_window.Visible = true;
                        pibo_wait_key.Visible = true;
                    }
                }
            }
            else if (((int)wParam == 257) || ((int)wParam == 261)) //WM_KEYUP || WM_SYSKEYUP
            {
                HookManager.keyboardHookStruct MyKeyboardHookStruct = (HookManager.keyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(HookManager.keyboardHookStruct));
                if (((Keys)MyKeyboardHookStruct.vkCode).ToString() == "LShiftKey" |
                    ((Keys)MyKeyboardHookStruct.vkCode).ToString() == "RShiftKey")
                {
                    shift = "";
                }
                else if (((Keys)MyKeyboardHookStruct.vkCode).ToString() == "LControlKey" |
                        ((Keys)MyKeyboardHookStruct.vkCode).ToString() == "RControlKey")
                {
                    ctrl = "";
                }
                else if (((Keys)MyKeyboardHookStruct.vkCode).ToString() == "LMenu" |
                        ((Keys)MyKeyboardHookStruct.vkCode).ToString() == "RMenu")
                {
                    alt = "";
                }
            }
        }

        private void ScintillaWriteKeyboardLine(string keys)
        {
            ScintillaWriteLine("KeyBoard(\"" + keys + "\");");
        }

        private void ScintillaWriteClickLine(string type, long time, HookManager.POINT point)
        {
            if (!string.IsNullOrEmpty(input))
            {
                ScintillaWriteKeyboardLine(input);
                input = "";
            }

            if (Math.Abs(point.x - lastDown.x) > 5 || Math.Abs(point.y - lastDown.y) > 5)
            {
                type = type.Replace("Click", "Hold");

                ScintillaWriteLine(type + "(" + lastDown.x + ", " + lastDown.y + ", " + point.x + ", " + point.y + ");");

                return;
            }
            else if ((Math.Abs(time - lastClickTime) / 10000) < doubleClickTime && Math.Abs(point.x - lastClick.x) <= 5 && Math.Abs(point.y - lastClick.y) <= 5)
            {
                type = type.Replace("Click", "DoubleClick");

                ScintillaDeletePrevLine();

                ScintillaWriteLine(type + "(" + point.x + ", " + point.y + ");");

                lastClickTime = 0;
            }
            else
            {
                ScintillaWriteLine(type + "(" + point.x + ", " + point.y + ");");

                lastClickTime = DateTime.Now.ToFileTime();
                lastClick = point;
            }
        }

        private void ScintillaDeletePrevLine()
        {
            string txt = scintilla.GetTextRange(0, scintilla.CurrentPosition);
            int deleteEndIndex = txt.LastIndexOf("\n");
            txt = txt.Substring(0, txt.LastIndexOf("\n"));
            int deleteStartIndex = txt.LastIndexOf("\n");

            scintilla.DeleteRange(deleteStartIndex, deleteEndIndex  - deleteStartIndex);
        }

        private void ScintillaWrite(string text)
        {
            scintilla.InsertText(scintilla.CurrentPosition, text);
            scintilla.CurrentPosition += text.Length;
            scintilla.AnchorPosition += text.Length;
        }

        private void ScintillaNewLine()
        {
            scintilla.InsertText(scintilla.CurrentPosition, "\n");
            int newCursorPosition = scintilla.GetTextRange(scintilla.CurrentPosition + 1, scintilla.TextLength - (scintilla.CurrentPosition + 1)).IndexOf("\n");

            scintilla.CurrentPosition += newCursorPosition;
            scintilla.AnchorPosition = scintilla.CurrentPosition;
        }

        private void ScintillaWriteLine(string text)
        {
            ScintillaWrite(text);

            ScintillaNewLine();
        }

        private void ScintillaSaveChangesToStrings()
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    code = scintilla.Text;
                    break;
                case 1:
                    references = scintilla.Text;
                    break;
                case 2:
                    hookmanager = scintilla.Text;
                    break;
                case 3:
                    assemblynames = scintilla.Text;
                    break;
            }
        }
        #endregion

        #region Events

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            pibo_compile.Location = new Point(this.Size.Width - 55, pibo_compile.Location.Y);
            pibo_save.Location = new Point(this.Size.Width - 55, pibo_save.Location.Y);
            pibo_open.Location = new Point(this.Size.Width - 55, pibo_open.Location.Y);
            pibo_record.Location = new Point(this.Size.Width - 55, pibo_record.Location.Y);
            pibo_select_coordinate.Location = new Point(this.Size.Width - 55, pibo_select_coordinate.Location.Y);
            pibo_select_color.Location = new Point(this.Size.Width - 55, pibo_select_color.Location.Y);
            pibo_select_window.Location = new Point(this.Size.Width - 55, pibo_select_window.Location.Y);
            pibo_wait_time.Location = new Point(this.Size.Width - 55, pibo_wait_time.Location.Y);
            pibo_wait_color.Location = new Point(this.Size.Width - 55, pibo_wait_color.Location.Y);
            pibo_wait_window.Location = new Point(this.Size.Width - 55, pibo_wait_window.Location.Y);
            pibo_wait_key.Location = new Point(this.Size.Width - 55, pibo_wait_key.Location.Y);

            tabControl1.Size = new Size(this.Size.Width - 60, tabControl1.Height);
            scintilla.Size = new Size(this.Size.Width - 60, this.Height - scintilla.Location.Y - 39);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            int NULL = 0;
            SystemParametersInfo((uint)SystemParametersInfoAction.SPI_SETCURSORS, 0, ref NULL, 0);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControleSelectedIndex)
            {
                case 0:
                    code = scintilla.Text;
                    break;
                case 1:
                    references = scintilla.Text;
                    break;
                case 2:
                    hookmanager = scintilla.Text;
                    break;
                case 3:
                    assemblynames = scintilla.Text;
                    break;
            }
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    scintilla.Text = code;
                    break;
                case 1:
                    scintilla.Text = references;
                    break;
                case 2:
                    scintilla.Text = hookmanager;
                    break;
                case 3:
                    scintilla.Text = assemblynames;
                    break;
                case 4:
                    scintilla.Text = credits;
                    break;
            }
            tabControleSelectedIndex = tabControl1.SelectedIndex;
        }

        private void pibo_compile_Click(object sender, EventArgs e)
        {
            if (compiled == 0 && !recording && !select_color && !select_coordinate && !select_window && !select_key)
            {
                #region Compile

                CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });

                string[] assemblyNames = assemblynames.Split(new[] { "\r\n", "\n\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                CompilerParameters compilerParams = new CompilerParameters(assemblyNames);
                compilerParams.GenerateInMemory = true;
                compilerParams.GenerateExecutable = false;

                ScintillaSaveChangesToStrings();

                string source = code;
                
                source = source.Replace("public void Main(", references + "\n\n" + hookmanager + "\n\n        public void Main(");

                int indexMain = source.IndexOf("public void Main()");
                List<int> openBrace = source.AllIndexesOf("{");
                List<int> closeBrace = source.AllIndexesOf("}");

                int indexMainOpenBrace = 0;
                int j;
                for (j = 0; j < openBrace.Count; j++)
                    if (openBrace[j] > indexMain)
                    {
                        indexMainOpenBrace = openBrace[j];
                        break;
                    }

                int indexMainCloseBrace = 0;
                for (int i = 0; i < closeBrace.Count; i++)
                {
                    if (closeBrace[i] > indexMainOpenBrace)
                    {
                        if (openBrace.Count == j + 1 || closeBrace[i] < openBrace[j + 1])
                        {
                            indexMainCloseBrace = closeBrace[i];
                            break;
                        }
                        else
                            j++;
                    }
                }

                source = source.Insert(indexMainCloseBrace, "UnHook();");
                source = source.Insert(indexMainOpenBrace + 1, "Hook();");

                //scintilla.Text = source;

                compilerResults = provider.CompileAssemblyFromSource(compilerParams, source);

                if (compilerResults.Errors.Count != 0)
                {
                    pibo_compile.Image = Properties.Resources.StepForwardNormalOrange;
                    toolTip1.SetToolTip(pibo_compile, "Recompile");
                    CompileErrors ce = new CompileErrors(compilerResults.Errors);
                    ce.ShowDialog();
                    compilerResults = null;
                    return;
                }

                toolTip1.SetToolTip(pibo_compile, "Run");
                pibo_compile.Image = Properties.Resources.StepForwardHot;
                compiled = 1;

                #endregion
            }
            else if (compiled == 1 && !recording && !select_color && !select_coordinate && !select_window)
            {
                #region Run

                object o = compilerResults.CompiledAssembly.CreateInstance("AAS.Class");
                MethodInfo mi = o.GetType().GetMethod("Main");

                pibo_compile.Image = Properties.Resources.Run;
                toolTip1.SetToolTip(pibo_compile, "Running");
                compiled = 2;

                scintilla.Enabled = false;
                tabControl1.Enabled = false;
                pibo_compile.Enabled = false;
                pibo_save.Enabled = false;
                pibo_open.Enabled = false;
                pibo_select_coordinate.Enabled = false;
                pibo_select_color.Enabled = false;
                pibo_select_window.Enabled = false;
                pibo_wait_time.Enabled = false;
                pibo_wait_color.Enabled = false;
                pibo_wait_window.Enabled = false;
                pibo_wait_key.Enabled = false;

                mi.Invoke(o, null);

                scintilla.Enabled = true;
                tabControl1.Enabled = true;
                pibo_compile.Enabled = true;
                pibo_save.Enabled = true;
                pibo_open.Enabled = true;
                pibo_select_coordinate.Enabled = true;
                pibo_select_color.Enabled = true;
                pibo_select_window.Enabled = true;
                pibo_wait_time.Enabled = true;
                pibo_wait_color.Enabled = true;
                pibo_wait_window.Enabled = true;
                pibo_wait_key.Enabled = true;

                pibo_compile.Image = Properties.Resources.StepForwardHot;
                toolTip1.SetToolTip(pibo_compile, "Run");
                if (compiled == 2)
                    compiled = 1;

                #endregion
            }
        }

        private void pibo_save_Click(object sender, EventArgs e)
        {
            if ((saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) && !recording && !select_color && !select_coordinate && !select_window && !select_key)
            {
                using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        ScintillaSaveChangesToStrings();

                        sw.Write(Encryption.EncryptString(code, cryptPassword)
                            + "|######|" + Encryption.EncryptString(references, cryptPassword)
                            + "|######|" + Encryption.EncryptString(hookmanager, cryptPassword)
                            + "|######|" + Encryption.EncryptString(assemblynames, cryptPassword));
                    }
                    MessageBox.Show("Save successful");
                }
            }
        }

        private void pibo_open_Click(object sender, EventArgs e)
        {
            if ((openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) && !recording && !select_color && !select_coordinate && !select_window && !select_key)
            {
                try
                {
                    FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);

                    string tmp = sr.ReadToEnd();

                    code = Encryption.DecryptString(tmp.Remove(tmp.IndexOf("|######|")), cryptPassword);
                    tmp = tmp.Substring(tmp.IndexOf("|######|") + 8);
                    references = Encryption.DecryptString(tmp.Remove(tmp.IndexOf("|######|")), cryptPassword);
                    tmp = tmp.Substring(tmp.IndexOf("|######|") + 8);
                    hookmanager = Encryption.DecryptString(tmp.Remove(tmp.IndexOf("|######|")), cryptPassword);
                    assemblynames = Encryption.DecryptString(tmp.Substring(tmp.IndexOf("|######|") + 8), cryptPassword);

                    switch (tabControl1.SelectedIndex)
                    {
                        case 0:
                            scintilla.Text = code;
                            break;
                        case 1:
                            scintilla.Text = references;
                            break;
                        case 2:
                            scintilla.Text = hookmanager;
                            break;
                        case 3:
                            scintilla.Text = assemblynames;
                            break;
                    }

                    sr.Close();
                    sr.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
                catch
                {
                    MessageBox.Show("Invalide File!");
                }
            }
        }

        private void pibo_record_Click(object sender, EventArgs e)
        {
            if (!recording && !select_color && !select_coordinate && !select_window)
            {
                recording = true;

                pibo_compile.Visible = false;
                pibo_open.Visible = false;
                pibo_save.Visible = false;
                pibo_select_coordinate.Visible = false;
                pibo_select_color.Visible = false;
                pibo_select_window.Visible = false;
                pibo_wait_color.Visible = false;
                pibo_wait_window.Visible = false;
                pibo_wait_key.Visible = false;

                pibo_record.Image = Properties.Resources.stop_2;
                toolTip1.SetToolTip(pibo_record, "Stop recording");

                HookManager.Hook(this);
            }
            else if (recording)
            {
                recording = false;

                pibo_compile.Visible = true;
                pibo_open.Visible = true;
                pibo_save.Visible = true;
                pibo_select_coordinate.Visible = true;
                pibo_select_color.Visible = true;
                pibo_select_window.Visible = true;
                pibo_wait_color.Visible = true;
                pibo_wait_window.Visible = true;
                pibo_wait_key.Visible = true;

                pibo_record.Image = Properties.Resources.Record_Button;
                toolTip1.SetToolTip(pibo_record, "Start recording");

                HookManager.UnHook();

                if (!string.IsNullOrEmpty(input))
                {
                    ScintillaWriteKeyboardLine(input);
                    input = "";
                }
            }
        }

        private void pibo_select_coordinate_Click(object sender, EventArgs e)
        {
            if (!select_coordinate && !select_color && !recording && !select_window)
            {
                select_coordinate = true;

                pibo_compile.Visible = false;
                pibo_open.Visible = false;
                pibo_save.Visible = false;
                pibo_record.Visible = false;
                pibo_select_color.Visible = false;
                pibo_select_window.Visible = false;
                pibo_wait_time.Visible = false;
                pibo_wait_color.Visible = false;
                pibo_wait_window.Visible = false;
                pibo_wait_key.Visible = false;

                HookManager.Hook(this);

                pibo_select_coordinate.Image = Properties.Resources.Stop;
                toolTip1.SetToolTip(pibo_select_coordinate, "Stop coordinate selection");

                IntPtr hcur = LoadCursor(IntPtr.Zero, (int)IDC_STANDARD_CURSORS.IDC_CROSS);
                Cursor c = new Cursor(hcur);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_APPSTARTING);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_CROSS);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_HAND);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_HELP);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_IBEAM);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_NO);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_NORMAL);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZEALL);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZENESW);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZENS);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZENWSE);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZEWE);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_UP);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_WAIT);
            }
            else if (select_coordinate)
            {
                select_coordinate = false;

                pibo_compile.Visible = true;
                pibo_open.Visible = true;
                pibo_save.Visible = true;
                pibo_record.Visible = true;
                pibo_select_color.Visible = true;
                pibo_select_window.Visible = true;
                pibo_wait_time.Visible = true;
                pibo_wait_color.Visible = true;
                pibo_wait_window.Visible = true;
                pibo_wait_key.Visible = true;

                HookManager.UnHook();

                pibo_select_coordinate.Image = Properties.Resources.crosshairs;
                toolTip1.SetToolTip(pibo_select_coordinate, "Select coordinate");

                int NULL = 0;
                SystemParametersInfo((uint)SystemParametersInfoAction.SPI_SETCURSORS, 0, ref NULL, 0);
            }
        }
        
        private void pibo_select_color_Click(object sender, EventArgs e)
        {
            if (!select_color && !select_coordinate && !recording && !select_window)
            {
                select_color = true;

                pibo_compile.Visible = false;
                pibo_open.Visible = false;
                pibo_save.Visible = false;
                pibo_record.Visible = false;
                pibo_select_coordinate.Visible = false;
                pibo_select_window.Visible = false;
                pibo_wait_time.Visible = false;
                pibo_wait_color.Visible = false;
                pibo_wait_window.Visible = false;
                pibo_wait_key.Visible = false;

                HookManager.Hook(this);

                pibo_select_color.Image = Properties.Resources.color;
                toolTip1.SetToolTip(pibo_select_color, "Stop color selection");

                MemoryStream memoryStream = new MemoryStream(Properties.Resources.pipette);

                string tempFileName = "tmp_cursor.cur";
                Stream tempfile = File.OpenWrite(tempFileName);

                byte[] buffer = new byte[8 * 1024];
                int len;

                while ((len = memoryStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    tempfile.Write(buffer, 0, len);
                }
                memoryStream.Close();
                memoryStream.Dispose();
                tempfile.Close();
                tempfile.Dispose();

                IntPtr hcur = LoadCursorFromFile(tempFileName);

                File.Delete(tempFileName);

                Cursor c = new Cursor(hcur);
                
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_APPSTARTING);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_CROSS);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_HAND);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_HELP);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_IBEAM);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_NO);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_NORMAL);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZEALL);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZENESW);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZENS);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZENWSE);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZEWE);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_UP);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_WAIT);
            }
            else if (select_color)
            {
                select_color = false;
                waitSelect = false;

                pibo_compile.Visible = true;
                pibo_open.Visible = true;
                pibo_save.Visible = true;
                pibo_record.Visible = true;
                pibo_select_coordinate.Visible = true;
                pibo_select_window.Visible = true;
                pibo_wait_time.Visible = true;
                pibo_wait_color.Visible = true;
                pibo_wait_window.Visible = true;
                pibo_wait_key.Visible = true;

                HookManager.UnHook();

                pibo_select_color.Image = Properties.Resources.pipette_2;
                toolTip1.SetToolTip(pibo_select_color, "Select color");

                int NULL = 0;
                SystemParametersInfo((uint)SystemParametersInfoAction.SPI_SETCURSORS, 0, ref NULL, 0);
            }
        }

        private void pibo_select_window_Click(object sender, EventArgs e)
        {
            if (!select_coordinate && !select_color && !recording && !select_window)
            {
                select_window = true;

                pibo_compile.Visible = false;
                pibo_open.Visible = false;
                pibo_save.Visible = false;
                pibo_record.Visible = false;
                pibo_select_coordinate.Visible = false;
                pibo_select_color.Visible = false;
                pibo_wait_time.Visible = false;
                pibo_wait_color.Visible = false;
                pibo_wait_window.Visible = false;
                pibo_wait_key.Visible = false;

                HookManager.Hook(this);

                pibo_select_window.Image = Properties.Resources.Stop;
                toolTip1.SetToolTip(pibo_select_window, "Stop window selection");

                IntPtr hcur = LoadCursor(IntPtr.Zero, (int)IDC_STANDARD_CURSORS.IDC_HAND);
                Cursor c = new Cursor(hcur);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_APPSTARTING);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_CROSS);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_HAND);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_HELP);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_IBEAM);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_NO);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_NORMAL);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZEALL);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZENESW);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZENS);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZENWSE);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_SIZEWE);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_UP);
                SetSystemCursor(c.CopyHandle(), (uint)OCR_SYSTEM_CURSORS.OCR_WAIT);
            }
            else if (select_window)
            {
                select_window = false;
                waitSelect = false;

                pibo_compile.Visible = true;
                pibo_open.Visible = true;
                pibo_save.Visible = true;
                pibo_record.Visible = true;
                pibo_select_coordinate.Visible = true;
                pibo_select_color.Visible = true;
                pibo_wait_time.Visible = true;
                pibo_wait_color.Visible = true;
                pibo_wait_window.Visible = true;
                pibo_wait_key.Visible = true;

                HookManager.UnHook();

                pibo_select_window.Image = Properties.Resources.Coherence;
                toolTip1.SetToolTip(pibo_select_window, "Select window");

                int NULL = 0;
                SystemParametersInfo((uint)SystemParametersInfoAction.SPI_SETCURSORS, 0, ref NULL, 0);
            }
        }

        private void pibo_wait_time_Click(object sender, EventArgs e)
        {
            if (!select_color && !select_coordinate && !select_window)
            {
                ScintillaWriteLine("Wait(1000);");
            }
        }

        private void pibo_wait_color_Click(object sender, EventArgs e)
        {
            if (!select_color && !select_coordinate && !select_window && !recording)
            {
                ScintillaWrite("WaitForColor(");
                waitSelect = true;
                pibo_select_color_Click(null, null);
            }
        }

        private void pibo_wait_window_Click(object sender, EventArgs e)
        {
            if (!select_color && !select_coordinate && !select_window && !recording)
            {
                ScintillaWrite("WaitForActiveWindow(\"");
                waitSelect = true;
                pibo_select_window_Click(null, null);
            }
        }

        private void pibo_wait_key_Click(object sender, EventArgs e)
        {
            if (!select_color && !select_coordinate && !select_window && !recording)
            {
                select_key = true;
                
                pibo_compile.Visible = false;
                pibo_open.Visible = false;
                pibo_save.Visible = false;
                pibo_record.Visible = false;
                pibo_select_coordinate.Visible = false;
                pibo_select_color.Visible = false;
                pibo_select_window.Visible = false;
                pibo_wait_time.Visible = false;
                pibo_wait_color.Visible = false;
                pibo_wait_window.Visible = false;
                pibo_wait_key.Visible = false;

                ScintillaWrite("WaitForKey(\"");
                HookManager.Hook(this);
            }
        }

        #endregion
    }
    
    public static class MyExtensions
    {
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
