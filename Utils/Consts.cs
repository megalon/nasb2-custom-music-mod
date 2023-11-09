﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NickCustomMusicMod.Utils
{
    // This list dictates which folders are auto generated
    // It also translates the folder names to their in game ID
    public static class Consts
    {
        public static readonly string stagesFolderName = "Stages";
        public static readonly string menusFolderName = "Menus";
        public static readonly string victoryThemesFolderName = "Victory Themes";
		public static readonly string songPacksFolderName = "_Song Packs";
		public static readonly string musicBankFolderName = "_Music Bank";

        public static readonly Dictionary<string, string> StageIDs = new Dictionary<string,string> {
            { "Jellyfish Fields", "mx_jellyfishfields" } ,
            { "Flying Dutchmans Ship", "mx_theflyingdutchmansship" },
            { "Chum Bucket", "mx_thechumbucket"},
            { "Bun Wrestling Ring", "mx_bunwrestlingring"},
            { "Harmonic Convergence", "mx_harmonicconvergence" },
            { "Western Air Temple", "mx_westernairtemple" },
            { "Fire Masters Meeting", "mx_firemastersmeeting" },
            { "Irken Armada Invasion", "mx_irkenarmadainvasion" },
            { "Sewers Slam", "mx_sewersslam" },
            { "Rooftop Rumble", "mx_rooftoprumble" },
            { "Technodrome Takedown", "mx_technodrometakedown" },
            { "Hardcore Chores", "mx_hardcorechores" },
            { "Food Dream", "mx_fooddream " },
            { "Jimmys Lab", "mx_jimmyslab" },
            { "Messy Kitchen", "mx_messykitchen" },
            { "Wild Savannah", "mx_wildsavannah" },
            { "Loud Castle", "mx_loudcastle" },
            { "Royal Woods Cemetary", "mx_royalwoodscemetery" },
            { "Miracle City Volcano", "mx_miraclecityvolcano" },
            { "Angry Beavers Dam", "mx_angrybeaversdam" },
            { "Pariahs Keep", "mx_pariahskeep" },
            { "Clockworks Lair", "mx_clockworkslair" },
            { "Reptars Ruins", "mx_reptarsruins" },
            { "City Aquarium", "mx_cityaquarium" },
            { "Tremorton Joyride", "mx_tremortonjoyride" },
            { "Training", "mx_trainingstage" }
        };

        public static readonly Dictionary<string, string> NASB1StageIDs = new Dictionary<string, string> {
            { "CatDogs House", "House" },
            { "Ghost Zone", "Zone" },
            { "Glove World", "CarnivalLofi" },
            { "Harmonic Convergence", "SpiritWorld" },
            { "Irken Armada Invasion", "Armada" },
            { "Jellyfish Fields", "SlideHouse" },
            { "Omashu", "Omashu" },
            { "Powdered Toast Trouble", "DuoKitchen" },
            { "Rooftop Rumble", "Rooftop" },
            { "Royal Woods Cemetery", "YMC" },
            { "Sewers Slam", "SewersCombined" },
            { "Showdown at Teeter Totter Gulch", "Playground" },
            { "Slime Time", "DoubleDare" },
            { "Space Madness", "DuoMadness" },
            { "Sweet Dreams", "GarfieldDream" },
            { "Technodrome Takedown", "Drome" },
            { "The Dump", "Trash" },
            { "The Flying Dutchmans Ship", "Shanty" },
            { "The Loud House", "Loud" },
            { "Traffic Jam", "Stomp" },
            { "Tremorton Joyride", "NeoFrontHouse"},
            { "Western Air Temple", "Temple2" },
            { "Wild Waterfall", "Waterfall" }
        };

        public static readonly Dictionary<string, string> MenuIDs = new Dictionary<string, string> {
            { "MainMenu", "mx_maintheme" },
            { "OnlineMenu", "mx_onlinemenu" },
            { "Versus", "mx_vs_loading" },
        };

        public static readonly string[] NASB1MenuIDs = {
            "MainMenu",
            "Versus",
            "OnlineMenu",
            "ArcadeMap",
            "Victory",
            "LoseV1",
            "LoseV2"
        };

        public static readonly Dictionary<string, string> CharacterIDs = new Dictionary<string, string> {
            { "SpongeBob", "char_apple" },
            { "Patrick", "char_star" },
            { "Sandy", "char_diver" },
            { "Aang", "char_kite" },
            { "Korra", "char_athlete" },
            { "Toph", "char_clay" },
            { "Lincoln Loud", "char_rascal" },
            { "Lucy Loud", "char_goth" },
            { "Leonardo", "char_moon" },
            { "Michelangelo", "char_pizza" },
            { "April", "char_reporter" },
            { "Ren and Stimpy", "char_duo" },
            { "Powdered ToastMan", "char_hero" },
            { "Zim", "char_alien" },
            { "CatDog", "char_chimera" },
            { "Reptar", "char_mascot" },
            { "Nigel Thornberry", "char_narrator" },
            { "Helga", "char_rival" },
            { "Danny Phantom", "char_plasma" },
            { "Oblina", "char_snake" },
            { "Garfield", "char_orb" },
            { "Shredder", "char_cheese" },
            { "Jenny", "char_neo" }
        };
    }
}
