using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using NickCustomMusicMod.Utils;
using UnityEngine.Localization;
using NASB2CustomMusicMod.Management;

namespace NickCustomMusicMod.Management
{
    internal class CustomMusicManager
	{
		public static string rootCustomSongsPath;

		public static void Init()
        {
			songDictionaries = new Dictionary<string, Dictionary<string, CustomMusicTrack>>();

			rootCustomSongsPath = Path.Combine(Paths.BepInExRootPath, "CustomSongs");

			// Create the folder if it doesn't exist
			Directory.CreateDirectory(rootCustomSongsPath);

			Plugin.LogInfo("Generating folders if they don't exist...");

			// Create the "_Song Packs" folder, "Template" folder, and "_Music Bank" in one go
            Directory.CreateDirectory(Path.Combine(rootCustomSongsPath, Consts.songPacksFolderName, Consts.templateSongPackName, Consts.musicBankFolderName));

			string templateSongPackPath = Path.Combine(rootCustomSongsPath, Consts.songPacksFolderName, Consts.templateSongPackName);

            foreach (string menuID in Consts.MenuIDs.Keys)
            {
                Directory.CreateDirectory(Path.Combine(rootCustomSongsPath, Consts.menusFolderName, menuID));
				FileHandlingUtils.TryToCreateFile(Path.Combine(templateSongPackPath, Consts.menusFolderName), $"{menuID}.txt");
            }

            foreach (string stageName in Consts.StageIDs.Keys)
            {
                Directory.CreateDirectory(Path.Combine(rootCustomSongsPath, Consts.stagesFolderName, stageName));
                FileHandlingUtils.TryToCreateFile(Path.Combine(templateSongPackPath, Consts.stagesFolderName), $"{stageName}.txt");
            }

            foreach (string characterName in Consts.CharacterFolderNames)
            {
                Directory.CreateDirectory(Path.Combine(rootCustomSongsPath, Consts.victoryThemesFolderName, characterName));
                FileHandlingUtils.TryToCreateFile(Path.Combine(templateSongPackPath, Consts.victoryThemesFolderName), $"{characterName}.txt");
            }

			Plugin.LogInfo("Finished generating folders!");

            Plugin.LogInfo("Loading songs from subfolders...");
            LoadFromSubDirectories(Consts.stagesFolderName);
            LoadFromSubDirectories(Consts.menusFolderName);
            LoadFromSubDirectories(Consts.victoryThemesFolderName);
            Plugin.LogInfo("Finished loading songs from subfolders!");

            Plugin.LogInfo("Loading song packs...");
            LoadFromSongPacks();
            Plugin.LogInfo("Finished loading song packs!");
        }

		public static void LoadFromSubDirectories(string parentFolderName)
		{
			if (!Directory.Exists(Path.Combine(rootCustomSongsPath, parentFolderName))) return;

			var subDirectories = Directory.GetDirectories(Path.Combine(rootCustomSongsPath, parentFolderName));

			Plugin.LogInfo($"Looping through sub directories in \"{parentFolderName}\"");

			// Copy files from old folders to new
			foreach (string directory in subDirectories)
			{
				FileHandlingUtils.RenameFolderOrMoveFiles(directory);
			}

			// Since we may have deleted folders in the previous step, get the list again
			subDirectories = Directory.GetDirectories(Path.Combine(rootCustomSongsPath, parentFolderName));
			foreach (string directory in subDirectories)
			{
				var folderName = new DirectoryInfo(directory).Name;

				LoadSongsFromFolder(parentFolderName, folderName);
			}
		}

		public static void LoadSongsFromFolder(string parentFolderName, string folderName)
		{
			Plugin.LogInfo($"LoadSongsFromFolder \"{folderName}\"");
			
			string path = Path.Combine(rootCustomSongsPath, parentFolderName, folderName);

			Dictionary<string, CustomMusicTrack> MusicTrackDict = new Dictionary<string, CustomMusicTrack>();

			foreach (string songPath in from x in Directory.GetFiles(path)
				where x.ToLower().EndsWith(".ogg") || x.ToLower().EndsWith(".wav") || x.ToLower().EndsWith(".mp3")
				select x)
				{
				addMusicTrackToDict(MusicTrackDict, songPath);
			}

			string dictKey = constructDictionaryKey(parentFolderName, folderName);

            Plugin.LogDebug($"Dictionarykey: {dictKey}");

			songDictionaries.Add(dictKey, MusicTrackDict);
		}

		public static void LoadFromSongPacks() {
			if (!Directory.Exists(Path.Combine(rootCustomSongsPath, Consts.songPacksFolderName))) return;

			var subDirectories = Directory.GetDirectories(Path.Combine(rootCustomSongsPath, Consts.songPacksFolderName));

			foreach (string directory in subDirectories) {
				var packName = new DirectoryInfo(directory).Name;

				// Don't load template pack
				if (packName.Equals(Consts.templateSongPackName)) continue;

				LoadPack(packName);
			}
		}

