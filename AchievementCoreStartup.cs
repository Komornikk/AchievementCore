using MSCLoader;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AchievementCore
{
    public static class AchievementCoreStartup
    {
        private static bool started = false, saving = false, onload = false;
        private static GameObject canvas, ui, achbox;
        private static AssetBundle ab;
        private static AchievementHandler AchievementHandler;
        static Sprite default_icon;
        private static object[] achievements;
        private static readonly string saveFile = Application.persistentDataPath + "\\Achievements.dat";
        
        public static void SaveAchievements()
        {
            if (!saving)
            {
                saving = true;
                SaveBytes.save(saveFile, AchievementIDHolder.unlocked_achievements);
                SaveBytes.save(saveFile, AchievementIDHolder.locked_achievements);
            }
        }
        public static void OnLoad()
        {
            if (!onload)
            {
                onload = true;
                
            }
        }
        public static void Startup()
        {
            if (!started)
            {
                started = true;
                onload = false;
                GameObject g = new GameObject("AchievementCore");
                AchievementHandler = g.AddComponent<AchievementHandler>();
                AchievementIDHolder.AchievementHandler = AchievementHandler;
                canvas = ModUI.CreateCanvas("AchievementCoreUI", true);
                ab = LoadAssets.LoadBundle("AchievementCore.Assets.achcore.unity3d");
                achbox = GameObject.Instantiate(ab.LoadAsset<GameObject>("achbox.prefab"));
                achbox.transform.SetParent(canvas.transform);
                achbox.transform.localScale = Vector3.one;
                achbox.transform.localPosition = new Vector3(-1016f, 311f, 0f);
                achbox.name = "AchievementBoxHolder";
                AchievementHandler.achievement_box = achbox.transform.GetChild(0).gameObject;
                default_icon = achbox.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite;
                AchievementHandler.default_icon = default_icon;
                //ui = GameObject.Instantiate(ab.LoadAsset<GameObject>("AchievementCoreUI.prefab"));
                //ui.transform.SetParent(canvas.transform, false);
                //AchievementHandler.menu_parent = canvas.transform.GetChild(0).GetChild(0).gameObject;
                //ui.transform.localPosition = new Vector3(-521f, -564f, 0f);
                //ui.transform.localScale = new Vector3(1.1f, 1.1f, 0.8f);
                //ui.transform.Find("MENU_PARENT/Button/Text").GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                //AchievementHandler.ui = ui;
                LoadAchievements();
                GameObject.DontDestroyOnLoad(g);
                //GameObject.DontDestroyOnLoad(achbox);
                //GameObject.DontDestroyOnLoad(ui);
                ModConsole.Log("<color=yellow>Achievement Core loaded succesfully!</color>");
                ab.Unload(false);
            }
            else
            {
                //ui.transform.localPosition = new Vector3(-521f, -564f, 0f);
                //ui.transform.localScale = new Vector3(1.1f, 1.1f, 0.8f);
                //ui.transform.Find("MENU_PARENT/Button/Text").GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
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
