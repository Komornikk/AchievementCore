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
        public override string Description => ""; //Short description of your mod

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
        }
        public static AchievementHandler AchievementHandler;
        private void Mod_OnLoad()
        {
            GameObject g = new GameObject("AchievementCore");
            AchievementHandler = g.AddComponent<AchievementHandler>();
            GameObject testOBJ = new GameObject("testCube").AddComponent<BoxCollider>().gameObject;
            testOBJ.GetComponent<BoxCollider>().isTrigger = true;
            testOBJ.AddComponent<TestObject>();
            AchievementIDHolder.Achievement_IDs.Add("test_id");
        }
        public static object[] achievements;
        private static readonly string saveFile = Application.persistentDataPath + "\\Achievements.dat";
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
