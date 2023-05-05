using MSCLoader;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AchievementCore
{
    public static class AchievementCoreStartup
    {
        private static bool started = false, saving = false;
        private static GameObject canvas, ui;
        private static AssetBundle ab;
        private static AchievementHandler AchievementHandler;
        private static object[] achievements;
        private static readonly string saveFile = Application.persistentDataPath + "\\Achievements.dat";
        //static Mod.Setup Mod_OnSave() => Mod.Setup.OnSave;
        
        public static void SaveAchievements()
        {
            if (!saving)
            {
                saving = true;
                SaveBytes.save(saveFile, AchievementIDHolder.unlocked_achievements);
                SaveBytes.save(saveFile, AchievementIDHolder.locked_achievements);
            }
        }
        public static void Startup()
        {
            if (!started)
            {
                started = true;
                GameObject g = new GameObject("AchievementCore");
                AchievementHandler = g.AddComponent<AchievementHandler>();
                AchievementIDHolder.AchievementHandler = AchievementHandler;
                canvas = ModUI.CreateCanvas("AchievementCoreCanvas", true);
                ab = LoadAssets.LoadBundle("AchievementCore.Assets.achcore.unity3d");
                ui = GameObject.Instantiate(ab.LoadAsset<GameObject>("AchievementCoreUI.prefab"));
                ui.transform.SetParent(canvas.transform, false);
                ui.transform.localPosition = new Vector3(-521f, -564f, 0f);
                ui.transform.localScale = new Vector3(1.1f, 1.1f, 0.8f);
                ui.transform.Find("MENU_PARENT/Button/Text").GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                AchievementHandler.ui = ui;
                LoadAchievements();
                GameObject.DontDestroyOnLoad(g);
                GameObject.DontDestroyOnLoad(ui);
                ModConsole.Log("<color=yellow>Achievement Core loaded succesfully!</color>");
                ab.Unload(false);
            }
            else
            {
                ui.transform.localPosition = new Vector3(-521f, -564f, 0f);
                ui.transform.localScale = new Vector3(1.1f, 1.1f, 0.8f);
                ui.transform.Find("MENU_PARENT/Button/Text").GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                started = false;
            }
        }
        private static void LoadAchievements()
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
    }
}
