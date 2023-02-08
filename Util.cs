using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiGoC
{
    class Util
    {
        public enum PrintType { INFO, WARNING, ERROR, Group, LP }
        static public void Print(string t, PrintType type = PrintType.INFO, ConsoleColor c = ConsoleColor.Gray)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[");

            if (type == PrintType.INFO)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("信息");
            }
            else if (type == PrintType.WARNING)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("警告");
            }
            else if (type == PrintType.ERROR)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("错误");
            }
            else if (type == PrintType.Group)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("群聊");
            }
            else if (type == PrintType.LP)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("插件载入");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] ");

            Console.ForegroundColor = c;
            Console.Write(t + "\n");

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
