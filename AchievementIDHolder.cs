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
public class Achievement
{
    public Achievement(string achievementID, string mod_name, string name, string description, Sprite icon, bool hidden)
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
}
public class AchievementIDHolder
{
    public static List<string> unlocked_achievements = new List<string>();
    public static List<string> locked_achievements = new List<string>();
    public static List<Sprite> images;
    public static AchievementHandler AchievementHandler;
    public static Dictionary<string, AchievementData> achievements;

    static AchievementIDHolder()
    {
        achievements = new Dictionary<string, AchievementData>();
    }
}