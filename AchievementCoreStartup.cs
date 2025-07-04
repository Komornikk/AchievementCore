﻿using MSCLoader;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using UnityEngine.UI;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

public class AchievementCore : Mod
{
    public override string ID => "AchievementCore";
    public override string Name => "Achievement Core";
    public override string Author => "komornik";
    public override string Version => "1.0.5";
    public override string Description => "Achievement system for all your mods!";
    public static bool DEBUG = false;
    protected private static GameObject canvas, achbox, coreGO, achievementExplorer, filler, box_prefab;
    protected private static AssetBundle ab;
    protected private static AchievementHandler AchievementHandler;
    protected private static CanvasController cc;
    protected static Sprite default_icon;
    protected private static readonly string saveFile = Application.persistentDataPath + "\\Achievements.dat";

    public override void ModSetup()
    {
        SetupFunction(Setup.OnMenuLoad, Mod_OnMenuLoad);
        SetupFunction(Setup.OnLoad, Mod_OnLoad);
        SetupFunction(Setup.OnSave, Mod_OnSave);
        SetupFunction(Setup.Update, Mod_Update);
        SetupFunction(Setup.ModSettings, Mod_Settings);
    }
    public static SettingsCheckBox scb;
    public void Mod_Settings()
    {
        scb = Settings.AddCheckBox(this, "disableAchievementPopups", "DISABLE ACHIEVEMENT POPUPS", false);
    }
    public void Mod_Update()
    {
        achbox.SetActive(!scb.GetValue());
        if (Input.GetKeyDown(KeyCode.G) && DEBUG)
        {
            PrintAllIDs();
        }
        if (Input.GetKeyDown(KeyCode.H) && DEBUG)
        {
            foreach (string s in cc.mod_ids)
            {
                ModConsole.Print(s);
            }
        }
    }
    void GenerateALotOfAchievementsAndMods()
    {
        int mods = 3;
        int achievements = 3;

        for (int modIndex = 0; modIndex < mods; modIndex++)
        {
            string modId = $"mod_{modIndex}";
            string modName = $"Mod {modIndex}";
            Achievement.CreateAchievement(modId, modName, "afasfdaw", "asfawd");
        }
    }
    void PrintAllIDs()
    {
        MSCLoader.ModConsole.Print("<color=red>UNLOCKED ACHIEVEMENTS:</color>");
        if (AchievementIDHolder.unlocked_achievements.Count > 0)
        {
            foreach (string s in AchievementIDHolder.unlocked_achievements)
            {
                ModConsole.Print(s);
            }
        }
        else
        {
            ModConsole.Print("<color=green>There are no unlocked achievements</color>");
        }
        MSCLoader.ModConsole.Print("<color=red>LOCKED ACHIEVEMENTS:</color>");
        if (AchievementIDHolder.locked_achievements.Count == 0)
        {
            ModConsole.Print("<color=green>There are no locked achievements</color>");
        }
        else
        {
            foreach (string s in AchievementIDHolder.locked_achievements)
            {
                MSCLoader.ModConsole.Print(s);
            }
        }
    }
    public void Mod_OnLoad()
    {
        AchievementHandler.ui.GetComponent<CanvasController>().FindOptionsMenu();
        AchievementHandler.ui.GetComponent<CanvasController>().onLoad = true;
        AchievementHandler.ui.GetComponent<CanvasController>().ui.SetActive(false);
    }
    //public static void SetResolution(1280, 540, true);
    public void Mod_OnMenuLoad()
    {
        AchievementIDHolder.achievements.Clear();
        ConsoleCommand.Add(new DebugController());
        ab = LoadAssets.LoadBundle("AchievementCore.Assets.achcore.unity3d");
        canvas = GameObject.Instantiate(ab.LoadAsset<GameObject>("AchievementCoreCanvas.prefab"));
        coreGO = canvas.transform.Find("coreGO").gameObject;
        AchievementHandler = coreGO.GetComponent<AchievementHandler>();
        AchievementIDHolder.AchievementHandler = AchievementHandler;
        canvas.name = "AchievementCoreCanvas";
        box_prefab = ab.LoadAsset<GameObject>("AchievementBox.prefab");
        achbox = canvas.transform.Find("achbox").gameObject;
        achbox.name = "AchievementBoxHolder";
        AchievementHandler.ui = canvas.transform.Find("AchievementUI").gameObject;
        cc = AchievementHandler.ui.GetComponent<CanvasController>();
        FsmState s = GameObject.Find("Interface").transform.Find("Buttons/ButtonContinue").GetComponent<PlayMakerFSM>().GetState("Action");
        cc.boltscrew = s.GetAction<PlaySound>(0).clip.Value as AudioClip;
        cc.onLoad = false;
        filler = ab.LoadAsset<GameObject>("filler.prefab");
        AchievementHandler.achievement_box = achbox.transform.GetChild(0).gameObject;
        default_icon = achbox.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite;
        GameObject.DontDestroyOnLoad(canvas);
        ModConsole.Log("<color=yellow>Achievement Core loaded succesfully!</color>");
        
        AddBaseAchievements();
        AchievementHandler.TriggerAchievement("achcore_using_achcore", true);
        ab.Unload(false);
        AchievementHandler.StartSecondPass();
    }
    void AddVanillaAchievements()
    {
        // nothing here for now
    }
    void AddBaseAchievements()
    {
        Achievement.CreateAchievement("achcore_using_achcore", "base", "Achievement Get!", "You're a user of <color=yellow>Achievement Core</color>!", null, false);
    }
    public static IEnumerator SecondPassMenu()
    {
        yield return null;
        AchievementIDHolder.locked_achievements.AddRange(AchievementIDHolder.achievements.Keys.ToList());
        LoadAchievements();
        foreach (string s in AchievementIDHolder.achievements.Keys)
        {
            if (!cc.mod_ids.Contains(s)) cc.mod_ids.Add(AchievementIDHolder.achievements[s].mod_id);
        }
        AchievementHandler.TriggerAchievement("achcore_using_achcore", true);
        //AchievementHandler.SecondPass();
        //cc.SecondPass();
        cc.GenerateBaseAchievementKey();
    }
    private static void LoadAchievements()
    {
        if (File.Exists(saveFile))
        {
            using (var binaryReader = new BinaryReader(File.Open(saveFile, FileMode.Open)))
            {
                int count = binaryReader.ReadInt32();
                //MSCLoader.ModConsole.Print(count);
                AchievementIDHolder.unlocked_achievements.Clear();
                for (int i = 0; i < count; i++)
                {
                    string id = binaryReader.ReadString();
                    AchievementIDHolder.unlocked_achievements.Add(id);
                    AchievementIDHolder.locked_achievements.Remove(id);
                }
            }
        }
        else
        {
            AchievementIDHolder.unlocked_achievements.Clear();
            foreach (string id in AchievementIDHolder.achievements.Keys)
            {
                AchievementIDHolder.locked_achievements.Add(id);
            }
        }
    }
    public void Mod_OnSave()
    {
        using (var binaryWriter = new BinaryWriter(File.Open(saveFile, FileMode.Create)))
        {
            binaryWriter.Write(AchievementIDHolder.unlocked_achievements.Count);
            foreach (string id in AchievementIDHolder.unlocked_achievements)
            {
                binaryWriter.Write(id);
            }
        }
    }

}
