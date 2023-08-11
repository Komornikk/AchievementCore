using MSCLoader;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using UnityEngine.UI;
public class AchievementCore : Mod
{
    public override string ID => "AchievementCore";
    public override string Name => "Achievement Core";
    public override string Author => "komornik";
    public override string Version => "1.0.2";
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
    }
    public void Mod_Update()
    {
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
        int mods = 30;
        int achievements = 100;

        for (int modIndex = 0; modIndex < mods; modIndex++)
        {
            string modId = $"mod_{modIndex}";
            string modName = $"Mod {modIndex}";
            for (int achievementIndex = 0; achievementIndex < achievements; achievementIndex++)
            {
                //doesn't work currently
                //Achievement newAchievement = new Achievement($"{modId}_achievement_{achievementIndex}", modName, $"Achievement {achievementIndex}", $"Description of Achievement {achievementIndex}");
            }
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
        achievementExplorer.GetComponent<CanvasController>().FindOptionsMenu();
        achievementExplorer.GetComponent<CanvasController>().onLoad = true;
        achievementExplorer.GetComponent<CanvasController>().ui.SetActive(false);
    }
    public void Mod_OnMenuLoad()
    {
        AchievementIDHolder.achievements.Clear();
        ConsoleCommand.Add(new DebugController());
        ab = LoadAssets.LoadBundle("AchievementCore.Assets.achcore.unity3d");
        coreGO = GameObject.Instantiate(ab.LoadAsset<GameObject>("coreGO.prefab"));
        coreGO.name = "AchievementCore";
        AchievementHandler = coreGO.GetComponent<AchievementHandler>();
        AchievementIDHolder.AchievementHandler = AchievementHandler;
        canvas = ModUI.CreateCanvas("AchievementCoreUI", true);
        box_prefab = ab.LoadAsset<GameObject>("AchievementBox.prefab");
        achbox = GameObject.Instantiate(ab.LoadAsset<GameObject>("achbox.prefab"));
        achbox.transform.SetParent(canvas.transform);
        achbox.transform.localScale = Vector3.one;
        achbox.transform.localPosition = new Vector3(-1066f, 361f, 0f);
        achbox.name = "AchievementBoxHolder";
        achievementExplorer = GameObject.Instantiate(ab.LoadAsset<GameObject>("AchievementUI.prefab"));
        achievementExplorer.transform.SetParent(canvas.transform);
        achievementExplorer.name = "AchievementUI";
        AchievementHandler.ui = achievementExplorer;
        cc = achievementExplorer.GetComponent<CanvasController>();
        cc.onLoad = false;
        cc.ah = AchievementHandler;
        filler = ab.LoadAsset<GameObject>("filler.prefab");
        AchievementHandler.achievement_box = achbox.transform.GetChild(0).gameObject;
        default_icon = achbox.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite;
        GameObject.DontDestroyOnLoad(coreGO);
        ModConsole.Log("<color=yellow>Achievement Core loaded succesfully!</color>");
        AddBaseAchievements();
        //AddVanillaAchievements();
        AchievementHandler.TriggerAchievement("achcore_using_achcore", true);
        ab.Unload(false);
        AchievementHandler.StartSecondPass();
        //GenerateALotOfAchievementsAndMods();
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
