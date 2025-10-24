using SodaCraft.Localizations;
using UnityEngine;

namespace DuckSort.Utils
{
    public static class L10n
    {
        public static string GetLabel(string buttonType)
        {
            SystemLanguage language = LocalizationManager.CurrentLanguage;
            switch (language)
            {
                case SystemLanguage.ChineseSimplified:
                    return buttonType switch
                    {
                        "按价值"   => "按价值",
                        "按重量"   => "按重量",
                        "按价重比" => "按价重比",
                        _ => buttonType
                    };

                case SystemLanguage.ChineseTraditional:
                    return buttonType switch
                    {
                        "按价值"   => "按價值",
                        "按重量"   => "按重量",
                        "按价重比" => "按價重比",
                        _ => buttonType
                    };

                case SystemLanguage.Japanese:
                    return buttonType switch
                    {
                        "按价值"   => "価値順",
                        "按重量"   => "重量順",
                        "按价重比" => "価値/重量順",
                        _ => buttonType
                    };

                case SystemLanguage.German:
                    return buttonType switch
                    {
                        "按价值"   => "Wert",
                        "按重量"   => "Gewicht",
                        "按价重比" => "W/G",
                        _ => buttonType
                    };

                case SystemLanguage.Russian:
                    return buttonType switch
                    {
                        "按价值"   => "Ценность",
                        "按重量"   => "Вес",
                        "按价重比" => "Ц/В",
                        _ => buttonType
                    };

                case SystemLanguage.Spanish:
                    return buttonType switch
                    {
                        "按价值"   => "Valor",
                        "按重量"   => "Peso",
                        "按价重比" => "V/P",
                        _ => buttonType
                    };

                case SystemLanguage.Korean:
                    return buttonType switch
                    {
                        "按价值"   => "가치",
                        "按重量"   => "무게",
                        "按价重比" => "가/무",
                        _ => buttonType
                    };

                case SystemLanguage.French:
                    return buttonType switch
                    {
                        "按价值"   => "Valeur",
                        "按重量"   => "Poids",
                        "按价重比" => "V/P",
                        _ => buttonType
                    };

                case SystemLanguage.Portuguese:
                    return buttonType switch
                    {
                        "按价值"   => "Valor",
                        "按重量"   => "Peso",
                        "按价重比" => "V/P",
                        _ => buttonType
                    };

                case SystemLanguage.English:
                default:
                    return buttonType switch
                    {
                        "按价值"   => "Value",
                        "按重量"   => "Weight",
                        "按价重比" => "Val/Weight",
                        _ => buttonType
                    };
            }
        }
    }
}
