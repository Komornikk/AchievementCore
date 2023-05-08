using System.Collections.Generic;
using UnityEngine;
namespace AchievementCore
{
    public static class AchievementIDHolder 
    {
        public static List<string> Achievement_IDs;
        public static List<string> unlocked_achievements;
        public static List<string> locked_achievements;
        public static List<string> mod_id;
        public static List<string> names;
        public static List<string> descs;
        public static List<Sprite> images;
        public static AchievementHandler AchievementHandler;
        static AchievementIDHolder()
        {
            Achievement_IDs = new List<string>();
            unlocked_achievements = new List<string>();
            locked_achievements = new List<string>();
            mod_id = new List<string>();
            names = new List<string>();
            descs = new List<string>();
            images = new List<Sprite>();
        }
    }
}