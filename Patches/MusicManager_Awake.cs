using HarmonyLib;
using NickCustomMusicMod;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine;
using SMU.Reflection;
using NickCustomMusicMod.Management;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using NASB2CustomMusicMod.Utils;

namespace NASB2CustomMusicMod.Patches
{
    [HarmonyPatch(typeof(MusicManager), "Awake")]
    internal class MusicManager_Awake
    {
        static bool Prefix(MusicManager __instance)
        {
            return true;
        }

        private static bool CheckToSkipOnlineMenuMusic(ref string id)
        {
            // Skip the online menu music if the setting is enabeld in the config
            // and there are no custom songs for the mx_onlinemenu
            if (Plugin.Instance.skipOnlineMenuMusicIfEmpty.Value && id.Equals("mx_onlinemenu"))
            {
                if (CustomMusicManager.songDictionaries.TryGetValue(id, out var dict) && dict.Keys.Count == 0)
                {
                    if (!Plugin.previousMusicID.Equals("mx_maintheme"))
                    {
                        // Switch to main menu theme if we are coming out of a match in online mode
                        Plugin.LogInfo($"No songs found for \"OnlineMenu\", and \"Skip OnlineMenu Music if Empty\" is {Plugin.Instance.skipOnlineMenuMusicIfEmpty.Value}! Changing music ID to {id}.");
                        id = "mx_maintheme";
                    }
                    else
                    {
                        Plugin.LogInfo($"No songs found for \"OnlineMenu\", and \"Skip OnlineMenu Music if Empty\" is {Plugin.Instance.skipOnlineMenuMusicIfEmpty.Value}! Current song will keep playing.");
                        return true;
                    }
                }
            }

            Plugin.previousMusicID = id;

            return false;
        }
    }
}
