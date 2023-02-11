using EleCho.ConsoleAnsi;

namespace MiraiGoC
{
    /// <summary>
    /// 基于虚拟终端 ANSI 转义序列的简单日志类
    /// (Console.Background 和 Console.Foreground 在 Linux 上是不支持的, 为了兼容性, 使用 ANSI 转义序列)
    /// </summary>
    class SimpleLogger
    {
        readonly List<TextWriter> extraTargets = new List<TextWriter>();

        public SimpleLogger() { }

        public SimpleLogger WithTarget(TextWriter stream)
        {
            extraTargets.Add(stream);
            return this;
        }

        public SimpleLogger RemoveTarget(TextWriter stream)
        {
            extraTargets.Remove(stream);
            return this;
        }

        public SimpleLogger LogInfo(string info)
        {
            foreach (TextWriter target in extraTargets)
                target.WriteLine($"[{DateTime.Now}][INFO] {info}");

            Console.WriteLine($"[{DateTime.Now}][INFO] {info}");             // 不附加格式
            return this;
        }

        public SimpleLogger LogWarning(string warning)
        {
            foreach (TextWriter target in extraTargets)
                target.WriteLine($"[{DateTime.Now}][WARNING] {warning}");

            Console.WriteLine(new ConsoleAnsiSeq()
                .SetForeColor(ConsoleColor.Yellow)                           // 颜色
                .AppendText($"[{DateTime.Now}][WARNING] {warning}")             // 内容
                .SetTextFormat(SgrCode.Default)                              // 重置格式
                .Build());

            return this;
        }

        public SimpleLogger LogError(string error)
        {
            foreach (TextWriter target in extraTargets)
                target.WriteLine($"[{DateTime.Now}][ERROR] {error}");

            Console.WriteLine(new ConsoleAnsiSeq()
                .SetForeColor(ConsoleColor.Red   )                           // 颜色
                .AppendText($"[{DateTime.Now}][ERROR] {error}")              // 内容
                .SetTextFormat(SgrCode.Default)                              // 重置格式
                .Build());

            return this;
        }

        public SimpleLogger LogDebug(string msg)
        {
            foreach (TextWriter target in extraTargets)
                target.WriteLine($"[{DateTime.Now}][DEBUG] {msg}");

            Console.WriteLine(new ConsoleAnsiSeq()
                .SetForeColor(ConsoleColor.Green)                            // 颜色
                .AppendText($"[{DateTime.Now}][DEBUG] {msg}")                // 内容
                .SetTextFormat(SgrCode.Default)                              // 重置格式
                .Build());

            return this;
        }
    }
}
