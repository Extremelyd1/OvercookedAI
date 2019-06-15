using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace AI {

    class Keyboard {

        private static Keyboard INSTANCE;

        private ArrayList keysDown = new ArrayList();

        private Keyboard() {

        }

        public static Keyboard Get() {
            if (INSTANCE == null) {
                INSTANCE = new Keyboard();
            }

            return INSTANCE;
        }

        private void Send(Input key, KEYEVENTF flag) {
            INPUT[] inputs = new INPUT[1];
            INPUT input = new INPUT();
            input.type = 1; // 1 = Keyboard Input
            input.ki.wScan = 0;
            input.ki.time = 0;
            input.ki.dwExtraInfo =  IntPtr.Zero;
            input.ki.wVk = key;
            input.ki.dwFlags = flag;
            inputs[0] = input;
            SendInput(1, inputs, INPUT.Size);
        }

        public void SendDown(Input key) {
            Send(key, 0);

            keysDown.Add(key);
        }

        public void SendUp(Input key) {
            Send(key, KEYEVENTF.KEYUP);

            keysDown.Remove(key);
        }

        public bool IsKeyDown(Input key) {
            return keysDown.Contains(key);
        }

        public void StopXMovement() {
            if (keysDown.Contains(Input.MOVE_RIGHT)) {
                SendUp(Input.MOVE_RIGHT);
            }
            if (keysDown.Contains(Input.MOVE_LEFT)) {
                SendUp(Input.MOVE_LEFT);
            }
        }

        public void StopZMovement() {
            if (keysDown.Contains(Input.MOVE_DOWN)) {
                SendUp(Input.MOVE_DOWN);
            }
            if (keysDown.Contains(Input.MOVE_UP)) {
                SendUp(Input.MOVE_UP);
            }
        }

        /// <summary>
        /// Declaration of external SendInput method
        /// </summary>
        [DllImport("user32.dll")]
        private static extern UInt32 SendInput(
            UInt32 nInputs,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] pInputs,
            UInt32 cbSize);


        // Declare the INPUT struct
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT {
            [FieldOffset(0)]
            public uint type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
            public static UInt32 Size {
                get { return (UInt32) Marshal.SizeOf(typeof(INPUT)); }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT {
            internal int dx;
            internal int dy;
            internal MouseEventDataXButtons mouseData;
            internal MOUSEEVENTF dwFlags;
            internal uint time;
            internal UIntPtr dwExtraInfo;
        }

        [Flags]
        public enum MouseEventDataXButtons : uint {
            Nothing = 0x00000000,
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }

        [Flags]
        public enum MOUSEEVENTF : uint {
            ABSOLUTE = 0x8000,
            HWHEEL = 0x01000,
            MOVE = 0x0001,
            MOVE_NOCOALESCE = 0x2000,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            VIRTUALDESK = 0x4000,
            WHEEL = 0x0800,
            XDOWN = 0x0080,
            XUP = 0x0100
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT {
            internal Input wVk;
            internal short wScan;
            internal KEYEVENTF dwFlags;
            internal int time;
            internal IntPtr dwExtraInfo;
        }

        [Flags]
        public enum KEYEVENTF : uint {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            SCANCODE = 0x0008,
            UNICODE = 0x0004
        }

        public enum Input : short {
            ///<summary>
            ///CTRL key
            ///</summary>
            CHOP_THROW = 0x11,
            ///<summary>
            ///ALT key
            ///</summary>
            DASH = 0x12,
            ///<summary>
            ///SPACEBAR
            ///</summary>
            PICK_DROP = 0x20,
            ///<summary>
            ///LEFT ARROW key
            ///</summary>
            LEFT = 0x25,
            ///<summary>
            ///UP ARROW key
            ///</summary>
            UP = 0x26,
            ///<summary>
            ///RIGHT ARROW key
            ///</summary>
            RIGHT = 0x27,
            ///<summary>
            ///DOWN ARROW key
            ///</summary>
            DOWN = 0x28,
            ///<summary>
            ///A key
            ///</summary>
            MOVE_LEFT = 0x41,
            ///<summary>
            ///D key
            ///</summary>
            MOVE_RIGHT = 0x44,
            ///<summary>
            ///E key
            ///</summary>
            EMOTE = 0x45,
            ///<summary>
            ///S key
            ///</summary>
            MOVE_DOWN = 0x53,
            ///<summary>
            ///W key
            ///</summary>
            MOVE_UP = 0x57,
            ///<summary>
            ///Left CONTROL key
            ///</summary>
            CHOP_THROW2 = 0xA2,
            ///<summary>
            ///Right CONTROL key
            ///</summary>
            CHOP_THROW3 = 0xA3
        }

        /// <summary>
        /// Define HARDWAREINPUT struct
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT {
            internal int uMsg;
            internal short wParamL;
            internal short wParamH;
        }

    }
}
