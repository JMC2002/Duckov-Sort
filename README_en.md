> Note: The English page is translated by GPT and may not be updated in a timely manner. The latest version is based on the Chinese page. The DLL in the project folder is also the latest version, but it may not be usable during the development stage.

**🌐[ [中文](README.md) | English ]**
    
[📝 Change log](CHANGELOG.md)

[📦 Releases](https://github.com/JMC2002/Duckov-Sort/releases)
# ⚖️ Sorting by Weight/Value/Value-to-Weight Ratio
## 🧩  0. Installation

For the Steam version, you can simply subscribe through the Steam Workshop: [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3592004817)

For other versions, you can either compile it yourself or download the `DuckSort.zip` in [📦 Releases](https://github.com/JMC2002/Duckov-Sort/releases), and then extract it to the `Mods` directory under your game installation folder (create the folder if it doesn't exist). The file structure should look like this:

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
> ⚠️ The release interface will be updated along with the Workshop. The latest version is subject to the source code.

## 🧠 1. Introduction
After playing the game for a while, I wanted to sort items by price or weight to make it easier to throw away junk. After waiting for several days without anyone creating this mod, I decided to make it myself.

## ⚙️ 2. Features
- Added an extra row of sorting options below the “Sort” button in the backpack and stash, allowing sorting by **Value**, **Weight**, and **Val/Weight**.
- The mod allows customization of which buttons and texts are shown. By default, the following are **enabled**: **Sort by Price**, **Sort by Weight**, **Sort by Price/Weight Ratio buttons**, and **Item Price Text**; while **disabled** by default are **Sort by Rarity**, **Sort by Unit Price buttons**, and **Price/Weight Ratio Text**.  
- To modify settings in-game, subscribe to and enable the [ModConfig](https://steamcommunity.com/sharedfiles/filedetails/?id=3590674339) mod.  
  Place **ModConfig** above this mod in the load order (a restart is required the first time). You can then configure settings directly in-game.  
  Currently, changes other than sort order (ascending/descending) require restarting the game to take effect, but this will be improved in future updates.

## 🔔 3. Notes
- This mod includes its own item price display, which may conflict with other mods that show item prices. If duplicate prices appear, simply disable this mod’s price display as described above.
- **Sort by Rarity** uses the game’s built-in item rarity system, similar to the rules used in [Fancy Items & Sounds](https://steamcommunity.com/sharedfiles/filedetails/?id=3588329796).
- **Sort by Unit Price** is mainly designed for players using infinite stack mods.
- The localization texts (including buttons and README) for this mod, except for the Chinese language, were generated with the help of automatic translation. Contributions for text improvements are welcome. You can submit a PR or leave a comment.  Contributions for text improvements are welcome. You can submit a PR or leave a comment. The Japanese translation was kindly provided by my friend [さっちゃん](https://steamcommunity.com/profiles/76561199492777489/).

## 🧩 4. Compatibility
- This mod involves UI changes. Mods that heavily modify the UI may conflict with this one.
- This mod uses the Harmony framework. Other mods that conflict with Harmony might cause issues with this mod.
-  This mod heavily relies on reflection, so it may become incompatible after future game updates.

## 🧭 5. TODO
- Currently, the mod sorts in descending order by default. I may add an ascending/descending toggle button in the future. ✔️
- The UI is quite basic at the moment and may be improved in the future. ✔️
- A configuration menu may be added to quickly enable/disable certain features. ✔️
- The code is a bit rough at the moment and will likely be optimized in the future. ✔️
- The sorting seems a bit slow, and I will look into performance optimization later. ✔️

**If you like this Mod, please consider giving it a thumbs-up!**