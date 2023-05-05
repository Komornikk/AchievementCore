﻿using MSCLoader;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
namespace AchievementCore
{
    public class AchievementCore : Mod
    {
        public override string ID => "AchievementCore"; //Your mod ID (unique)
        public override string Name => "AchievementCore"; //You mod name
        public override string Author => "komornik"; //Your Username
        public override string Version => "1.0"; //Version
        public override string Description => "Achievements system for My Summer Car mods!"; //Short description of your mod

        public bool loaded = false;
        public AssetBundle ab;
        public static AchievementHandler AchievementHandler;
        public GameObject canvas;
        private GameObject ui;
        public static object[] achievements;
        private static readonly string saveFile = Application.persistentDataPath + "\\Achievements.dat";
        public void Mod_OnMenuLoad()
        {
            /*
            if (!loaded)
            {
                GameObject g = new GameObject("AchievementCore");
                AchievementHandler = g.AddComponent<AchievementHandler>();
                canvas = ModUI.CreateCanvas("AchievementCoreCanvas", true);
                ab = LoadAssets.LoadBundle("AchievementCore.Assets.achcore.unity3d");
                ui = GameObject.Instantiate(ab.LoadAsset<GameObject>("AchievementCoreUI.prefab"));
                ui.transform.SetParent(canvas.transform);
                ui.transform.position = new Vector3(-334f, -205f, 0f);
                ui.transform.localScale = new Vector3(1.1f, 1.1f, 0.8f);
                LoadAchievements();
                GameObject.DontDestroyOnLoad(g);
                GameObject.DontDestroyOnLoad(ui);
                loaded = true;
            }
            ab.Unload(false);
            */
        }
        private void Mod_OnLoad()
        {   

        }
        void LoadAchievements()
        {
            if (File.Exists(saveFile))
            {
                achievements = SaveBytes.load(saveFile, null);
                if (achievements != null)
                {
                    AchievementIDHolder.unlocked_achievements = (List<string>)achievements[0];
                    AchievementIDHolder.locked_achievements = (List<string>)achievements[1];
                }
                else
                {
                    AchievementIDHolder.locked_achievements = AchievementIDHolder.Achievement_IDs;
                }
            }
            else
            {
                AchievementIDHolder.locked_achievements = AchievementIDHolder.Achievement_IDs;
                AchievementIDHolder.unlocked_achievements = new List<string>();
            }
        }
        private void Mod_OnSave()
        {
            //AchievementCoreStartup.SaveAchievements();
        }
    }
}
