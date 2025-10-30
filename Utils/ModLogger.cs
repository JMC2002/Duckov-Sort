using DuckSort.Core;
using System;

// using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DuckSort.Utils
{
    public static class ModLogger
    {
        // === Debug 模式开关 ===
        public static bool EnableDebug => ModConfig.EnableDebugLogs;

        public static void Debug(string message,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            if (!EnableDebug)
                return;

            UnityEngine.Debug.Log(Format("DEBUG", message, caller, file, line));
        }

        // 输出普通信息日志。
        public static void Info(string message,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            UnityEngine.Debug.Log(Format("INFO", message, caller, file, line));
        }

        // 输出警告日志。
        public static void Warn(string message,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            UnityEngine.Debug.LogWarning(Format("WARN", message, caller, file, line));
        }

        // 输出错误日志。
        public static void Error(string message, Exception? ex = null,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            UnityEngine.Debug.LogError(Format("ERROR", message + (ex != null ? $"\n{ex}" : ""), caller, file, line));
        }

        // 格式化输出文本，包含时间、版本信息。
        private static string Format(string level, string message,
            string caller, string file, int line)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            return $"{VersionInfo.Tag} [{timestamp}] [{level}] {caller} (L{line}): {message}";
        }
    }


    // 用于测量代码块执行时间的辅助类。
    // 使用方法：
    //    void SortInventory(Inventory inventory, Comparison<Item> comparison)
    //    {
    //        using (new ScopedTimer("SortInventory"))
    //        {
    //            var items = inventory.GetItems();
    //            items.Sort(comparison);
    //            // ...
    //        }
    //    }
    public sealed class ScopedTimer : System.IDisposable
    {
        private readonly string _label;
        private readonly System.Diagnostics.Stopwatch _watch;

        public ScopedTimer(string label)
        {
            _label = label;
            _watch = System.Diagnostics.Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _watch.Stop();
            ModLogger.Debug($"[Timer] {_label} 耗时: {_watch.Elapsed.TotalMilliseconds:F2} ms");
        }
    }
}
