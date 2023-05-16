using System.Collections.Generic;
using UnityEngine;
namespace AchievementCore
{
    public static class AchievementIDHolder 
    {
        public struct AchievementData
        {
            public string mod_id;
            public string name;
            public string description;
            public bool hidden;
        }
        public static List<string> Achievement_IDs;
        public static List<string> unlocked_achievements;
        public static List<string> locked_achievements;
        public static List<Sprite> images;
        public static AchievementHandler AchievementHandler;
        public static Dictionary<string, AchievementData> achievements;

        static AchievementIDHolder()
        {
            achievements = new Dictionary<string, AchievementData>();
        }
    }
}