## ⚠️ This project is a work in progress ⚠️

# NASB2 Custom Music Mod

This mod allows you to add custom songs to the stages, menus, and victory themes in the game!
_____

## 🚀 Installation

### First Time Install

1. Get the BepInEx zip pinned in the NASB discord #modding channel. It has BepInEx 5 and the unstripped Unity libraries.
1. Extract the BepInEx zip into your game's install directory
1. Download the [latest release zip](https://github.com/megalon/nasb2-custom-music-mod/releases/latest) such as `NASB2CustomMusicMod-9.9.9.zip`
1. Extract this zip into your BepInEx folder, and the mod should be installed!

Your BepInEx folder should now have a `CustomSongs` folder, and you should have the plugins in your `plugins` folder
```
BepInEx
    ↳ CustomSongs
    ↳ plugins
        ↳ Steven-Slime_Modding_Utilities
        NASB2CustomMusicMod.dll
        ...
    ...
```

### Updating

If you're updating from a previous version, just extract the latest release zip into your `BepInEx` folder like before, and let it overwrite any files. Any custom songs should be preserved.

## ℹ Usage

For basic usage see the simple format shown below.

For more advanced usage, see [Song Packs](#-song-packs-advanced-usage).

### Folder structure
```
BepInEx
    ↳ CustomSongs
        ↳ Menus
            ↳ MainMenu
            ↳ OnlineMenu
              ...
        ↳ Stages
            ↳ Angry Beavers Dam
            ↳ Bun Wrestling Ring
              ...
        ↳ Victory Themes
            ↳ Aang
            ↳ AngryBeavers
              ...
```

Place `.wav`, `.ogg`, or `.mp3` files into the folder corresponding to the stage or menu you want to change.

For example, if you want to use a different song for the Jellyfish Fields stage, place the audio file in `BepInEx\CustomSongs\Stages\Jellyfish Fields`

If multiple audio files are in the same folder, one is randomly selected each time that stage / menu / victory theme is loaded!

## ⟲ Loop points *(optional)*

To define a loop start and end point, create a JSON file in the same folder as your audio file, and give it the same name as your audio file.

A JSON file is just a regular text file, but instead of `.txt` it is `.json`

You may need to [turn on file extensions on Windows](https://fileinfo.com/help/windows_10_show_file_extensions) to be able to see and edit the `.json` extension.

**File structure example**
```
Stages
    ↳ Jellyfish Fields
        ↳ Song1.wav
        ↳ Song1.json
```

**JSON file contents**

Samples are more precise than seconds, but the NASB1 version of this mod used seconds, so this mod allows both for backwards compatibility.

*Using samples*
```json
{
  "loopPoints": { "start": 199665, "end": 21977555 }
}
```
When `21,977,555` samples have elapsed in the song, it will loop back to `199,665` samples!

*Using Seconds*
```json
{
  "loopStartPointSec": "4.462",
  "loopEndPointSec": "49.803"
}
```
When `49.803` seconds have elapsed in the song, it will loop back to `4.462` seconds!

To find loop points in your song, you can use some free audio software like [Audacity](https://www.audacityteam.org/).

I would recommend using a DAW with more precise BPM / looping support, such as Reaper, Ableton Live, FL Studio, etc.

*If both samples and seconds are present in a file, samples take priority because they are more precise.*

## 🎵 Song Packs *(Advanced Usage)*

A Song Pack is a more efficient way to reuse songs across multiple stages / menus / victory themes!

Song Packs should be placed in the folder `BepInEx/CustomSongs/_Song Packs/`

Example Song Pack file structure:
```
↳ SongPack1
    ↳ _Music Bank
        ↳ song1.mp3
        ↳ different-song.ogg
        ↳ looped-song.mp3
        ↳ looped-song.json
        ↳ 4th-song.ogg
          ...
    ↳ Menus
        ↳ MainMenu.txt
        ↳ OnlineMenu.txt
          ...
    ↳ Stages
        ↳ Angry Beavers Dam.txt
        ↳ Bun Wrestling Ring.txt
          ...
    ↳ Victory Themes
        ↳ Aang.txt
        ↳ AngryBeavers.txt
          ...
```

### How to make a Song Pack
1. Navigate into the `BepInEx/CustomSongs/_Song Packs/` folder.
1. Duplicate the `_Template` folder, and rename it. Something like `Absolute Jammers`.
1. Place all of the songs you want to use in this song pack into the `Absolute Jammers/_Music Bank` folder.
1. Open the text file for the menu / stage / victory theme you want to add music to. For example, the main menu is `Absolute Jammers/Menus/MainMenu.txt`
1. In this text file, enter a list of song file names from the `_Music Bank` folder that you want to play on this menu / stage / victory theme. The order doesn't matter, but __***you must put each song on it's own line!***__

Example `MainMenu.txt`:
```
song1.mp3
looped-song.mp3
different-song.ogg
```
You can then reuse these songs in other menus / stages / victory themes!

Example `Jellyfish Fields.txt`
```
song1.mp3
looped-song.mp3
4th-song.ogg
```

That's it! When you launch the game it should load all song packs and any songs placed in the regular folders.

You can check the log file for errors, `BepInEx/LogOutput.log`. If you have issues loading songs, check your spelling! Files are case sensitive!

**Note:** *If you want the songs to loop at specific points, you will need to place corresponding json files in the `_Music Bank` folder as well. See [Loop Points.](#-loop-points-optional)*


## ❔ FAQ

### "My .mp3 didn't work!"
I've had multiple people unable to load certain mp3 files.

If you've downloaded a file from a youtube downloader, such as `ytmp3.cc`, your file might actually be an `.m4a` disguised as a `.mp3`

Unity cannot load these (it seems).

Use a different converter, or convert to `.ogg` instead! Thanks.

### "What happens if I have multiple songs in the same folder?"

The mod will randomly select a song to play each time that stage / menu / victory is loaded.

### "Can I include the default songs in the random selection?"

Yes. You must enable the option in the config file. The config file is generated the first time you run the mod and is in `BepInEx/config/megalon.nasb2_custom_music_mod.cfg`

## 🔧 Developing

Make sure you have all of the required files from the [installation section](https://github.com/megalon/nasb2-custom-music-mod#-installation)

But to clarify, you need:

- BepInEx 5
- Unstripped core Unity library (explained below)
- [SlimeModdingUtilities](https://github.com/DeadlyKitten/SlimeModdingUtilities)

Unstripped Core Library:

NASB2 ships without all of the dlls that typically ship with Unity games. For BepInEx to work, you need the "unstripped core Unity library". This can be found included in the BepInEx version in the NASB discord as `unstripped_corlib`.

If BepInEx doesn't load, check that your `doorstop_config.ini` points to the unstripped core library folder. Something like this:

```
dllSearchPathOverride=unstripped_corlib
```

### Setup

Clone the project, then create a file in the root of the project directory named:

`NickCustomMusicMod.csproj.user`

Here you need to set the `GameDir` property to match your install directory.

Example:
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <PropertyGroup>
    <GameDir>D:\SteamLibrary\steamapps\common\Nickelodeon All-Star Brawl 2</GameDir>
  </PropertyGroup>
</Project>
```

Now when you build the mod, it should resolve your references automatically, and the build event will copy the plugin into your `BepInEx\plugins` folder!
