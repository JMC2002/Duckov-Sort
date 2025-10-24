using DuckSort.Core;
using System;
using UnityEngine;

namespace DuckSort.Utils
{
    public static class ModLogger
    {
        // 输出普通信息日志。
        public static void Info(string message)
        {
            Debug.Log(Format("INFO", message));
        }

        // 输出警告日志。
        public static void Warn(string message)
        {
            Debug.LogWarning(Format("WARN", message));
        }

        // 输出错误日志。
        public static void Error(string message, Exception? ex = null)
        {
            Debug.LogError(Format("ERROR", message + (ex != null ? $"\n{ex}" : "")));
        }

        // 格式化输出文本，包含时间、版本信息。
        private static string Format(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            return $"{VersionInfo.Tag} [{timestamp}] [{level}] {message}";
        }
    }
}
