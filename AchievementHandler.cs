using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace AchievementCore
{
    public class AchievementHandler : MonoBehaviour
    {
        //private static GameObject OptionsMenu, ui, menu_parent;
        public GameObject achievement_box;
        private bool set = false;
        public Sprite default_icon;
        //private GameObject achievement_box;
        public void TriggerAchievement(string achievement_id, string achievement_name, string achievement_description, Sprite icon)
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
                StartCoroutine(TriggerAchievementBox(achievement_name, achievement_description, icon));
                //MSCLoader.ModConsole.Print("Achievement Get!\n" + achievement_name + "\n" + achievement_description);
                return;
            }
            else
            {
                MSCLoader.ModConsole.Error("The Achievement_IDs list is empty!");
                return;
            }
        }
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
                StartCoroutine(TriggerAchievementBox(achievement_name, achievement_description));
                //MSCLoader.ModConsole.Print("Achievement Get!\n" + achievement_name + "\n" + achievement_description);
                return;
            }
            else
            {
                MSCLoader.ModConsole.Error("The Achievement_IDs list is empty!");
                return;
            }
        }
        /*
        private void TriggerAchievementBox(string name, string description)
        {
            achievement_box.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = name;
            achievement_box.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = description;
            achievement_box.GetComponent<Animation>
        }
        */
        private IEnumerator TriggerAchievementBox(string name, string description, Sprite icon)
        {
            achievement_box.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = name.ToUpper();
            achievement_box.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = description.ToUpper();
            achievement_box.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = icon;
            achievement_box.GetComponent<Animation>().Play("in");
            yield return new WaitForSeconds(6f);
            achievement_box.GetComponent<Animation>().Play("out");
        }
        private IEnumerator TriggerAchievementBox(string name, string description)
        {
            achievement_box.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = name;
            achievement_box.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = description;
            achievement_box.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = default_icon;
            achievement_box.GetComponent<Animation>().Play("in");
            yield return new WaitForSeconds(6f);
            achievement_box.GetComponent<Animation>().Play("out");
        }
        /*
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
        */
    }
}