# 🧾 Changelog

所有对本项目的重要更改都将记录在此文件中。

版本号规则： 主版本号.次版本号.修订号，其中主版本号涉及到大功能完善，次版本号原则上Steam创意工坊发布新版本后进行更新，修订号每次涉及代码的commit后更新（从0开始）。

## [1.4.3] - 2025-10-26
### Fixed
上一个版本推送错误，导致本应该默认关闭显示的价重比默认打开显示、无法通过修改json文件修改价重比、价值显示与否，此版本接入了[ModConfig](https://steamcommunity.com/sharedfiles/filedetails/?id=3590674339)，可以在游戏内修改设置，**但是由于是紧急推送的版本，目前除了排序顺序外的选项均需要完全退出游戏后重新进入方能生效**，这个问题很快会解决。

### 提示
- 如果你不想订阅ModConfig，可以按上次更新提到的方法手动修改DuckSortConfig.json文件（如果存档位置没有需要打开游戏一次），启用ModConfig模组需要将该模组置于本模组上方，且首次启用需要重启游戏方可生效。


## [1.4.2] - 2025-10-26
### Added
- 新增配置修改，暂时没做游戏内的，有需要的可以手动修改存档目录（可以在选择存档界面的左下角点击复制存档路径）下的`DuckSortConfig.json`，第一次更新需要打开游戏一次生成，其中`ShowPriceButton`、`ShowWeightButton`、`ShowRatioButton`控制三个按钮是否显示，`ShowPriceText`与`ShowRatioText`控制是否显示物品的价格与价重比（注：游戏内显示的重量为四舍五入值，因此可能出现显示的价重比与你手动计算不符的情况），`DefaultSortAscending`控制默认排序规则，其中`true`代表升序，`false`代表降序。很快会接入游戏内配置界面。默认配置如下：
```json
{
  "ShowPriceButton": true,
  "ShowWeightButton": true,
  "ShowRatioButton": true,
  "ShowPriceText": true,
  "ShowRatioText": false,
  "DefaultSortAscending": false
}
```
### 下一个版本计划
- 游戏内配置界面
- 按钮切换升序/降序
- 添加按稀有度排序的功能

## [1.3.1] - 2025-10-25
### Changed
- 优化韩语显示
### Fixed
- 修复了上次更新带来的简体中文以外的语言显示异常问题

## [1.2.9] - 2025-10-25
### Changed
- 大面积重构代码，预留隐藏按钮、新增按钮、升降序的空间；
- 大幅度优化了排序速度，减少到原来的1/8
### Fixed
- 修复了相同价格时相同物品可能不放在一起的BUG；

## [1.1.6] - 2025-10-24
### Changed
- 大面积重构了代码结构，正规化了log信息 

### Fixed
- 修修复了英文版本按钮字体换行的问题

## [1.0.0] - 2025-10-23
### Added
- 项目初始版本发布
