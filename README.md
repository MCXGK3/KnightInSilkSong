# KnightInSilkSong

## Description 描述

It allows you to switch between the Knight and Hornet in the game.
这个mod可以使你在游戏中进行小骑士与大黄蜂之间进行切换

## Installation 安装

1. Install [BepInEx](https://github.com/BepInEx/BepInEx/releases) for Hollow Knight Silksong if you haven't already.如果没有为《空洞骑士：丝之歌》下载过BepInEx mod，下载[BepInEx](https://github.com/BepInEx/BepInEx/releases)。
2. Download the latest release of the Knight Mod from the [Releases](https://github.com/MCXGK3/KnightInSilkSong/releases) page.从[Releases](https://github.com/MCXGK3/KnightInSilkSong/releases) 界面获取最新的发行版。
3. Extract contents from the downloaded ZIP file and copy them into the BepInEx's `plugins` folder.将下载到的zip文件解压并复制到BepInEx的 `plugins`文件夹。
4. If you use ThunderStore, you can download it directly from [there](https://thunderstore.io/c/hollow-knight-silksong/p/shownyoung/KnightInSilksong/) without following the steps above.
   如果你使用ThunderStore，那么你可以不经过上述步骤直接从这里下载。

> Developed and tested on Windows platform. Compatibility with other platforms is not guaranteed.
> 在Windows平台下开发和测试，与其他平台的兼容性不能保证。

## Basic Usage

1. Launch Hollow Knight Silksong with BepInEx.打开安装了BepInEx与本mod的游戏本体。
2. During gameplay, press the key you set (defalut is F5) to toggle between the Knight and Hornet.在游戏中，按下你所设下的切换键（默认为F5）来进行小骑士与大黄蜂间的切换。
3. To adjust the status of the knight (such as nail damage, etc.), you can modify the `BepInEx\config\SlotX\PlayerData.json` (which appears after saving once), and other settings (such as damage modifier, etc.) can be modified by editing 'Hollow Knight SilksongBepInExconfigio.github.shownyoung.knightinsilksong.cfg'.要对小骑士的状态（如骨钉伤害等）进行调整，可以修改 `BepInEx\config\SlotX\PlayerData.json`文件（保存一次后出现），其他设置（如伤害修正等）可以修改 `Hollow Knight Silksong\BepInEx\config\io.github.shownyoung.knightinsilksong.cfg`来完成。
4. The option for ability synchronization is also set in `BepInEx\config\SlotX\SlotSetting.cfg`. 同步能力的选项也在 `BepInEx\config\SlotX\SlotSetting.cfg`中设置。
5. This mod is now compatible with skin mods, and the names of the images in customknight correspond as follows (taking Patchwork as an example). However, please note that you must obtain the consent of the skin creator before using the skin in SS. 此mod现在可以与皮肤mod兼容，其在CK中的皮肤图片对应路径如下所示（以Patchwork为例）。但需要注意，在丝之歌中使用皮肤前请获得皮肤作者的同意。

| name in CK     | path for Patchwork                                                   |
| -------------- | -------------------------------------------------------------------- |
| Knight.png     | Patchwork\Spritesheets\Knight(Hollownest)\atlas0.png                 |
| Hud.png        | Patchwork\Spritesheets\HUD Cln(Hollownest)\atlas0.png                |
| Geo.png        | Patchwork\Spritesheets\Geo(Hollownest)\atlas0.png                    |
| Hatchling.png  | Patchwork\Spritesheets\\Knight Hatchling Cln(Hollownest)\atlas0.png  |
| Wraiths.png    | Patchwork\Spritesheets\\Spell Effects 2(Hollownest)\atlas0.png       |
| Grimm.png      | Patchwork\Spritesheets\\Grimmchild Cln(Hollownest)\atlas0.png        |
| VoidSpells.png | Patchwork\Spritesheets\\Spell Effects Neutral(Hollownest)\atlas0.png |
| Unn.png        | Patchwork\Spritesheets\\Knight Slug Cln(Hollownest)\atlas0.png       |
| VS.png         | Patchwork\Spritesheets\\Spell Effects(Hollownest)\atlas0.png         |
| Baldur.png     | Patchwork\Spritesheets\\Charm Blocker Cln(Hollownest)\atlas0.png     |
| Shield.png     | Patchwork\Spritesheets\\Orbit Shield Cln(Hollownest)\atlas0.png      |
| Weaver.png     | Patchwork\Spritesheets\\Weaverling Cln(Hollownest)\atlas0.png        |
| Liquid.png     | Patchwork\Spritesheets\\HUD_Soulorb_fills(Hollownest)\atlas0.png     |
| Sprint.png     | Patchwork\Spritesheets\\Knight Dream Gate Cln(Hollownest)\atlas0.png |
| OrbFull.png    | Patchwork\Sprites\T2D\soul_orb_glow0000.png                          |

## Special Thanks

Credit to @olvior for the effort to enable the knight to beat the game completely. 感谢@olvior为使小骑士能够正常完成游戏所做的努力。

Credit to @ManicJamie for helping update compatibility with DebugMod 1.0. 感谢@ManicJamie帮助更新了与DebugMod1.0的兼容。

## Future Plans

- Fix a lot of bugs. 修很多bug
- The processing required to achieve ending 2 and 3. 实现结局二三所需的处理

## Attention

- This mod is not allowed to be ported to pirated versions such as the mobile version of SS.此mod不允许被移植到如手机版丝之歌等盗版上。
