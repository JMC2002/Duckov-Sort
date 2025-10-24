using DuckSort.Core;
using System;
// using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DuckSort.Utils
{
    public static class ModLogger
    {
        // 输出普通信息日志。
        public static void Info(string message,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            Debug.Log(Format("INFO", message, caller, file, line));
        }

        // 输出警告日志。
        public static void Warn(string message,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            Debug.LogWarning(Format("WARN", message, caller, file, line));
        }

        // 输出错误日志。
        public static void Error(string message, Exception? ex = null,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            Debug.LogError(Format("ERROR", message + (ex != null ? $"\n{ex}" : ""), caller, file, line));
        }

        // 格式化输出文本，包含时间、版本信息。
        private static string Format(string level, string message,
            string caller, string file, int line)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            return $"{VersionInfo.Tag} [{timestamp}] [{level}] {caller} (L{line}): {message}";
        }
    }
}
