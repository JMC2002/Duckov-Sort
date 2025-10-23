**[ [中文](README.md) | English ]**

# Sorting by Weight/Value/Value-to-Weight Ratio
## 0. Installation

For the Steam version, you can simply subscribe through the Steam Workshop: [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3592004817)

For other versions, you can either compile it yourself or download the `DuckSort.zip`, and then extract it to the `Mods` directory under your game installation folder (create the folder if it doesn't exist). The file structure should look like this:

```sh
-- Escape from Duckov
    |-- Duckov.exe
    |-- Duckov_Data
         |-- Mods
              |-- DuckSort
                   |-- 0Harmony.dll
                   |-- DuckSort.dll
                   |-- info.ini
                   |-- preview.png
```
Please note that this zip may not always be updated regularly. For the latest version, refer to the Steam Workshop or the source code.

## 1. Introduction
After playing the game for a while, I wanted to sort items by price or weight to make it easier to throw away junk. After waiting for several days without anyone creating this mod, I decided to make it myself.

## 2. Features
A new row of sorting options appears below the "Sort" button in both the backpack and warehouse, allowing you to sort by price, weight, or value-to-weight ratio.

## 3. Notes
- The mod comes with a feature that displays item prices. This may conflict with other mods that display item prices. If you have both enabled, the price will be displayed twice. It is recommended to disable other price-display mods. I may consider adding a toggle option for this feature in the future.
- The localization texts (including buttons and README) for this mod, except for the Chinese language, were generated with the help of automatic translation. Contributions for text improvements are welcome. You can submit a PR or leave a comment.

## 4. Compatibility
- This mod involves UI changes. Mods that heavily modify the UI may conflict with this one.
- This mod uses the Harmony framework. Other mods that conflict with Harmony might cause issues with this mod.

## 5. TODO
- Currently, the mod sorts in descending order by default. I may add an ascending/descending toggle button in the future.
- The UI is quite basic at the moment and may be improved in the future.
- A configuration menu may be added to quickly enable/disable certain features.
- The code is a bit rough at the moment and will likely be optimized in the future.
- The sorting seems a bit slow, and I will look into performance optimization later.