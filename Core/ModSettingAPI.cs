using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Duckov.Modding;
using DuckSort.Utils;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class ModSettingAPI
{
    private const string ADD_DROP_DOWN_LIST = "AddDropDownList";
    private const string ADD_SLIDER = "AddSlider";
    private const string ADD_TOGGLE = "AddToggle";
    private const string ADD_KEYBINDING = "AddKeybinding";
    private const string GET_VALUE = "GetValue";
    private const string SET_VALUE = "SetValue";
    private const string REMOVE_UI = "RemoveUI";
    private const string REMOVE_MOD = "RemoveMod";
    private const string ADD_INPUT = "AddInput";
    private const string HAS_CONFIG = "HasConfig";
    private const string GET_SAVED_VALUE = "GetSavedValue";
    private const string ADD_KEYBINDING_WITH_DEFAULT = "AddKeybindingWithDefault";
    private const string ADD_BUTTON = "AddButton";
    private const string ADD_GROUP = "AddGroup";
    private static float Version = 0.4f;
    private static readonly Version VERSION = new Version(0, 4, 0);
    public const string MOD_NAME = "ModSetting";
    private const string TYPE_NAME = "ModSetting.ModBehaviour";
    private static Type modBehaviour;
    private static ModInfo modInfo;

    public static bool IsInit { get; private set; }

    // 缓存委托避免重复反射
    private static Dictionary<string, Delegate> methodCache = new Dictionary<string, Delegate>();

    private static readonly string[] methodNames = new[] {
        ADD_DROP_DOWN_LIST,
        ADD_SLIDER,
        ADD_TOGGLE,
        ADD_KEYBINDING,
        GET_VALUE,
        SET_VALUE,
        REMOVE_UI,
        REMOVE_MOD,
        ADD_INPUT,
        HAS_CONFIG,
        GET_SAVED_VALUE,
        ADD_KEYBINDING_WITH_DEFAULT,
        ADD_BUTTON,
        ADD_GROUP
    };

    /// <summary>
    /// 初始化API
    /// </summary>
    /// <param name="modInfo">mod信息</param>
    /// <returns>是否成功初始化</returns>
    public static bool Init(ModInfo modInfo)
    {
        if (IsInit) return true;
        if (modInfo.name == MOD_NAME)
        {
            ModLogger.Error("初始化失败，不能使用ModSetting的info进行初始化");
            return false;
        }
        ModSettingAPI.modInfo = modInfo;
        modBehaviour = FindTypeInAssemblies(TYPE_NAME);
        if (modBehaviour == null) return false;
        VersionAvailable();
        foreach (string methodName in methodNames)
        {
            MethodInfo[] methodInfos = modBehaviour.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == methodName)
                .ToArray();
            if (methodInfos.Length == 0)
            {
                ModLogger.Error($"{methodName}方法找不到");
                return false;
            }
        }

        IsInit = true;
        return true;
    }

    /// <summary>
    /// 添加一个下拉列表控件
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="description">描述文本</param>
    /// <param name="options">选项列表</param>
    /// <param name="defaultValue">当前值</param>
    /// <param name="onValueChange">值改变时的回调函数</param>
    /// <returns></returns>
    public static bool AddDropdownList(string key, string description,
        List<string> options, string defaultValue, Action<string> onValueChange = null)
    {
        if (!Available(key)) return false;
        Type delegateType = typeof(Action<ModInfo, string, string, List<string>, string, Action<string>>);
        return InvokeMethod(ADD_DROP_DOWN_LIST,
            ADD_DROP_DOWN_LIST,
            new object[] { modInfo, key, description, options, defaultValue, onValueChange },
            delegateType);
    }

    /// <summary>
    /// 添加一个浮点数滑块控件
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="description">描述文本</param>
    /// <param name="defaultValue">当前值</param>
    /// <param name="sliderRange">滑块范围</param>
    /// <param name="onValueChange">值改变时的回调函数</param>
    /// <param name="decimalPlaces">小数位数</param>
    /// <param name="characterLimit">输入字符限制</param>
    /// <returns></returns>
    public static bool AddSlider(string key, string description,
        float defaultValue, Vector2 sliderRange, Action<float> onValueChange = null, int decimalPlaces = 1,
        int characterLimit = 5)
    {
        if (!Available(key)) return false;
        Type[] paramTypes = {
            typeof(ModInfo), typeof(string), typeof(string),
            typeof(float), typeof(Vector2), typeof(Action<float>), typeof(int), typeof(int)
        };
        Type delegateType = typeof(Action<ModInfo, string, string, float, Vector2, Action<float>, int, int>);
        return InvokeMethod(ADD_SLIDER + "Float",
            ADD_SLIDER,
            new object[]
                { modInfo, key, description, defaultValue, sliderRange, onValueChange, decimalPlaces, characterLimit },
            delegateType,
            paramTypes);
    }

    /// <summary>
    /// 添加一个整数滑块控件
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="description">描述文本</param>
    /// <param name="defaultValue">当前值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="onValueChange">值改变时的回调函数</param>
    /// <param name="characterLimit">输入字符限制</param>
    /// <returns></returns>
    public static bool AddSlider(string key, string description,
        int defaultValue, int minValue, int maxValue, Action<int> onValueChange = null, int characterLimit = 5)
    {
        if (!Available(key)) return false;
        Type[] paramTypes = {
            typeof(ModInfo), typeof(string), typeof(string),
            typeof(int), typeof(int), typeof(int), typeof(Action<int>), typeof(int)
        };
        Type delegateType = typeof(Action<ModInfo, string, string, int, int, int, Action<int>, int>);
        return InvokeMethod(ADD_SLIDER + "Int", ADD_SLIDER,
            new object[]
                { modInfo, key, description, defaultValue, minValue, maxValue, onValueChange, characterLimit },
            delegateType,
            paramTypes);
    }

    /// <summary>
    /// 添加一个开关控件
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="description">描述文本</param>
    /// <param name="enable">当前值</param>
    /// <param name="onValueChange">值改变时的回调函数</param>
    /// <returns></returns>
    public static bool AddToggle(string key, string description,
        bool enable, Action<bool> onValueChange = null)
    {
        if (!Available(key)) return false;
        Type delegateType = typeof(Action<ModInfo, string, string, bool, Action<bool>>);
        return InvokeMethod(ADD_TOGGLE,
            ADD_TOGGLE,
            new object[] { modInfo, key, description, enable, onValueChange },
            delegateType);
    }

    /// <summary>
    /// 添加一个按键绑定控件，默认值None
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="description">描述文本</param>
    /// <param name="keyCode">当前值</param>
    /// <param name="onValueChange">值改变时的回调函数</param>
    /// <returns></returns>
    public static bool AddKeybinding(string key, string description,
        KeyCode keyCode, Action<KeyCode> onValueChange = null)
    {
        if (!Available(key)) return false;
        return InvokeMethod(ADD_KEYBINDING,
            ADD_KEYBINDING,
            new object[] { modInfo, key, description, keyCode, onValueChange },
            typeof(Action<ModInfo, string, string, KeyCode, Action<KeyCode>>));
    }

    /// <summary>
    /// 添加一个带默认值的按键绑定控件
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="description">描述文本</param>
    /// <param name="keyCode">当前值</param>
    /// <param name="defaultKeyCode">默认值</param>
    /// <param name="onValueChange">值改变时的回调函数</param>
    /// <returns></returns>
    public static bool AddKeybinding(string key, string description,
        KeyCode keyCode, KeyCode defaultKeyCode, Action<KeyCode> onValueChange = null)
    {
        if (!Available(key)) return false;
        return InvokeMethod(ADD_KEYBINDING_WITH_DEFAULT,
            ADD_KEYBINDING_WITH_DEFAULT,
            new object[] { modInfo, key, description, keyCode, defaultKeyCode, onValueChange },
            typeof(Action<ModInfo, string, string, KeyCode, KeyCode, Action<KeyCode>>));
    }

    /// <summary>
    /// 添加一个输入框控件
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="description">描述文本</param>
    /// <param name="defaultValue">当前值</param>
    /// <param name="characterLimit">输入字符限制</param>
    /// <param name="onValueChange">值改变时的回调函数</param>
    /// <returns></returns>
    public static bool AddInput(string key, string description,
        string defaultValue, int characterLimit = 40, Action<string> onValueChange = null)
    {
        if (!Available(key)) return false;
        return InvokeMethod(ADD_INPUT,
            ADD_INPUT,
            new object[] { modInfo, key, description, defaultValue, characterLimit, onValueChange },
            typeof(Action<ModInfo, string, string, string, int, Action<string>>));
    }

    /// <summary>
    /// 添加一个按钮控件
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="description">描述文本</param>
    /// <param name="buttonText">按钮文本</param>
    /// <param name="onClickButton">点击时的回调函数</param>
    /// <returns></returns>
    public static bool AddButton(string key, string description,
        string buttonText = "按钮", Action onClickButton = null)
    {
        if (!Available(key)) return false;
        return InvokeMethod(ADD_BUTTON,
            ADD_BUTTON,
            new object[] { modInfo, key, description, buttonText, onClickButton },
            typeof(Action<ModInfo, string, string, string, Action>));
    }

    /// <summary>
    /// 添加一个分组控件，用于将多个控件组织在一起
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="description">描述文本</param>
    /// <param name="keys">包含的控件键列表(暂不支持嵌套Group)</param>
    /// <param name="scale">缩放比例(相对于mod标题)，最大0.9f</param>
    /// <param name="topInsert">是否插入到顶部</param>
    /// <param name="open">是否默认展开</param>
    /// <returns></returns>
    public static bool AddGroup(string key, string description, List<string> keys,
        float scale = 0.7f, bool topInsert = false, bool open = false)
    {
        if (!Available(key)) return false;
        return InvokeMethod(ADD_GROUP,
            ADD_GROUP,
            new object[] { modInfo, key, description, keys, scale, topInsert, open },
            typeof(Action<ModInfo, string, string, List<string>, float, bool, bool>));
    }

    /// <summary>
    /// 获取指定key的配置值
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="callback">回调函数返回结果</param>
    /// <typeparam name="T">值类型</typeparam>
    /// <returns></returns>
    public static bool GetValue<T>(string key, Action<T> callback = null)
    {
        if (!Available(key)) return false;
        MethodInfo methodInfo = GetStaticPublicMethodInfo(GET_VALUE);
        if (methodInfo == null) return false;
        MethodInfo genericMethod = methodInfo.MakeGenericMethod(typeof(T));
        genericMethod.Invoke(null, new object[] { modInfo, key, callback });
        return true;
    }

    /// <summary>
    /// 设置指定key的配置值，单方面通知控件更新
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="value">设置值</param>
    /// <param name="callback">回调函数返回是否成功</param>
    /// <typeparam name="T">值类型</typeparam>
    /// <returns></returns>
    public static bool SetValue<T>(string key, T value, Action<bool> callback = null)
    {
        if (!Available(key)) return false;
        MethodInfo methodInfo = GetStaticPublicMethodInfo(SET_VALUE);
        if (methodInfo == null) return false;
        MethodInfo genericMethod = methodInfo.MakeGenericMethod(typeof(T));
        genericMethod.Invoke(null, new object[] { modInfo, key, value, callback });
        return true;
    }

    /// <summary>
    /// 检查是否存在此mod的配置文件
    /// </summary>
    /// <returns></returns>
    public static bool HasConfig()
    {
        if (!Available()) return false;
        MethodInfo methodInfo = GetStaticPublicMethodInfo(HAS_CONFIG);
        if (methodInfo == null) return false;
        return (bool)methodInfo.Invoke(null, new object[] { modInfo });
    }

    /// <summary>
    /// 获取已保存的配置值
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="value">保存的值</param>
    /// <typeparam name="T">值类型</typeparam>
    /// <returns></returns>
    public static bool GetSavedValue<T>(string key, out T value)
    {
        value = default;
        if (!Available(key)) return false;
        MethodInfo methodInfo = GetStaticPublicMethodInfo(GET_SAVED_VALUE);
        if (methodInfo == null) return false;
        MethodInfo genericMethod = methodInfo.MakeGenericMethod(typeof(T));
        // 准备参数数组（注意：out 参数需要特殊处理）
        object[] parameters = new object[] { modInfo, key, null };
        bool result = (bool)genericMethod.Invoke(null, parameters);
        // 获取 out 参数的值
        value = (T)parameters[2];
        return result;
    }

    /// <summary>
    /// 移除指定键对应的UI控件
    /// </summary>
    /// <param name="key">控件key</param>
    /// <param name="callback">回调函数返回操作结果</param>
    /// <returns></returns>
    public static bool RemoveUI(string key, Action<bool> callback = null)
    {
        if (!Available(key)) return false;
        return InvokeMethod(REMOVE_UI,
            REMOVE_UI,
            new object[] { modInfo, key, callback },
            typeof(Action<ModInfo, string, Action<bool>>));
    }

    /// <summary>
    /// 移除整个模组的UI配置,当禁用此mod时，ModSetting会自动移除相对应的UI，一般来说不需要调用，除非想要主动移除mod所有UI
    /// </summary>
    /// <param name="callback">回调函数返回操作结果</param>
    /// <returns></returns>
    public static bool RemoveMod(Action<bool> callback = null)
    {
        if (!Available()) return false;
        Type delegateType = typeof(Action<ModInfo, Action<bool>>);
        return InvokeMethod(REMOVE_MOD, REMOVE_MOD, new object[] { modInfo, callback }, delegateType);
    }

    private static bool Available()
    {
        return IsInit && modInfo.displayName != null && modInfo.name != null;
    }

    private static bool Available(string key)
    {
        return IsInit && modInfo.displayName != null && modInfo.name != null && key != null;
    }

    private static bool VersionAvailable()
    {
        FieldInfo versionField = modBehaviour.GetField("VERSION", BindingFlags.Public | BindingFlags.Static);
        if (versionField != null && versionField.FieldType == typeof(float))
        {
            Version modSettingVersion = (Version)versionField.GetValue(null);
            if (modSettingVersion != VERSION)
            {
                Debug.LogWarning($"警告:ModSetting的版本:{modSettingVersion} (API的版本:{VERSION}),新功能将无法使用");
                return false;
            }
            return true;
        }

        return false;
    }

    private static Type FindTypeInAssemblies(string typeName)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            if (assembly.FullName.Contains(MOD_NAME))
            {
                Debug.Log($"找到{MOD_NAME}相关程序集: {assembly.FullName}");
            }

            Type type = assembly.GetType(typeName);
            if (type != null) return type;
        }

        Debug.Log("找不到程序集");
        return null;
    }

    private static MethodInfo GetStaticPublicMethodInfo(string methodName, Type[] parameterTypes = null)
    {
        if (!IsInit) return null;
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static;
        if (parameterTypes != null)
        {
            MethodInfo[] methodInfos = modBehaviour.GetMethods(bindingFlags).Where(m => m.Name == methodName).ToArray();
            return methodInfos.Where(methodInfo => {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length != parameterTypes.Length) return false;
                for (int i = 0; i < parameters.Length; i++)
                {
                    // 处理参数类型匹配（包括继承和接口实现）
                    if (!IsParameterTypeMatch(parameters[i].ParameterType, parameterTypes[i]))
                        return false;
                }

                return true;
            }).FirstOrDefault();
        }
        else
        {
            MethodInfo methodInfo = modBehaviour.GetMethod(methodName, bindingFlags);
            return methodInfo;
        }
    }

    private static bool IsParameterTypeMatch(Type parameterType, Type providedType)
    {
        // 精确匹配
        if (parameterType == providedType)
            return true;
        // 处理值类型和可空类型
        if (parameterType.IsValueType && Nullable.GetUnderlyingType(parameterType) == providedType)
            return true;
        // 处理继承关系
        if (parameterType.IsAssignableFrom(providedType))
            return true;
        return false;
    }

    private static bool InvokeMethod(string cacheKey, string methodName, object[] parameters, Type delegateType,
        Type[] paramTypes = null)
    {
        if (!methodCache.ContainsKey(cacheKey))
        {
            MethodInfo method = GetStaticPublicMethodInfo(methodName, paramTypes);
            if (method == null) return false;
            // 创建委托
            methodCache[cacheKey] = Delegate.CreateDelegate(delegateType, method);
        }

        try
        {
            methodCache[cacheKey].DynamicInvoke(parameters);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"委托调用{methodName}失败: {ex.Message}");
            return false;
        }
    }
}