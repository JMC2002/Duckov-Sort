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
                        "按价值"             => "按价值",
                        "按重量"             => "按重量",
                        "按价重比"           => "按价重比",
                        "价重比"             => "价重比",
                        "按稀有度"           => "按稀有度",
                        "按单价"             => "按单价",
                        "按ID"               => "按ID",
                        "显示稀有度按钮"     => "显示稀有度按钮",
                        "显示单价按钮"       => "显示单价按钮",
                        "显示价格按钮"       => "显示价格按钮",
                        "显示重量按钮"       => "显示重量按钮",  
                        "显示价重比按钮"     => "显示价重比按钮",
                        "显示ID按钮"         => "显示ID按钮",
                        "显示价格信息"       => "显示价格信息",  
                        "显示价重比信息"     => "显示价重比信息",
                        "交换左右键排序方向" => "交换左右键排序方向",
                        "启用调试日志"       => "启用调试日志",
                        "单价"               => "单价",
                        "显示单价信息"       => "显示单价信息",
                        _ => buttonType
                    };

                case SystemLanguage.ChineseTraditional:
                    return buttonType switch
                    {
                        "按价值"             => "按價值",
                        "按重量"             => "按重量",
                        "按价重比"           => "按價重比",
                        "价重比"             => "價重比",
                        "按稀有度"           => "按稀有度",
                        "按单价"             => "按單價",
                        "按ID"               => "按ID",
                        "显示稀有度按钮"     => "顯示稀有度按鈕",
                        "显示单价按钮"       => "顯示單價按鈕",
                        "显示价格按钮"       => "顯示價格按鈕",
                        "显示重量按钮"       => "顯示重量按鈕",
                        "显示价重比按钮"     => "顯示價重比按鈕",
                        "显示ID按钮"         => "顯示ID按鈕",
                        "显示价格信息"       => "顯示價格資訊",
                        "显示价重比信息"     => "顯示價重比資訊",
                        "交换左右键排序方向" => "交換左右鍵排序方向",
                        "启用调试日志"       => "啟用除錯日誌",
                        "单价"               => "單價",
                        "显示单价信息"       => "顯示單價資訊",

                        _ => buttonType
                    };

                case SystemLanguage.Japanese:
                    return buttonType switch
                    {
                        "按价值"             => "価値順",
                        "按重量"             => "重量順",
                        "按价重比"           => "価値/重量順",
                        "价重比"             => "価値/重量",
                        "按稀有度"           => "レアリティ順",
                        "按单价"             => "単価順",
                        "按ID"               => "ID順",
                        "显示稀有度按钮"     => "レアリティボタンを表示",
                        "显示单价按钮"       => "単価ボタンを表示",
                        "显示价格按钮"       => "価格ボタンを表示",
                        "显示重量按钮"       => "重量ボタンを表示"  ,
                        "显示价重比按钮"     => "価格／重量比ボタンを表示"  ,
                        "显示ID按钮"         => "IDボタンを表示",
                        "显示价格信息"       => "価格情報を表示"  ,
                        "显示价重比信息"     => "価格／重量比情報を表示"  ,
                        "交换左右键排序方向" => "並べ替え操作の左右クリックの機能を反転",
                        "启用调试日志"       => "デバッグログを有効にする",
                        "单价"               => "単価",
                        "显示单价信息"       => "単価情報を表示",

                        _ => buttonType
                    };

                case SystemLanguage.German:
                    return buttonType switch
                    {
                        "按价值"             => "Wert",
                        "按重量"             => "Gewicht",
                        "按价重比"           => "W/G",
                        "价重比"             => "W/G",
                        "按稀有度"           => "Seltenheit",
                        "按单价"             => "Einzelpreis",
                        "按ID"               => "Nach ID",
                        "显示稀有度按钮"     => "Seltenheitsschaltfläche",
                        "显示单价按钮"       => "Einzelpreisschaltfläche",
                        "显示价格按钮"       => "Preisschaltfläche anzeigen",
                        "显示重量按钮"       => "Gewichtsschaltfläche anzeigen",
                        "显示价重比按钮"     => "Preis-Gewichts-Verhältnis-Schaltfläche anzeigen",
                        "显示ID按钮"         => "ID-Schaltfläche anzeigen",
                        "显示价格信息"       => "Preisinformation anzeigen",
                        "显示价重比信息"     => "Preis-Gewichts-Verhältnis-Information anzeigen",
                        "交换左右键排序方向" => "Links-/Rechtsklick-Sortierrichtung tauschen",
                        "启用调试日志"       => "Debug-Log aktivieren",
                        "单价"               => "Einzelpreis",
                        "显示单价信息"       => "Einzelpreis anzeigen",

                        _ => buttonType
                    };

                case SystemLanguage.Russian:
                    return buttonType switch
                    {
                        "按价值"             => "Ценность",
                        "按重量"             => "Вес",
                        "按价重比"           => "Ц/В",
                        "价重比"             => "Ц/В",
                        "按稀有度"           => "Редкость",
                        "按单价"             => "Цена",
                        "按ID"               => "По ID",
                        "显示稀有度按钮"     => "Кнопка редкости",
                        "显示单价按钮"       => "Кнопка цены",
                        "显示价格按钮"       => "Показать кнопку Ценность",
                        "显示重量按钮"       => "Показать кнопку веса",
                        "显示价重比按钮"     => "Показать кнопку Ц/В",
                        "显示ID按钮"         => "Показать кнопку ID",
                        "显示价格信息"       => "Показать информацию о цене",
                        "显示价重比信息"     => "Показать информацию о Ц/В",
                        "交换左右键排序方向" => "Поменять местами направление сортировки левой и правой кнопок",
                        "启用调试日志"       => "Включить журнал отладки",
                        "单价"               => "Цена за единицу",
                        "显示单价信息"       => "Показать информацию о цене за единицу",


                        _ => buttonType
                    };

                case SystemLanguage.Spanish:
                    return buttonType switch
                    {
                        "按价值"             => "Valor",
                        "按重量"             => "Peso",
                        "按价重比"           => "V/P",
                        "价重比"             => "V/P",
                        "按稀有度"           => "Rareza",
                        "按单价"             => "Precio",
                        "按ID"               => "Por ID",
                        "显示稀有度按钮"     => "Botón de rareza",
                        "显示单价按钮"       => "Botón de precio",
                        "显示价格按钮"       => "Mostrar botón de precio",
                        "显示重量按钮"       => "Mostrar botón de peso",
                        "显示价重比按钮"     => "Mostrar botón de relación precio-peso",
                        "显示ID按钮"         => "Mostrar botón de ID",
                        "显示价格信息"       => "Mostrar información de precio",
                        "显示价重比信息"     => "Mostrar información de relación precio-peso",
                        "交换左右键排序方向" => "Intercambiar la dirección de orden con los clics izquierdo y derecho",
                        "启用调试日志"       => "Habilitar registro de depuración",
                        "单价"               => "Precio unitario",
                        "显示单价信息"       => "Mostrar información del precio unitario",


                        _ => buttonType
                    };

                case SystemLanguage.Korean:
                    return buttonType switch
                    {
                        "按价值"             => "가치",
                        "按重量"             => "무게",
                        "按价重比"           => "가치/무게",
                        "价重比"             => "가치/무게",
                        "按稀有度"           => "희귀도",
                        "按单价"             => "단가",
                        "按ID"               => "ID순으로",
                        "显示稀有度按钮"     => "희귀도 버튼",
                        "显示单价按钮"       => "단가 버튼",
                        "显示价格按钮"       => "가격 버튼 표시",
                        "显示重量按钮"       => "무게 버튼 표시",
                        "显示价重比按钮"     => "가격/무게 비율 버튼 표시",
                        "显示ID按钮"         => "ID 버튼 표시",
                        "显示价格信息"       => "가격 정보 표시",
                        "显示价重比信息"     => "가격/무게 비율 정보 표시",
                        "交换左右键排序方向" => "좌우 클릭 정렬 방향 전환",
                        "启用调试日志"       => "디버그 로그 활성화",
                        "单价"               => "단가",
                        "显示单价信息"       => "단가 정보 표시",


                        _ => buttonType
                    };

                case SystemLanguage.French:
                    return buttonType switch
                    {
                        "按价值"             => "Valeur",
                        "按重量"             => "Poids",
                        "按价重比"           => "V/P",
                        "价重比"             => "V/P",
                        "按稀有度"           => "Rareté",
                        "按单价"             => "Unitaire",
                        "按ID"               => "Par ID",
                        "显示稀有度按钮"     => "Afficher le bouton de rareté",
                        "显示单价按钮"       => "Afficher le bouton de prix unitaire",
                        "显示价格按钮"       => "Afficher le bouton de Valeur",
                        "显示重量按钮"       => "Afficher le bouton de poids",
                        "显示价重比按钮"     => "Afficher le bouton du rapport valeur/poids",
                        "显示ID按钮"         => "Afficher le bouton ID",
                        "显示价格信息"       => "Afficher les informations de prix",
                        "显示价重比信息"     => "Afficher les informations du rapport valeur/poids",
                        "交换左右键排序方向" => "Inverser la direction de tri des clics gauche/droite",
                        "启用调试日志"       => "Activer le journal de débogage",
                        "单价"               => "Unitaire",
                        "显示单价信息"       => "Afficher les informations sur le prix unitaire",

                        _ => buttonType
                    };

                case SystemLanguage.Portuguese:
                    return buttonType switch
                    {
                        "按价值"             => "Valor",
                        "按重量"             => "Peso",
                        "按价重比"           => "V/P",
                        "价重比"             => "V/P",
                        "按稀有度"           => "Raridade",
                        "按单价"             => "Preço",
                        "按ID"               => "Por ID",
                        "显示稀有度按钮"     => "Botão de raridade",
                        "显示单价按钮"       => "Botão de preço",
                        "显示价格按钮"       => "Mostrar botão de Valor",
                        "显示重量按钮"       => "Mostrar botão de peso",
                        "显示价重比按钮"     => "Mostrar botão de relação V/P",
                        "显示ID按钮"         => "Mostrar botão de ID",
                        "显示价格信息"       => "Mostrar informações de preço",
                        "显示价重比信息"     => "Mostrar informações de relação V/P",
                        "交换左右键排序方向" => "Inverter a direção de ordenação dos cliques esquerdo/direito",
                        "启用调试日志"       => "Ativar registro de depuração",
                        "单价"               => "Preço unitário",
                        "显示单价信息"       => "Mostrar informações de preço unitário",

                        _ => buttonType
                    };

                case SystemLanguage.English:
                default:
                    return buttonType switch
                    {
                        "按价值"             => "Value",
                        "按重量"             => "Weight",
                        "按价重比"           => "Val/Weight",
                        "价重比"             => "Val/Weight",
                        "按稀有度"           => "Rarity",
                        "按单价"             => "Unit Price",
                        "按ID"               => "By ID",
                        "显示稀有度按钮"     => "Show Rarity Button",
                        "显示单价按钮"       => "Show Unit Price Button",
                        "显示价格按钮"       => "Show Price Button",
                        "显示重量按钮"       => "Show Weight Button",
                        "显示价重比按钮"     => "Show Price-to-Weight Button",
                        "显示ID按钮"         => "Show ID button",
                        "显示价格信息"       => "Show Price Info",
                        "显示价重比信息"     => "Show Price-to-Weight Info",
                        "交换左右键排序方向" => "Swap left and right click sorting direction",
                        "启用调试日志"       => "Enable debug log",
                        "单价"               => "Unit Price",
                        "显示单价信息"       => "Show unit price info",

                        _ => buttonType
                    };
            }
        }
    }
}
