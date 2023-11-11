using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using SMU.Reflection;
using NickCustomMusicMod;

namespace NASB2CustomMusicMod.Patches
{
    [HarmonyPatch(typeof(MusicManager), "Update")]
    internal class MusicManager_Update
    {
        /*
         * This builtin function is a mess.
         * I'm replacing it with a rewritten version that is functionally the same
         */
        static bool Prefix(MusicManager __instance)
        {
            if (__instance.MusicSources.Length <= 0 || (__instance.EndTime == 0f && !Plugin.playingCustomSong))
            {
                return false;
            }

            if (!Plugin.playingCustomSong && __instance.MusicSources[0].time >= __instance.EndTime)
            {
                foreach (AudioSource musicSource in __instance.MusicSources)
                {
                    if (musicSource.isActiveAndEnabled)
                    {
                        musicSource.Stop();
                    }
                }
            }

            if (__instance.GetField<MusicManager, bool>("paused")) return false;

            if (!__instance.MusicSources[0].isPlaying)
            {
                foreach (AudioSource musicSource in __instance.MusicSources)
                {
                    if (!Plugin.playingCustomSong)
                    {
                        musicSource.time = __instance.StartTime;
                    }

                    if (musicSource.isActiveAndEnabled)
                    {
                        Plugin.LogInfo("MusicManager_Update: Playing song!");
                        musicSource.Play();
                    }
                }
            }

            return false;
        }
    }
}
