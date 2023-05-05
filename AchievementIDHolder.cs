using System.Collections.Generic;

namespace AchievementCore
{
    public static class AchievementIDHolder 
    {
        public static List<string> Achievement_IDs;
        public static List<string> unlocked_achievements;
        public static List<string> locked_achievements;
        public static AchievementHandler AchievementHandler;
        static AchievementIDHolder()
        {
            Achievement_IDs = new List<string>();
            unlocked_achievements = new List<string>();
            locked_achievements = new List<string>();
        }
    }
}