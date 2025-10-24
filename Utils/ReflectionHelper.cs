using System;
using System.Collections.Concurrent;
using System.Reflection;
using Unity.VisualScripting;

namespace DuckSort.Utils
{
    public static class ReflectionHelper
    {
        // 🔒 缓存区（线程安全）
        private static readonly ConcurrentDictionary<(Type, string), FieldInfo?> _fieldCache = new();
        private static readonly ConcurrentDictionary<(Type, string), MethodInfo?> _methodCache = new();

        // ================== 字段操作 ==================

        /// <summary>
        /// 获取私有字段值（自动缓存 FieldInfo）
        /// </summary>
        public static T? GetFieldValue<T>(object obj, string fieldName)
        {
            if (obj == null)
            {
                ModLogger.Warn($"GetFieldValue<{typeof(T).Name}> 失败：obj 为 null");
                return default;
            }

            var type = obj.GetType();
            var field = _fieldCache.GetOrAdd((type, fieldName),
                key => key.Item1.GetField(key.Item2, BindingFlags.Instance | BindingFlags.NonPublic));

            if (field == null)
            {
                ModLogger.Warn($"在 {type.Name} 中找不到字段 {fieldName}");
                return default;
            }

            var value = field.GetValue(obj);
            return value is T t ? t : default;
        }

        /// <summary>
        /// 设置私有字段值（自动缓存 FieldInfo）
        /// </summary>
        public static void SetFieldValue<T>(object obj, string fieldName, T value)
        {
            if (obj == null)
            {
                ModLogger.Warn($"SetFieldValue 失败：obj 为 null");
                return;
            }

            var type = obj.GetType();
            var field = _fieldCache.GetOrAdd((type, fieldName),
                key => key.Item1.GetField(key.Item2, BindingFlags.Instance | BindingFlags.NonPublic));

            if (field == null)
            {
                ModLogger.Warn($"在 {type.Name} 中找不到字段 {fieldName}");
                return;
            }

            field.SetValue(obj, value);
        }

        // ================== 方法操作 ==================

        /// <summary>
        /// 调用私有实例方法（可返回值，自动缓存 MethodInfo）
        /// </summary>
        public static T? CallMethod<T>(object obj, string methodName, params object[] args)
        {
            if (obj == null)
            {
                ModLogger.Warn($"CallMethod<{typeof(T).Name}> 失败：obj 为 null");
                return default;
            }

            var type = obj.GetType();
            var method = _methodCache.GetOrAdd((type, methodName),
                key => key.Item1.GetMethod(key.Item2, BindingFlags.Instance | BindingFlags.NonPublic));

            if (method == null)
            {
                ModLogger.Warn($"在 {type.Name} 中找不到方法 {methodName}");
                return default;
            }

            var result = method.Invoke(obj, args);
            return result is T t ? t : default;
        }

        /// <summary>
        /// 调用静态私有方法（可返回值，自动缓存 MethodInfo）
        /// </summary>
        public static T? CallStaticMethod<T>(Type type, string methodName, params object[] args)
        {
            if (type == null)
            {
                ModLogger.Warn($"CallStaticMethod<{typeof(T).Name}> 失败：type 为 null");
                return default;
            }

            var method = _methodCache.GetOrAdd((type, methodName),
                key => key.Item1.GetMethod(key.Item2, BindingFlags.Static | BindingFlags.NonPublic));

            if (method == null)
            {
                ModLogger.Warn($"在 {type.Name} 中找不到静态方法 {methodName}");
                return default;
            }

            var result = method.Invoke(null, args);
            return result is T t ? t : default;
        }

        public static bool CallStaticMethodWithOut<TOut>(
                                Type type, string methodName,
                                object?[] args,
                                out TOut? outValue)
        {
            outValue = default;

            if (type == null)
            {
                ModLogger.Warn($"CallStaticMethodWithOut<{typeof(TOut).Name}> 失败：type 为 null");
                return false;
            }

            var method = _methodCache.GetOrAdd((type, methodName),
                key => key.Item1.GetMethod(key.Item2, BindingFlags.Static | BindingFlags.NonPublic));

            if (method == null)
            {
                ModLogger.Warn($"在 {type.Name} 中找不到静态方法 {methodName}");
                return false;
            }

            var result = method.Invoke(null, args);

            // out 参数会被写回 args 数组中
            if (args.Length > 1 && args[1] is TOut t)
                outValue = t;

            return result is bool b && b;
        }

    }


}
