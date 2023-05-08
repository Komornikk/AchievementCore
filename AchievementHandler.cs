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
        public void TriggerAchievement(string mod_id, string achievement_id, string achievement_name, string achievement_description)
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
                return;
            }
            else
            {
                MSCLoader.ModConsole.Error("The Achievement_IDs list is empty!");
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
            foreach (GameObject g in ui.transform.GetChild(1).GetChild(1).GetChild(0))
            {
                GameObject.Destroy(g);
            }
            List<string> names = new List<string>();
            List<string> desc = new List<string>();
            //List<Sprite> icons = new List<Sprite>();
            foreach (string s in AchievementIDHolder.names)
            {
                if (s.Contains(mod_id+"_"))
                {
                    names.Add(s.Replace(mod_id + "_", ""));
                }
            }
            foreach (string s in AchievementIDHolder.descs)
            {
                if (s.Contains(mod_id + "_"))
                {
                    desc.Add(s.Replace(mod_id+"_", ""));
                }
            }
            for (int i = 0; i < names.Count; i++)
            {
                GameObject inst = GameObject.Instantiate(box_prefab);
                inst.transform.SetParent(ui.transform.GetChild(1).GetChild(1).GetChild(0));
                inst.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = names[i];
                inst.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = desc[i];
                //inst.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = icons[i];
            }
            GameObject.Instantiate(filler).transform.SetParent(ui.transform.GetChild(1).GetChild(0));
        }
    }
}