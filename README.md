**🌐[ 中文 | [English](README_en.md) ]**

[📝更新日志](CHANGELOG.md)

[📦 Releases](https://github.com/JMC2002/Duckov-Sort/releases)

# ⚖️ 按重量/价值/价重比排序
## 🧩 0. 安装
Steam版本直接在创意工坊订阅即可，👉 [创意工坊页面](https://steamcommunity.com/sharedfiles/filedetails/?id=3592004817)

其他版本可以自行编译，或者在[📦 Releases](https://github.com/JMC2002/Duckov-Sort/releases)界面下载DuckSort.zip后解压到游戏安装目录下的Mods
目录下（没有就新建一个），文件结构如下：
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
> ⚠️ release界面会随着创意工坊更新，最新版本以源代码为准，项目文件夹里的那个DLL也是最新的，但开发阶段不一定能用

## 🧠 1. 简介
刚上手游戏一会就想着按价格或者重量排序物品，
方便扔垃圾，结果等几天都没人做这个mod，干脆自己动手了

[演示视频（B站）](https://www.bilibili.com/video/BV1uBsBzMEm4/?vd_source=a23dec0dc1d809e1d014dd2f9135e10b#reply278472261105)

## ⚙️ 2. 功能
- 背包和仓库的“整理”按钮下方多出一行排序选项，对应按价格、重量、
价格/重量比排序
- 模组可以自己设置显示与隐藏的项，默认显示**按价格、重量、价重比排序按钮**与**物品价格文本**，默认隐藏**按稀有度、单价排序按钮与价重比文本**，如需自定义，可以在存档路径（**存档路径、而非游戏路径**，在存档选择的界面可以找到路径）下找到“**DuckSortConfig.json**”（如果没有这个文件，需要打开一次游戏后生成，删除可以恢复默认设置），将对应的键值改为 **true** 或 **false** 即可
- 如欲于游戏内修改设置，订阅并启用ModConfig模组（[ModConfig创意工坊页面](https://steamcommunity.com/sharedfiles/filedetails/?id=3590674339)），将ModConfig模组置于本模组上方（首次启用需要重启游戏方可生效），即可于游戏内设置，目前除了修改排序升序/降序之外的设置均需重启游戏方可生效，以后会优化

## 🔔 3. 提醒
- MOD自带显示物品价格，与其他显示物品价格的
MOD冲突，同时打开会重复显示价格，此时按上面的方法关闭本MOD的价格显示即可
- **按稀有度排序**的*稀有度*为游戏内置的物品稀有度，类似于MOD[炫彩物品与搜索音效](https://steamcommunity.com/sharedfiles/filedetails/?id=3588329796)使用的规则
- **按单价排序**主要提供给使用了无限堆叠MOD的玩家使用
- 本MOD的本地化文本（包括按钮与README）除了中文外均借助自动翻译生成，欢迎贡献文本，只需要提交PR或者在评论区留言即可，日文由朋友[さっちゃん](https://steamcommunity.com/profiles/76561199492777489/)帮忙完成。

## 🧩 4. 兼容性
- 本MOD涉及到UI，大面积重绘UI的MOD可能会与本MOD冲突
- 本MOD使用Harmony框架，其他与Harmony冲突的MOD可能会与本MOD冲突
- 本MOD大面积使用反射，可能会随着游戏版本更新而失效

## 🧭 5. TODO
- 目前默认按降序排序，后续可能会加个升序/降序切换按钮
- 目前UI较为粗糙，后续可能会优化UI
- 后续可能会增添配置菜单，方便快捷打开/关闭一些功能
- 目前代码写得很粗糙，后续应该会优化代码结构 ✔️
- 目前排序似乎有点慢，后面可能会看看优化性能 ✔️

**如果你喜欢这个 Mod 的话，希望可以点一个star~**