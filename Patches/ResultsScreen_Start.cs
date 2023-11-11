using HarmonyLib;
using NickCustomMusicMod;
using NickCustomMusicMod.Utils;
using Quantum;
using SMU.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NASB2CustomMusicMod.Patches
{
    [HarmonyPatch(typeof(ResultsScreen), "Start")]
    internal class ResultsScreen_Start
    {

        static bool Prefix(ResultsScreen __instance)
        {
            try
            {
                MatchManager matchManager = GameManager.Instance.MatchManager;

                // Get the character index of the character that won.
                // PlayerResults is in order with the first position being the winner
                // CharacterIndex is the position in the character select screen from 0 - 3
                int winningCharIndex = matchManager.PlayerResults[0].CharacterIndex;

                // Get the character manager of the winner
                CharacterManager winningCharacterManager = matchManager.CurrentCharacterManagers[winningCharIndex];

                // Get the name of the winning character
                string winnerName = Enum.GetName(typeof(CharacterCodename), winningCharacterManager.Codename);

                Plugin.LogInfo($"The winner is: {winnerName}");

                // This feels hacky, but just set the clip name to the dictionary key
                // so we can reuse the patched MusicManager.StartPlayingMusic when this plays
                __instance.ResultsMusic.MusicSources[0].clip.name = $"{Consts.victoryThemesFolderName}_{winnerName}";;
            } catch (Exception ex)
            {
                Plugin.LogError("Caught exception in ResultsScreen_Start!");
                Plugin.LogError(ex.Message);
            }

            return true;
        }
    }
}
