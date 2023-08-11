using System.Collections.Generic;
using UnityEngine;
public struct AchievementData
{
    public string mod_id;
    public string name;
    public string description;
    public Sprite icon;
    public bool hidden;
}

public static class Achievement
{
    public static void CreateAchievement(string achievementID, string mod_name, string name, string description, Sprite icon, bool hidden)
    {
        AchievementIDHolder.achievements.Add(achievementID, new AchievementData
        {
            mod_id = mod_name,
            name = name,
            description = description,
            icon = icon,
            hidden = hidden,
        });
    }
    public static void CreateAchievement(string achievementID, string mod_name, string name, string description, Sprite icon)
    {
        CreateAchievement(achievementID, mod_name, name, description, icon, false);
    }
    public static void CreateAchievement(string achievementID, string mod_name, string name, string description, bool hidden)
    {
        CreateAchievement(achievementID, mod_name, name, description, null, hidden);
    }
    public static void CreateAchievement(string achievementID, string mod_name, string name, string description)
    {
        CreateAchievement(achievementID, mod_name, name, description, null, false);
    }
    public static void TriggerAchievement(string achievement_id)
    {
        AchievementIDHolder.AchievementHandler.TriggerAchievement(achievement_id);
    }
    public static bool IsAchievementUnlocked(string achievement_id)
    {
        return AchievementIDHolder.unlocked_achievements.Contains(achievement_id);
    }
}
class AchievementIDHolder
{
    public static List<string> unlocked_achievements = new List<string>();
    public static List<string> locked_achievements = new List<string>();
    public static AchievementHandler AchievementHandler;
    public static Dictionary<string, AchievementData> achievements;

    static AchievementIDHolder()
    {
        achievements = new Dictionary<string, AchievementData>();
    }
}