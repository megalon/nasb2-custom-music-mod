﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;
using NickCustomMusicMod.Management;
using BepInEx.Configuration;
using NASB2CustomMusicMod.Management;

namespace NickCustomMusicMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance;
        internal static string previousMusicID;
        internal static bool playingCustomSong;
        internal static CustomMusicTrack customTrack;
        internal ConfigEntry<bool> useDefaultSongs;
        internal ConfigEntry<bool> skipOnlineMenuMusicIfEmpty;
        private void Awake()
        {
            Logger.LogDebug($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");

            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;

            var config = this.Config;

            useDefaultSongs = Config.Bind<bool>("Options", "Use Default Songs", false);
            skipOnlineMenuMusicIfEmpty = Config.Bind<bool>("Options", "Skip OnlineMenu Music if Empty", true);

            config.SettingChanged += OnConfigSettingChanged;

            // Harmony patches
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            CustomMusicManager.Init();
        }

        static void OnConfigSettingChanged(object sender, EventArgs args)
        {
            LogDebug($"{PluginInfo.PLUGIN_NAME} OnConfigSettingChanged");
            Plugin.Instance?.Config?.Reload();
        }

        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
    }
}
