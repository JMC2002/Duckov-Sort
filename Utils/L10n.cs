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
                        "Value" => "按价值",
                        "Weight" => "按重量",
                        "Ratio" => "按价重比",
                        _ => buttonType
                    };

                case SystemLanguage.ChineseTraditional:
                    return buttonType switch
                    {
                        "Value" => "按價值",
                        "Weight" => "按重量",
                        "Ratio" => "按價重比",
                        _ => buttonType
                    };

                case SystemLanguage.Japanese:
                    return buttonType switch
                    {
                        "Value" => "価値順",
                        "Weight" => "重量順",
                        "Ratio" => "価値/重量順",
                        _ => buttonType
                    };

                case SystemLanguage.German:
                    return buttonType switch
                    {
                        "Value" => "Wert",
                        "Weight" => "Gewicht",
                        "Ratio" => "W/G",
                        _ => buttonType
                    };

                case SystemLanguage.Russian:
                    return buttonType switch
                    {
                        "Value" => "Ценность",
                        "Weight" => "Вес",
                        "Ratio" => "Ц/В",
                        _ => buttonType
                    };

                case SystemLanguage.Spanish:
                    return buttonType switch
                    {
                        "Value" => "Valor",
                        "Weight" => "Peso",
                        "Ratio" => "V/P",
                        _ => buttonType
                    };

                case SystemLanguage.Korean:
                    return buttonType switch
                    {
                        "Value" => "가치",
                        "Weight" => "무게",
                        "Ratio" => "가/무",
                        _ => buttonType
                    };

                case SystemLanguage.French:
                    return buttonType switch
                    {
                        "Value" => "Valeur",
                        "Weight" => "Poids",
                        "Ratio" => "V/P",
                        _ => buttonType
                    };

                case SystemLanguage.Portuguese:
                    return buttonType switch
                    {
                        "Value" => "Valor",
                        "Weight" => "Peso",
                        "Ratio" => "V/P",
                        _ => buttonType
                    };

                case SystemLanguage.English:
                default:
                    return buttonType switch
                    {
                        "Value" => "Value",
                        "Weight" => "Weight",
                        "Ratio" => "Val/Weight",
                        _ => buttonType
                    };
            }
        }
    }
}
