## ⚠️ This project is a work in progress ⚠️

# NASB2 Custom Music Mod

This mod allows you to add custom songs to the stages, menus, and victory themes in the game!
_____

## 🚀 Installation

Requirements:

- BepInEx 5
- Unstripped core Unity library (explained below)
- [SlimeModdingUtilities](https://github.com/DeadlyKitten/SlimeModdingUtilities)

Unstripped Core Library:

NASB2 ships without all of the dlls that typically ship with Unity games. For BepInEx to work, you need the "unstripped core Unity library". This can be found included in the BepInEx version in the NASB discord as `unstripped_corlib`.

If BepInEx doesn't load, check that your `doorstop_config.ini` points to the unstripped core library folder. Something like this:

```
dllSearchPathOverride=unstripped_corlib
```

## ℹ Usage

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
