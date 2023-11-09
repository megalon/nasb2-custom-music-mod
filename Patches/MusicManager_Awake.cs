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

namespace NASB2CustomMusicMod.Patches
{
    [HarmonyPatch(typeof(MusicManager), "Awake")]
    internal class MusicManager_Awake
    {
        static bool Prefix(MusicManager __instance)
        {
            for (int i = 0; i < __instance.MusicSources.Length; ++i) {
                AudioSource musicSource = __instance.MusicSources[i];

                string id = musicSource.clip.name;
                Plugin.LogInfo($"MusicManager: {__instance.name} | MusicSources[{i}]: {id}");

                MusicTrack customTrack = CustomMusicManager.GetRandomCustomSong(id);

                if (customTrack == null) continue;

                __instance.StartCoroutine(LoadCustomSong(customTrack, musicSource));
            }

            return true;
        }

        public static IEnumerator LoadCustomSong(MusicTrack track, AudioSource musicSource)
        {
            Plugin.LogInfo("Loading song: " + track.Path);

            UnityEngine.AudioType audioType = UnityEngine.AudioType.UNKNOWN;

            switch (Path.GetExtension(track.Path).ToLower())
            {
                case ".wav":
                    audioType = UnityEngine.AudioType.WAV;
                    break;
                case ".mp3":
                    audioType = UnityEngine.AudioType.MPEG;
                    break;
                case ".ogg":
                    audioType = UnityEngine.AudioType.OGGVORBIS;
                    break;
                default:
                    yield break;
            }

            UnityWebRequest audioLoader = UnityWebRequestMultimedia.GetAudioClip(track.Path, audioType);
            // this stops the lag!
            (audioLoader.downloadHandler as DownloadHandlerAudioClip).streamAudio = true;
            yield return audioLoader.SendWebRequest();
            if (audioLoader.error != null)
            {
                Plugin.LogError(audioLoader.error);
                yield break;
            }

            musicSource.clip = DownloadHandlerAudioClip.GetContent(audioLoader);

            // !!!!!! TODO !!!!!!
            // !!!!!! TODO !!!!!!
            // !!!!!! TODO !!!!!!
            // !!!!!! TODO !!!!!!
            // Handle custom music data here
            // HandleCustomMusicData(entry, music);

            yield break;
        }
    }
}
