# KnightInSilkSong

## Description 描述
It allows you to switch between the Knight and Hornet in the game.\
这个mod可以使你在游戏中进行小骑士与大黄蜂之间进行切换

## Installation 安装

1. Install [BepInEx](https://github.com/BepInEx/BepInEx/releases) for Hollow Knight Silksong if you haven't already.\
如果没有为《空洞骑士：丝之歌》下载过BepInEx mod，下载[BepInEx](https://github.com/BepInEx/BepInEx/releases)。
2. Download the latest release of the Knight Mod from the [Releases](https://github.com/MCXGK3/KnightInSilkSong/releases) page.\
从[Releases](https://github.com/MCXGK3/KnightInSilkSong/releases) 界面获取最新的发行版。
3. Extract contents from the downloaded ZIP file and copy them into the BepInEx's `plugins` folder.\
将下载到的zip文件解压并复制到BepInEx的`plugins`文件夹。
4. If you use ThunderStore, you can download it directly from [there](https://thunderstore.io/c/hollow-knight-silksong/p/shownyoung/KnightInSilksong/) without following the steps above.\
如果你使用ThunderStore，那么你可以不经过上述步骤直接从这里下载。

> Developed and tested on Windows platform. Compatibility with other platforms is not guaranteed.\
在Windows平台下开发和测试，与其他平台的兼容性不能保证。

## Basic Usage

1. Launch Hollow Knight Silksong with BepInEx.\
打开安装了BepInEx与本mod的游戏本体。
2. During gameplay, press the key you set (defalut is F5) to toggle between the Knight and Hornet.\
在游戏中，按下你所设下的切换键（默认为F5）来进行小骑士与大黄蜂间的切换。
3. To adjust the status of the knight (such as nail damage, etc.), you can modify the 'PlayerData.json' file in the same directory of the mod (which appears after playing once), and other settings (such as damage modifier, etc.) can be modified by editing 'Hollow Knight SilksongBepInExconfigio.github.shownyoung.knightinsilksong.cfg'.\
要对小骑士的状态（如骨钉伤害等）进行调整，可以修改mod同目录下的`PlayerData.json`文件（游玩一次后出现），其他设置（如伤害修正等）可以修改`Hollow Knight Silksong\BepInEx\config\io.github.shownyoung.knightinsilksong.cfg`来完成。

## List of Known issues

| Issue Description | Related Report(s) | Fixed? |
|-------------------|-------------------|-------------------|
| When entering a room or sequence that has a special interaction with Hornet as the Knight may cause the game to softlock | #1, #8 | Yes |
| Sometimes when going through vertical transtions as the Knight you may end up not going through the transition succesfully | #19 |  |
| When using the Dreamgate it may cause the players camera to bug out and not be focused on the player | #24 |  |
| When playing as the Knight if you are swimming you may not be able to jump as intended | #18 |  |
| When swimming as the knight the player tends to sink further down instead of just floating in the water as intended | #18 |  |
| Under certain circumstances, the Hiveblood Charm might grant an incorrect number of Hiveblood health segments. Please use with discretion. | #29 |  |
| Under certain circumstances, The Knight's HUD might not appear. Opening and closing the Inventory should restore it. | | Yes |
| Under certain circumstances, Weaverlings or Grimmchild might instantly kill enemies, potentially causing softlocks. Please use them with discretion. | #25 |  |
| Under certain circumstances, The Knight might not play the corresponding animation or Hornet's animation might appear instead. This is a normal phenomenon; please ignore it. | | Yes |
