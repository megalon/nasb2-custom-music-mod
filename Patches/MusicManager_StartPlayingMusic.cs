using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine;
using NickCustomMusicMod.Utils;
using SMU.Reflection;
using NickCustomMusicMod;
using NASB2CustomMusicMod.Utils;
using NickCustomMusicMod.Management;
using UnityEngine.UI;

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
            PlayMusic(__instance, true);
            return false;
        }

        public static void PlayMusic(MusicManager __instance, bool checkIfAlreadyPlaying)
        {
            // Local variables to make the code easier to read
            AudioSource[] musicSources = __instance.MusicSources;
            List<float> originalVolumeList = __instance.GetField<MusicManager, List<float>>("originalVolumeList");

            // Need to set private field "paused"
            __instance.SetField<MusicManager, bool>("paused", false);

            for (int i = 0; i < musicSources.Length; ++i)
            {
                AudioSource musicSource = musicSources[i];

                // ---- Intercept and pick custom song ----

                string id = musicSource.clip.name;
                Plugin.LogInfo($"MusicManager: {__instance.name} | MusicSources[{i}]: {id}");

                //if (CheckToSkipOnlineMenuMusic(ref id)) return false;

                MusicTrack customTrack = CustomMusicManager.GetRandomCustomSong(id);

                if (customTrack == null)
                {
                    Plugin.playingCustomSong = false;
                } else
                {
                    Plugin.playingCustomSong = true;
                    __instance.StartCoroutine(SongLoader.LoadCustomSong(customTrack, musicSource));
                }

                // ----

                if (!musicSource.isActiveAndEnabled)
                {
                    continue;
                }

                // checkIsPlaying is here because
                // - MusicManager_StartPlayingMusic checks if the music is already playing before trying to play it
                // - MusicManager_StartPlaying doesn't check
                if (checkIfAlreadyPlaying && musicSource.isPlaying)
                {
                    continue;
                }

                musicSource.mute = false;
                musicSource.time = 0f;

                musicSource.Play();

                if (i < originalVolumeList.Count)
                {
                    __instance.StartCoroutine(__instance.StartFade(musicSource, 0.5f, originalVolumeList[i]));
                }

                Plugin.LogInfo("Playing music from clip:" + musicSource.clip.name);
            }
        }
    }
}
