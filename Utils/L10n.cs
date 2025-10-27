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
                        "价重比"   => "价重比",
                        "显示价格按钮"   => "显示价格按钮",
                        "显示重量按钮"   => "显示重量按钮",  
                        "显示价重比按钮" => "显示价重比按钮",
                        "显示价格信息"   => "显示价格信息",  
                        "显示价重比信息" => "显示价重比信息",
                        "是否升序排序"   => "是否升序排序",  
                        _ => buttonType
                    };

                case SystemLanguage.ChineseTraditional:
                    return buttonType switch
                    {
                        "按价值"   => "按價值",
                        "按重量"   => "按重量",
                        "按价重比" => "按價重比",
                        "价重比"   => "價重比",
                        "显示价格按钮" => "顯示價格按鈕",
                        "显示重量按钮" => "顯示重量按鈕",
                        "显示价重比按钮" => "顯示價重比按鈕",
                        "显示价格信息" => "顯示價格資訊",
                        "显示价重比信息" => "顯示價重比資訊",
                        "是否升序排序" => "是否升序排序",

                        _ => buttonType
                    };

                case SystemLanguage.Japanese:
                    return buttonType switch
                    {
                        "按价值"   => "価値順",
                        "按重量"   => "重量順",
                        "按价重比" => "価値/重量順",
                        "价重比"   => "価値/重量",
                        "显示价格按钮"   => "価格ボタンを表示",
                        "显示重量按钮"   => "重量ボタンを表示"  ,
                        "显示价重比按钮" => "価格／重量比ボタンを表示"  ,
                        "显示价格信息"   => "価格情報を表示"  ,
                        "显示价重比信息" => "価格／重量比情報を表示"  ,
                        "是否升序排序"   => "昇順で並べ替えるかどうか",

                        _ => buttonType
                    };

                case SystemLanguage.German:
                    return buttonType switch
                    {
                        "按价值"   => "Wert",
                        "按重量"   => "Gewicht",
                        "按价重比" => "W/G",
                        "价重比"   => "W/G",
                        "显示价格按钮" => "Preisschaltfläche anzeigen",
                        "显示重量按钮" => "Gewichtsschaltfläche anzeigen",
                        "显示价重比按钮" => "Preis-Gewichts-Verhältnis-Schaltfläche anzeigen",
                        "显示价格信息" => "Preisinformation anzeigen",
                        "显示价重比信息" => "Preis-Gewichts-Verhältnis-Information anzeigen",
                        "是否升序排序" => "Aufsteigend sortieren",

                        _ => buttonType
                    };

                case SystemLanguage.Russian:
                    return buttonType switch
                    {
                        "按价值"   => "Ценность",
                        "按重量"   => "Вес",
                        "按价重比" => "Ц/В",
                        "价重比"   => "Ц/В",
                        "显示价格按钮" => "Показать кнопку Ценность",
                        "显示重量按钮" => "Показать кнопку веса",
                        "显示价重比按钮" => "Показать кнопку Ц/В",
                        "显示价格信息" => "Показать информацию о цене",
                        "显示价重比信息" => "Показать информацию о Ц/В",
                        "是否升序排序" => "Сортировать по возрастанию",
                        _ => buttonType
                    };

                case SystemLanguage.Spanish:
                    return buttonType switch
                    {
                        "按价值"   => "Valor",
                        "按重量"   => "Peso",
                        "按价重比" => "V/P",
                        "价重比"   => "V/P",
                        "显示价格按钮" => "Mostrar botón de precio",
                        "显示重量按钮" => "Mostrar botón de peso",
                        "显示价重比按钮" => "Mostrar botón de relación precio-peso",
                        "显示价格信息" => "Mostrar información de precio",
                        "显示价重比信息" => "Mostrar información de relación precio-peso",
                        "是否升序排序" => "Ordenar de forma ascendente",
                        _ => buttonType
                    };

                case SystemLanguage.Korean:
                    return buttonType switch
                    {
                        "按价值"   => "가치",
                        "按重量"   => "무게",
                        "按价重比" => "가치/무게",
                        "价重比"   => "가치/무게",
                        "显示价格按钮" => "가격 버튼 표시",
                        "显示重量按钮" => "무게 버튼 표시",
                        "显示价重比按钮" => "가격/무게 비율 버튼 표시",
                        "显示价格信息" => "가격 정보 표시",
                        "显示价重比信息" => "가격/무게 비율 정보 표시",
                        "是否升序排序" => "오름차순 정렬 여부",
                        _ => buttonType
                    };

                case SystemLanguage.French:
                    return buttonType switch
                    {
                        "按价值"   => "Valeur",
                        "按重量"   => "Poids",
                        "按价重比" => "V/P",
                        "价重比"   => "V/P",
                        "显示价格按钮" => "Afficher le bouton de prix",
                        "显示重量按钮" => "Afficher le bouton de poids",
                        "显示价重比按钮" => "Afficher le bouton du rapport prix/poids",
                        "显示价格信息" => "Afficher les informations de prix",
                        "显示价重比信息" => "Afficher les informations du rapport prix/poids",
                        "是否升序排序" => "Trier par ordre croissant",
                        _ => buttonType
                    };

                case SystemLanguage.Portuguese:
                    return buttonType switch
                    {
                        "按价值"   => "Valor",
                        "按重量"   => "Peso",
                        "按价重比" => "V/P",
                        "价重比"   => "V/P",
                        "显示价格按钮" => "Mostrar botão de Valor",
                        "显示重量按钮" => "Mostrar botão de peso",
                        "显示价重比按钮" => "Mostrar botão de relação V/P",
                        "显示价格信息" => "Mostrar informações de preço",
                        "显示价重比信息" => "Mostrar informações de relação V/P",
                        "是否升序排序" => "Ordenar em ordem crescente",
                        _ => buttonType
                    };

                case SystemLanguage.English:
                default:
                    return buttonType switch
                    {
                        "按价值"   => "Value",
                        "按重量"   => "Weight",
                        "按价重比" => "Val/Weight",
                        "价重比"   => "Val/Weight",
                        "显示价格按钮" => "Show Price Button",
                        "显示重量按钮" => "Show Weight Button",
                        "显示价重比按钮" => "Show Price-to-Weight Button",
                        "显示价格信息" => "Show Price Info",
                        "显示价重比信息" => "Show Price-to-Weight Info",
                        "是否升序排序" => "Sort in Ascending Order",
                        _ => buttonType
                    };
            }
        }
    }
}
