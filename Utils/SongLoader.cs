using NickCustomMusicMod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;

namespace NASB2CustomMusicMod.Utils
{
    internal class SongLoader
    {
        public static IEnumerator LoadCustomSong(MusicTrack track, AudioSource musicSource)
        {
            //Plugin.LogInfo("Loading song: " + track.Path);

            if (musicSource.clip.name.Equals(track.Path))
            {
                Plugin.LogInfo("This custom song is already loaded, skipping! " + track.Path);
                yield return null;
            }

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

            musicSource.clip.name = track.Path;
            musicSource.clip = DownloadHandlerAudioClip.GetContent(audioLoader);

            musicSource.time = 0f;

            Plugin.LogInfo("musicSource.time:" + musicSource.time);
            //musicSource.volume = 1;

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
