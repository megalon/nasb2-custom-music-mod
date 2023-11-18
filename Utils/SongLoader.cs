using NickCustomMusicMod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json;
using NickCustomMusicMod.Data;
using NASB2CustomMusicMod.Management;
using System.Reflection;

namespace NASB2CustomMusicMod.Utils
{
    internal class SongLoader
    {
        public static IEnumerator LoadCustomSong(CustomMusicTrack track, AudioSource musicSource)
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


            Plugin.LogInfo("Loading song " + track.Path);

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

            Plugin.LogInfo("Finished loading song!");

            musicSource.time = 0f;

            //musicSource.volume = 1;

            // Handle custom music data here
            Plugin.LogInfo("Calling HandleCustomMusicData...");
            HandleCustomMusicData(track, musicSource);

            yield break;
        }

        private static void HandleCustomMusicData(CustomMusicTrack entry, AudioSource musicSource)
        {
            Plugin.LogInfo("Loading custom music data for:" + entry.Path);

            entry.LoopStartPointSec = -1;
            entry.LoopEndPointSec = -1;
            entry.LoopPoints = null;

            // Get loop points from json file here
            string jsonPath = Path.Combine(Path.GetDirectoryName(entry.Path), Path.GetFileNameWithoutExtension(entry.Path) + ".json");
            if (File.Exists(jsonPath))
            {
                try
                {
                    string jsonFile = File.ReadAllText(jsonPath);

                    Plugin.LogDebug(jsonFile);

                    var customMusicData = JsonConvert.DeserializeObject<CustomMusicData>(jsonFile, null);

                    customMusicData.loopStartPointSec = Mathf.Clamp(customMusicData.loopStartPointSec, 0, musicSource.clip.length);
                    customMusicData.loopEndPointSec = Mathf.Clamp(customMusicData.loopEndPointSec, 0, musicSource.clip.length);

                    if (customMusicData.loopPoints == null) {
                        // This is supposed to default when it's null on deserialization,
                        // but that doesn't seem to work so we have to do this
                        customMusicData.loopPoints = new CustomLoopPoints();
                    } else
                    {
                        customMusicData.loopPoints = new CustomLoopPoints(
                            Mathf.Clamp(customMusicData.loopPoints.Start, 0, musicSource.clip.samples),
                            Mathf.Clamp(customMusicData.loopPoints.End, 0, musicSource.clip.samples)
                        );
                    }


                    Plugin.LogInfo($"customMusicData: {customMusicData.loopStartPointSec}, {customMusicData.loopEndPointSec}");

                    LogMessage_DataIsZero(customMusicData.loopStartPointSec, "customMusicData.loopStartPointSec", jsonPath);
                    LogMessage_DataIsZero(customMusicData.loopEndPointSec, "customMusicData.loopEndPointSec", jsonPath);
                    LogMessage_DataIsZero(customMusicData.loopPoints.Start, "customMusicData.loopPointsSamples.StartPoint", jsonPath);
                    LogMessage_DataIsZero(customMusicData.loopPoints.End, "customMusicData.loopPointsSamples.EndPoint", jsonPath);

                    if (customMusicData.loopEndPointSec == 0 && customMusicData.loopStartPointSec > 0)
                    {
                        Plugin.LogWarning($"\"loopStartPointSec\" is greater than 0, but \"loopEndPointSec\" is 0! Setting \"loopEndPointSec\" to length of song for \"{jsonPath}\"");
                        customMusicData.loopEndPointSec = musicSource.clip.length;
                    }

                    if (customMusicData.loopPoints.End == 0 && customMusicData.loopPoints.Start > 0)
                    {
                        Plugin.LogWarning($"\"loopPointsSamples\" start is greater than 0, but \"loopPointsSamples\" end is 0! Setting end to length of song for \"{jsonPath}\"");
                        customMusicData.loopPoints = new CustomLoopPoints(customMusicData.loopPoints.Start, musicSource.clip.samples);
                    }

                    if (customMusicData.loopEndPointSec > 0 && customMusicData.loopStartPointSec > 0 && customMusicData.loopStartPointSec == customMusicData.loopEndPointSec)
                    {
                        Plugin.LogWarning($"\"loopStartPointSec\" and \"loopEndPointSec\" are the same value for \"{jsonPath}\"! Did you mean to do that?");
                    }

                    if (customMusicData.loopPoints.End > 0 && customMusicData.loopPoints.Start > 0 && customMusicData.loopPoints.Start == customMusicData.loopPoints.End)
                    {
                        Plugin.LogWarning($"\"loopPointsSamples\" start and end points are the same value for \"{jsonPath}\"! Did you mean to do that?");
                    }

                    entry.LoopStartPointSec = customMusicData.loopStartPointSec;
                    entry.LoopEndPointSec = customMusicData.loopEndPointSec;
                    entry.LoopPoints = customMusicData.loopPoints;
                }
                catch (Exception e)
                {
                    Plugin.LogError($"Error reading json data for {jsonPath}");
                    Plugin.LogError(e.Message);
                }
            }
            else
            {
                Plugin.LogInfo($"No json file found for {Path.GetFileName(entry.Path)}");
            }
        }

        private static void LogMessage_DataIsZero(int val, string fieldName, string jsonPath)
        {
            LogMessage_DataIsZero((float)val, fieldName, jsonPath);
        }

        private static void LogMessage_DataIsZero(float val, string fieldName, string jsonPath)
        {
            if (val != 0) return;

            Plugin.LogDebug($"\"{fieldName}\" is 0 for json file \"{jsonPath}\"! It might not be in the file, or is misspelled!");
        }
    }
}
