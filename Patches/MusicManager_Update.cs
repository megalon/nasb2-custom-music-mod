﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using SMU.Reflection;
using NickCustomMusicMod;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace NASB2CustomMusicMod.Patches
{
    [HarmonyPatch(typeof(MusicManager), "Update")]
    internal class MusicManager_Update
    {
        private static AudioSource firstMusicSource;

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

            firstMusicSource = __instance.MusicSources[0];

            if (!Plugin.playingCustomSong && firstMusicSource.time >= __instance.EndTime)
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

            if (!firstMusicSource.isPlaying)
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

            // Loop
            if (Plugin.customTrack.LoopStartPointSamples >= 0 && Plugin.customTrack.LoopEndPointSamples > 0)
            {
                if (firstMusicSource.timeSamples >= Plugin.customTrack.LoopEndPointSamples)
                {
                    Plugin.LogWarning($"Looping! firstMusicSource.timeSamples: {firstMusicSource.timeSamples} | Loop points: {Plugin.customTrack.LoopStartPointSamples}, {Plugin.customTrack.LoopEndPointSamples}");
                    firstMusicSource.timeSamples -= (Plugin.customTrack.LoopEndPointSamples - Plugin.customTrack.LoopStartPointSamples);
                }
            } else if (Plugin.customTrack.LoopStartPointSec >= 0 && Plugin.customTrack.LoopEndPointSec > 0)
            {
                if (firstMusicSource.time >= Plugin.customTrack.LoopEndPointSec)
                {
                    Plugin.LogWarning($"Looping! firstMusicSource.time: {firstMusicSource.time} | Loop points: {Plugin.customTrack.LoopStartPointSec}, {Plugin.customTrack.LoopEndPointSec}");
                    firstMusicSource.time -= (Plugin.customTrack.LoopEndPointSec - Plugin.customTrack.LoopStartPointSec);
                }
            }

            return false;
        }
    }
}
