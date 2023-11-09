using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NASB2CustomMusicMod.Patches
{
    /*
     * This builtin function is a mess.
     * I'm replacing it with a rewritten version that is functionally the same
     */
    [HarmonyPatch(typeof(MusicManager), "StartPlaying")]
    internal class MusicManager_StartPlaying
    {
        static bool Prefix(MusicManager __instance)
        {
            foreach (AudioSource environmentSource in __instance.EnviromentSources)
            {
                environmentSource.time = 0f;
                environmentSource.Play();
            }

            __instance.StartPlayingMusic();

            return false;
        }
    }
}
