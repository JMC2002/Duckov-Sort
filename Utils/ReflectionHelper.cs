using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace DuckSort.Utils
{
    public static class ReflectionHelper
    {
        // 缓存区（线程安全）
        private static readonly ConcurrentDictionary<(Type, string), Delegate?> _fieldGetterCache = new();
        private static readonly ConcurrentDictionary<(Type, string), Delegate?> _fieldSetterCache = new();
        private static readonly ConcurrentDictionary<(Type, string), Delegate?> _methodCache = new();

        // ================== 字段操作 ==================

        public static T GetFieldValue<T>(object obj, string fieldName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), $"GetFieldValue<{typeof(T).Name}> 失败：obj 为 null");

            var type = obj.GetType();
            var getter = (Func<object, object?>?)_fieldGetterCache.GetOrAdd((type, fieldName), key =>
            {
                var field = key.Item1.GetField(key.Item2, BindingFlags.Instance | BindingFlags.NonPublic);
                if (field == null)
                    return null;

                // 创建 DynamicMethod
                var dm = new DynamicMethod(
                    $"get_{key.Item1.Name}_{key.Item2}",
                    typeof(object),
                    new[] { typeof(object) },
                    key.Item1,
                    true
                );

                var il = dm.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0); // 加载目标对象
                il.Emit(OpCodes.Castclass, key.Item1); // 转型
                il.Emit(OpCodes.Ldfld, field); // 取字段
                if (field.FieldType.IsValueType)
                    il.Emit(OpCodes.Box, field.FieldType);
                il.Emit(OpCodes.Ret);

                return dm.CreateDelegate(typeof(Func<object, object?>));
            });

            if (getter == null)
                throw new MissingFieldException($"在 {type.Name} 中找不到字段 {fieldName}");

            var value = getter(obj);
            if (value is T t)
                return t;

            throw new InvalidCastException($"字段 {fieldName} 的类型与预期的 {typeof(T).Name} 不匹配");
        }

        public static void SetFieldValue<T>(object obj, string fieldName, T value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), $"SetFieldValue 失败：obj 为 null");

            var type = obj.GetType();
            var setter = (Action<object, object?>?)_fieldSetterCache.GetOrAdd((type, fieldName), key =>
            {
                var field = key.Item1.GetField(key.Item2, BindingFlags.Instance | BindingFlags.NonPublic);
                if (field == null)
                    return null;

                var dm = new DynamicMethod(
                    $"set_{key.Item1.Name}_{key.Item2}",
                    typeof(void),
                    new[] { typeof(object), typeof(object) },
                    key.Item1,
                    true
                );

                var il = dm.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, key.Item1);
                il.Emit(OpCodes.Ldarg_1);
                if (field.FieldType.IsValueType)
                    il.Emit(OpCodes.Unbox_Any, field.FieldType);
                else
                    il.Emit(OpCodes.Castclass, field.FieldType);
                il.Emit(OpCodes.Stfld, field);
                il.Emit(OpCodes.Ret);

                return dm.CreateDelegate(typeof(Action<object, object?>));
            });

            if (setter == null)
                throw new MissingFieldException($"在 {type.Name} 中找不到字段 {fieldName}");

            setter(obj, value);
        }

        // ================== 方法操作 ==================

        public static T CallMethod<T>(object obj, string methodName, params object[] args)
        {
            var type = obj?.GetType() ?? throw new ArgumentNullException(nameof(obj));
            var invoker = (Func<object, object?[], object?>?)_methodCache.GetOrAdd((type, methodName), key =>
            {
                var method = key.Item1.GetMethod(key.Item2, BindingFlags.Instance | BindingFlags.NonPublic);
                return method != null ? CreateInstanceMethodInvoker(method) : null;
            });

            if (invoker == null)
                throw new MissingMethodException($"在 {type.Name} 中找不到方法 {methodName}");

            var result = invoker(obj, args);
            if (result is T t)
                return t;

            if (result == null && default(T) == null)
                return default!;

            throw new InvalidCastException($"方法 {methodName} 的返回值类型与预期的 {typeof(T).Name} 不匹配");
        }

        public static void CallVoidMethod(object obj, string methodName, params object[] args)
        {
            var type = obj?.GetType() ?? throw new ArgumentNullException(nameof(obj));
            var invoker = (Action<object, object?[]>)_methodCache.GetOrAdd((type, methodName + "_void"), key =>
            {
                var method = key.Item1.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                return method != null ? CreateInstanceMethodAction(method) : null;
            })!;

            if (invoker == null)
                throw new MissingMethodException($"在 {type.Name} 中找不到方法 {methodName}");

            invoker(obj, args);
        }

        public static T CallStaticMethod<T>(Type type, string methodName, params object[] args)
        {
            var invoker = (Func<object?[], object?>?)_methodCache.GetOrAdd((type, methodName + "_static"), key =>
            {
                var method = key.Item1.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
                return method != null ? CreateStaticMethodInvoker(method) : null;
            });

            if (invoker == null)
                throw new MissingMethodException($"在 {type.Name} 中找不到静态方法 {methodName}");

            var result = invoker(args);
            if (result is T t)
                return t;

            if (result == null && default(T) == null)
                return default!;

            throw new InvalidCastException($"方法 {methodName} 的返回值类型与预期的 {typeof(T).Name} 不匹配");
        }

        public static bool CallStaticMethodWithOut<TOut>(
            Type type, string methodName,
            object?[] args,
            out TOut? outValue)
        {
            outValue = default;

            var invoker = (Func<object?[], object?>?)_methodCache.GetOrAdd((type, methodName + "_staticout"), key =>
            {
                var method = key.Item1.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
                return method != null ? CreateStaticMethodInvoker(method) : null;
            });

            if (invoker == null)
                throw new MissingMethodException($"在 {type.Name} 中找不到静态方法 {methodName}");

            var result = invoker(args);
            if (args.Length > 1 && args[1] is TOut t)
                outValue = t;

            if (result is bool b)
                return b;

            throw new InvalidCastException($"方法 {methodName} 的返回值类型与预期的 bool 不匹配");
        }

        // ================== 辅助 Emit 工具 ==================

        private static Func<object, object?[], object?> CreateInstanceMethodInvoker(MethodInfo method)
        {
            var dm = new DynamicMethod($"invoke_{method.Name}", typeof(object),
                new[] { typeof(object), typeof(object?[]) }, method.DeclaringType, true);

            var il = dm.GetILGenerator();
            var parameters = method.GetParameters();

            // 加载实例
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, method.DeclaringType!);

            // 加载参数
            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);

                var paramType = parameters[i].ParameterType;
                if (paramType.IsValueType)
                    il.Emit(OpCodes.Unbox_Any, paramType);
                else
                    il.Emit(OpCodes.Castclass, paramType);
            }

            il.EmitCall(OpCodes.Callvirt, method, null);

            if (method.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else if (method.ReturnType.IsValueType)
                il.Emit(OpCodes.Box, method.ReturnType);

            il.Emit(OpCodes.Ret);
            return (Func<object, object?[], object?>)dm.CreateDelegate(typeof(Func<object, object?[], object?>));
        }

        private static Action<object, object?[]> CreateInstanceMethodAction(MethodInfo method)
        {
            var dm = new DynamicMethod($"invoke_void_{method.Name}", typeof(void),
                new[] { typeof(object), typeof(object?[]) }, method.DeclaringType, true);
            var il = dm.GetILGenerator();
            var parameters = method.GetParameters();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, method.DeclaringType!);

            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);
                var paramType = parameters[i].ParameterType;
                if (paramType.IsValueType)
                    il.Emit(OpCodes.Unbox_Any, paramType);
                else
                    il.Emit(OpCodes.Castclass, paramType);
            }

            il.EmitCall(OpCodes.Callvirt, method, null);
            il.Emit(OpCodes.Ret);
            return (Action<object, object?[]>)dm.CreateDelegate(typeof(Action<object, object?[]>));
        }

        private static Func<object?[], object?> CreateStaticMethodInvoker(MethodInfo method)
        {
            var dm = new DynamicMethod($"invoke_static_{method.Name}", typeof(object),
                new[] { typeof(object?[]) }, method.DeclaringType, true);
            var il = dm.GetILGenerator();
            var parameters = method.GetParameters();

            // 为每个 ref/out 参数分配局部变量
            var locals = new LocalBuilder[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var paramType = parameters[i].ParameterType;
                var isByRef = paramType.IsByRef;
                var elementType = isByRef ? paramType.GetElementType()! : paramType;

                if (isByRef)
                {
                    locals[i] = il.DeclareLocal(elementType);
                    // args[i] → unbox/cast → stloc
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    if (elementType.IsValueType)
                        il.Emit(OpCodes.Unbox_Any, elementType);
                    else
                        il.Emit(OpCodes.Castclass, elementType);
                    il.Emit(OpCodes.Stloc, locals[i]);
                }
            }

            // 加载参数
            for (int i = 0; i < parameters.Length; i++)
            {
                var paramType = parameters[i].ParameterType;
                var isByRef = paramType.IsByRef;
                var elementType = isByRef ? paramType.GetElementType()! : paramType;

                if (isByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]); // 传递变量地址
                else
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    if (elementType.IsValueType)
                        il.Emit(OpCodes.Unbox_Any, elementType);
                    else
                        il.Emit(OpCodes.Castclass, elementType);
                }
            }

            // 调用静态方法
            il.EmitCall(OpCodes.Call, method, null);

            // 处理返回值
            if (method.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else if (method.ReturnType.IsValueType)
                il.Emit(OpCodes.Box, method.ReturnType);

            // 将 ref/out 值写回 args 数组
            for (int i = 0; i < parameters.Length; i++)
            {
                if (!parameters[i].ParameterType.IsByRef)
                    continue;

                var elementType = parameters[i].ParameterType.GetElementType()!;
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldloc, locals[i]);
                if (elementType.IsValueType)
                    il.Emit(OpCodes.Box, elementType);
                il.Emit(OpCodes.Stelem_Ref);
            }

            il.Emit(OpCodes.Ret);

            return (Func<object?[], object?>)dm.CreateDelegate(typeof(Func<object?[], object?>));
        }

    }
}
