using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MiraiGoC
{
    class Util
    {
        /// <summary>
        /// 启用 ANSI 转义序列
        /// </summary>
        public static void EnableAnsiSequence()
        {
            // 如果是 Windows 平台, 则使用 WINAPI 启用虚拟终端
            if (Environment.OSVersion.Platform== PlatformID.Win32NT)
                WindowsUtils.EnableVirtualTerminalProcessingOnWindows();
        }


        /// <summary>
        /// Windows 平台用的东西
        /// </summary>
        internal class WindowsUtils
        {
            private const int STD_OUTPUT_HANDLE = -11;
            private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
            private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

            [DllImport("kernel32.dll")]
            private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

            [DllImport("kernel32.dll")]
            private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll")]
            private static extern uint GetLastError();

            /// <summary>
            /// 启用虚拟终端
            /// </summary>
            /// <returns></returns>
            public static uint EnableVirtualTerminalProcessingOnWindows()
            {
                var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
                if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
                {
                    return GetLastError();
                }

                outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
                if (!SetConsoleMode(iStdOut, outConsoleMode))
                {
                    return GetLastError();
                }

                return 0;
            }
        }
    }
}
