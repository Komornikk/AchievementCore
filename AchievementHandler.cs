using UnityEngine;

namespace AchievementCore
{
    public class AchievementHandler : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            
        }
        public void TriggerAchievement(string achievement_id, string achievement_name)
        {
            if (AchievementIDHolder.unlocked_achievements.Contains(achievement_id)) return;
            if (AchievementIDHolder.Achievement_IDs.Count != 0)
            {
                if (!AchievementIDHolder.Achievement_IDs.Contains(achievement_id))
                {
                    MSCLoader.ModConsole.Error("Achievement ID doesn't exist! Maybe you forgot to add it to the AchievementIDHolder?");
                    return;
                }
                AchievementIDHolder.unlocked_achievements.Add(achievement_id);
                AchievementIDHolder.locked_achievements.Remove(achievement_id);
                MSCLoader.ModConsole.Print("Achievement Get!\n" + achievement_name);
                return;
            }
            else
            {
                MSCLoader.ModConsole.Error("The Achievement_IDs list is empty!");
                return;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}