		public static void LoadPack(string packName) {
			Plugin.LogInfo($"Loading SongPack:\"{packName}\"");
			var subDirectories = Directory.GetDirectories(Path.Combine(rootCustomSongsPath, Consts.songPacksFolderName, packName));

			foreach (string directory in subDirectories) {
				var folderName = new DirectoryInfo(directory).Name;

				if (folderName.Equals(Consts.musicBankFolderName)) continue;

				LoadFromPackSubdirectory(packName, folderName);
			}
		}

		public static void LoadFromPackSubdirectory(string packName, string folderName) {
			var folderPath = Path.Combine(rootCustomSongsPath, Consts.songPacksFolderName, packName, folderName);

			foreach (string textFileName in from x in Directory.GetFiles(folderPath) where x.ToLower().EndsWith(".txt") select x)
			{
				Plugin.LogInfo($"LoadFromPackSubdirectory {packName}\\{folderName}\\{textFileName}");

				string musicBankPath = Path.Combine(rootCustomSongsPath, Consts.songPacksFolderName, packName, Consts.musicBankFolderName);
				string listPath = Path.Combine(folderPath, textFileName);

				Dictionary<string, CustomMusicTrack> MusicTrackDict = songDictionaries[constructDictionaryKey(folderName, Path.GetFileNameWithoutExtension(textFileName))];

				foreach (string textLine in File.ReadLines(listPath))
				{
					if (textLine.IsNullOrWhiteSpace()) continue;

					addMusicTrackToDict(MusicTrackDict, Path.Combine(musicBankPath, textLine.Trim()));
				}
			}
		}

		public static CustomMusicTrack GetRandomCustomSong(string id)
		{
            // Get a random song for this stage / menu
            if (songDictionaries.ContainsKey(id))
            {
                Dictionary<string, CustomMusicTrack> musicDict = songDictionaries[id];
                int numCustomSongs = musicDict.Keys.Count;

                if (numCustomSongs > 0)
                {
                    int randInt;

                    // Include default songs if value is enabled
                    // and this is not a victory theme
                    if (Plugin.Instance.useDefaultSongs.Value && !id.StartsWith(Consts.victoryThemesFolderName))
                    {
                        randInt = UnityEngine.Random.Range(0, numCustomSongs + 1);
                    }
                    else
                    {
                        randInt = UnityEngine.Random.Range(0, numCustomSongs);
                    }

                    if (randInt >= numCustomSongs)
                    {
                        Plugin.LogInfo("Randomly selected default music instead of custom songs!");

                        return null;
                    }
                    else
                    {
                        string randomSong = musicDict.Keys.ToArray<string>()[randInt];
                        CustomMusicTrack musicEntry = musicDict[randomSong];

                        // Intercept the ID and use our custom one
                        Plugin.LogDebug($"Intercepting GetMusic id: {id} and changing to {musicEntry.Id}");

                        return musicEntry;
                    }
                }
                else
                {
                    Plugin.LogInfo($"No songs found for {id}! Using default music instead.");
                }
            } else
            {
                Plugin.LogInfo($"songDictionaries did not contain key: {id}");
            }

            return null;
        }

		/// <summary>
		/// This is a holdover from the odd way that NASB1 handled dict keys
		/// It still works for now!
		/// </summary>
		/// <param name="musicType">Likely the parent folder name. "Menu", "Stage", "Victory Themes", etc. </param>
		/// <param name="name">Name of this Menu / Stage / Character</param>
		/// <returns>Dictionary key for "songDictionaries" dictionary</returns>
		private static string constructDictionaryKey(string musicType, string name)
		{
			if (musicType == Consts.stagesFolderName || musicType == Consts.menusFolderName)
				return FileHandlingUtils.TranslateFolderNameToID(name);
			else
				return $"{musicType}_{FileHandlingUtils.TranslateFolderNameToID(name)}";
		}

		private static bool addMusicTrackToDict(Dictionary<string, CustomMusicTrack> MusicTrackDict, string songPath)
        {
			if (File.Exists(songPath))
			{
				Plugin.LogInfo($"Found custom song: \"{songPath}\"");

				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(songPath);

                // Fix this to actually have the correct text in it!
                LocalizedString localizedString = new LocalizedString();

                CustomMusicTrack music = new CustomMusicTrack
                {
					Id = "CUSTOM_" + fileNameWithoutExtension,
					TrackName = localizedString,
					Path = songPath,
					//AudioBus;
					//Volume;
					//StartTime;
					//EndTime;
					//StageAssociated;
				};

				if (MusicTrackDict.ContainsKey(music.Id))
				{
					Plugin.LogWarning($"Ignoring \"{songPath}\" because duplicate file was detected! Do you have two different files with the same name for this stage / menu / victory theme?");
				} else
                {
					MusicTrackDict.Add(music.Id, music);
					return true;
				}
			} else
            {
				Plugin.LogWarning($"addMusicTrackToDict failed because file doesn't exist! \"{songPath}\"");
            }
			return false;
		}

		internal static Dictionary<string, Dictionary<string, CustomMusicTrack>> songDictionaries;
	}

}
