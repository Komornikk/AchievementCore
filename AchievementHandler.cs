using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
namespace AchievementCore
{
    public class AchievementHandler : MonoBehaviour
    {
        public GameObject achievement_box, box_prefab, ui, filler;
        public Sprite default_icon;
        private Text header, description_text;
        private Image Icon;
        private Animation anim;
        private bool playing = false;
        private List<string> na = new List<string>();
        private List<string> de = new List<string>();
        private List<Sprite> ic = new List<Sprite>();
        private void Start()
        {
            header = achievement_box.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            description_text = achievement_box.transform.GetChild(1).GetChild(0).GetComponent<Text>();
            Icon = achievement_box.transform.GetChild(2).GetChild(0).GetComponent<Image>();
            anim = achievement_box.GetComponent<Animation>();
        }
        public void TriggerAchievement(string mod_id, string achievement_id, string achievement_name, string achievement_description, Sprite icon)
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
                return;
            }
            else
            {
                MSCLoader.ModConsole.Error("The Achievement_IDs list is empty!");
                return;
            }
        }
        public void TriggerAchievement(string mod_id, string achievement_id)
        {
            if (AchievementIDHolder.achievements.ContainsKey(achievement_id))
            {
                AchievementIDHolder.AchievementData ad = AchievementIDHolder.achievements[achievement_id];
                //MSCLoader.ModConsole.Print(ad.locked);
                //if (!ad.locked) return;
                //ad.locked = false;
                //MSCLoader.ModConsole.Print(ad.locked);
                if (!AchievementIDHolder.locked_achievements.Contains(achievement_id)) return;
                AchievementIDHolder.unlocked_achievements.Add(achievement_id);
                AchievementIDHolder.locked_achievements.Remove(achievement_id);
                GenerateAchievementList(mod_id);
                StartCoroutine(TriggerAchievementBox(ad.name.Replace(mod_id, ""), ad.description));
                return;
            }
        }
        private void AddToQueue(string n, string d, Sprite i)
        {
            na.Add(n);
            de.Add(d);
            ic.Add(i);
        }
        private void NextAchievement()
        {
            if (na != null && de != null && ic != null)
            {
                StartCoroutine(TriggerAchievementBox(na[0], de[0], ic[0]));
                RemoveFromQueue();
            }
            else return;
        }
        private void RemoveFromQueue()
        {
            na.Remove(na[0]);
            de.Remove(de[0]);
            ic.Remove(ic[0]);
        }
        private IEnumerator TriggerAchievementBox(string name, string description, Sprite icon)
        {
            if (playing)
            {
                AddToQueue(name, description, icon);
                StopCoroutine(TriggerAchievementBox(name, description, icon));
            }
            playing = true;
            header.text = name.ToUpper();
            description_text.text = description.ToUpper();
            Icon.sprite = icon;
            anim.Play("in");
            yield return new WaitForSeconds(6f);
            anim.Play("out");
            yield return new WaitForSeconds(anim["out"].length + anim["out"].normalizedTime);
            playing = false;
            NextAchievement();
        }
        private IEnumerator TriggerAchievementBox(string name, string description)
        {
            if (playing)
            {
                AddToQueue(name, description, default_icon);
                StopCoroutine(TriggerAchievementBox(name, description));
            }
            else
            {
                playing = true;
                header.text = name.ToUpper();
                description_text.text = description.ToUpper();
                Icon.sprite = default_icon;
                anim.Play("in");
                yield return new WaitForSeconds(6f);
                anim.Play("out");
                yield return new WaitForSeconds(anim["out"].length + anim["out"].normalizedTime);
                playing = false;
                NextAchievement();
            }
        }
        public void GenerateAchievementList(string mod_id)
        {
            //remove existing boxes
            foreach (Transform child in ui.transform.GetChild(1).GetChild(1).GetChild(0))
            {
                GameObject.Destroy(child.gameObject);
            }
            //generate achievement boxes
            foreach (string id in AchievementIDHolder.achievements.Keys)
            {
                AchievementIDHolder.AchievementData achievementData = AchievementIDHolder.achievements[id];
                if (achievementData.mod_id == mod_id)
                {
                    GameObject inst = GameObject.Instantiate(box_prefab);
                    inst.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = achievementData.name.ToUpper();
                    inst.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = achievementData.description.ToUpper();
                    inst.transform.SetParent(ui.transform.GetChild(1).GetChild(1).GetChild(0));
                    inst.name = $"Achievement_{id}";
                    /*
                    if (achievementData.hidden)
                    {
                        // do something for hidden
                    }
                    else if (AchievementIDHolder.locked_achievements.Contains(id))
                    {
                        inst.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                        inst.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                    }
                    */
                }
                //else /*MSCLoader.ModConsole.Error($"AchievementCore: No Mod ID for achievement {id}!"); */MSCLoader.ModConsole.Error($"id one:{id}  id two: {mod_id}");
            }

            //add filler box so the last achievement box doesn't get cut in half by the list 
            GameObject.Instantiate(filler).transform.SetParent(ui.transform.GetChild(1).GetChild(1).GetChild(0));
        }
    }
}