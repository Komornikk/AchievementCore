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
        private static GameObject canvas, achbox, coreGO, achievementExplorer, filler, box_prefab;
        private static AssetBundle ab;
        private static AchievementHandler AchievementHandler;
        static Sprite default_icon;
        private static object[] achievements;
        private static readonly string saveFile = Application.persistentDataPath + "\\Achievements.dat";
        public static void OnLoad()
        {
            if (!onload)
            {
                onload = true;
                achievementExplorer.GetComponent<CanvasController>().FindOptionsMenu();
                achievementExplorer.GetComponent<CanvasController>().onLoad = true;
                achievementExplorer.GetComponent<CanvasController>().ui.SetActive(false);
                GameObject.DontDestroyOnLoad(coreGO);
                GameObject.DontDestroyOnLoad(canvas);
            }
        }
        public static void AddAchievementNames(string mod_id, List<string> achievement_names)
        {
            for (int i = 0; i < achievement_names.Count; i++)
            {
                achievement_names[i] = mod_id + "_" + achievement_names[i];
            }
            AchievementIDHolder.names.AddRange(achievement_names);
        }
        public static void AddAchievementNames(string mod_id, string achievement_name)
        {
            AchievementIDHolder.names.Add(mod_id + "_" + achievement_name);
        }
        public static void AddAchievementDescriptions(string mod_id, List<string> achievement_descs)
        {
            for (int i = 0; i < achievement_descs.Count; i++)
            {
                achievement_descs[i] = mod_id + "_" + achievement_descs[i];
            }
            AchievementIDHolder.descs.AddRange(achievement_descs);
        }
        public static void AddAchievementDescriptions(string mod_id, string achievement_desc)
        {
            AchievementIDHolder.descs.Add(mod_id + "_" + achievement_desc);
        }
        public static void Startup()
        {
            if (!started)
            {
                started = true;
                onload = false;
                coreGO = new GameObject("AchievementCore");
                AchievementHandler = coreGO.AddComponent<AchievementHandler>();
                AchievementIDHolder.AchievementHandler = AchievementHandler;
                canvas = ModUI.CreateCanvas("AchievementCoreUI", true);
                ab = LoadAssets.LoadBundle("AchievementCore.Assets.achcore.unity3d");
                box_prefab = ab.LoadAsset<GameObject>("AchievementBox.prefab");
                AchievementHandler.box_prefab = box_prefab;
                achbox = GameObject.Instantiate(ab.LoadAsset<GameObject>("achbox.prefab"));
                achbox.transform.SetParent(canvas.transform);
                achbox.transform.localScale = Vector3.one;
                achbox.transform.localPosition = new Vector3(-1016f, 311f, 0f);
                achbox.name = "AchievementBoxHolder";
                achievementExplorer = GameObject.Instantiate(ab.LoadAsset<GameObject>("AchievementUI.prefab"));
                achievementExplorer.transform.SetParent(canvas.transform);
                achievementExplorer.transform.localPosition = new Vector3(-483f, -89f, 0);
                achievementExplorer.name = "AchievementUI";
                AchievementHandler.ui = achievementExplorer; 
                achievementExplorer.GetComponent<CanvasController>().onLoad = false;
                //achievementExplorer.GetComponent<CanvasController>().ui.SetActive(false);
                //achievementExplorer.AddComponent<CanvasController>();
                filler = ab.LoadAsset<GameObject>("filler.prefab");
                AchievementHandler.filler = filler;
                AchievementHandler.achievement_box = achbox.transform.GetChild(0).gameObject;
                default_icon = achbox.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite;
                AchievementHandler.default_icon = default_icon;
                LoadAchievements();
                GameObject.DontDestroyOnLoad(coreGO);
                AchievementHandler.GenerateAchievementList("StorageShed");
                ModConsole.Log("<color=yellow>Achievement Core loaded succesfully!</color>");
                ab.Unload(false);
            }
            else
            {
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
        public static void SaveAchievements()
        {
            if (!saving)
            {
                saving = true;
                SaveBytes.save(saveFile, AchievementIDHolder.unlocked_achievements);
                SaveBytes.save(saveFile, AchievementIDHolder.locked_achievements);
            }
        }
    }
}
