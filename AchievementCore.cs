using MSCLoader;
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

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
            SetupFunction(Setup.OnMenuLoad, Mod_OnMenuLoad);
        }
        public static AchievementHandler AchievementHandler;
        private GameObject canvas;
        private GameObject ui;
        public static object[] achievements;
        private static readonly string saveFile = Application.persistentDataPath + "\\Achievements.dat";
        private void Mod_OnMenuLoad()
        {
            GameObject g = new GameObject("AchievementCore");
            AchievementHandler = g.AddComponent<AchievementHandler>();
            canvas = ModUI.CreateCanvas("AchievementCoreCanvas", false);
            AssetBundle ab = LoadAssets.LoadBundle("AchievementCore.Assets.achcore.unity3d");
            ui = GameObject.Instantiate(ab.LoadAsset<GameObject>("AchievementCoreUI.prefab"));
            LoadAchievements();
            GameObject.DontDestroyOnLoad(g);
            GameObject.DontDestroyOnLoad(ui);
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
        }
        private void Mod_OnSave()
        {
            SaveBytes.save(saveFile, AchievementIDHolder.unlocked_achievements);
            SaveBytes.save(saveFile, AchievementIDHolder.locked_achievements);
        }
    }
}
