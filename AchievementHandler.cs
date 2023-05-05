using UnityEngine;
using UnityEngine.UI;
namespace AchievementCore
{
    public class AchievementHandler : MonoBehaviour
    {
        public static GameObject OptionsMenu, ui, menu_parent;
        private bool set = false;
        public void TriggerAchievement(string achievement_id, string achievement_name, string achievement_description)
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
                MSCLoader.ModConsole.Print("Achievement Get!\n" + achievement_name + "\n<color=green>" + achievement_description + "</color>");
                return;
            }
            else
            {
                MSCLoader.ModConsole.Error("The Achievement_IDs list is empty!");
                return;
            }
        }
        void Update()
        {
            if (Application.loadedLevelName == "GAME")
            {
                if (OptionsMenu == null) OptionsMenu = GameObject.Find("Systems").transform.Find("OptionsMenu").gameObject;
                menu_parent.SetActive(OptionsMenu.activeSelf);
            }
        }
        void LateUpdate()
        {
            if (Application.loadedLevelName == "GAME" && !set)
            {
                set = true;
                ui.transform.localPosition = new Vector3(-334f, -205f, 0f);
                ui.transform.Find("MENU_PARENT/Button/Text").GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            }
        }
    }
}