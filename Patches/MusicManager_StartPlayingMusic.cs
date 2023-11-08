using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine;
using NickCustomMusicMod.Utils;
using SMU.Reflection;
using NickCustomMusicMod;

namespace NASB2CustomMusicMod.Patches
{
    /*
     * This builtin function is a mess.
     * I'm replacing it with a rewritten version that is functionally the same
     */
    [HarmonyPatch(typeof(MusicManager), "StartPlayingMusic")]
    internal class MusicManager_StartPlayingMusic
    {
        static bool Prefix(MusicManager __instance)
        {
            // Local variables to make the code easier to read
            AudioSource[] musicSources = __instance.MusicSources;
            List<float> originalVolumeList = __instance.GetField<MusicManager, List<float>>("originalVolumeList");

            // Need to set private field "paused"
            __instance.SetField<MusicManager, bool>("paused", false);

            for (int i = 0; i < musicSources.Length; ++i)
            {
                AudioSource musicSource = musicSources[i];

                if (musicSource.isActiveAndEnabled && !musicSource.isPlaying)
                {
                    musicSource.mute = false;
                    musicSource.time = 0f;

                    musicSource.Play();

                    if (i < originalVolumeList.Count && musicSource.volume <= 0)
                    {
                        __instance.StartCoroutine(__instance.StartFade(musicSource, 0.5f, originalVolumeList[i]));
                    }

                    Plugin.LogInfo("Playing music from clip:" + musicSource.clip.name);
                }
            }

            return false;
        }
    }
}
