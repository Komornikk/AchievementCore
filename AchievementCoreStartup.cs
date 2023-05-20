using MSCLoader;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AchievementCore
{
    public class AchievementCore : Mod
    {
        public override string ID => "AchievementCore";
        public override string Name => "Achievement Core";
        public override string Author => "komornik";
        public override string Version => "1.0.0";
        public override string Description => "Achievement system for all your mods!";

        private static bool started = false, saving = false, onload = false, initialized = false;
        private static GameObject canvas, achbox, coreGO, achievementExplorer, filler, box_prefab;
        private static AssetBundle ab;
        private static AchievementHandler AchievementHandler;
        static Sprite default_icon;
        private static readonly string saveFile = Application.persistentDataPath + "\\Achievements.dat";

        public override void ModSetup()
        {
            SetupFunction(Setup.OnMenuLoad, Mod_OnMenuLoad);
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
        }
        public void Mod_OnLoad()
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
        public void Mod_OnMenuLoad()
        {
            if (!started && !initialized)
            {
                initialized = true;
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
                achbox.transform.localPosition = new Vector3(-1066f, 364f, 0f);
                achbox.name = "AchievementBoxHolder";
                achievementExplorer = GameObject.Instantiate(ab.LoadAsset<GameObject>("AchievementUI.prefab"));
                achievementExplorer.transform.SetParent(canvas.transform);
                achievementExplorer.transform.localPosition = new Vector3(-483f, -89f, 0);
                achievementExplorer.name = "AchievementUI";
                AchievementHandler.ui = achievementExplorer; 
                achievementExplorer.GetComponent<CanvasController>().onLoad = false;
                achievementExplorer.GetComponent<CanvasController>().ah = AchievementHandler;
                //achievementExplorer.GetComponent<CanvasController>().ui.SetActive(false);
                //achievementExplorer.AddComponent<CanvasController>();
                filler = ab.LoadAsset<GameObject>("filler.prefab");
                AchievementHandler.filler = filler;
                AchievementHandler.achievement_box = achbox.transform.GetChild(0).gameObject;
                default_icon = achbox.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite;
                AchievementHandler.default_icon = default_icon;
                //LoadAchievements();
                GameObject.DontDestroyOnLoad(coreGO);
                //if(AchievementIDHolder.achievements.Keys != null)
                //{
                //    foreach (string key in AchievementIDHolder.achievements.Keys)
                //    {
                //        AchievementIDHolder.locked_achievements.Add(key);
                //    }
                //}
                //AchievementHandler.GenerateAchievementList("StorageShed");
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
        }
        public void Mod_OnSave()
        {

        }
    }
}
