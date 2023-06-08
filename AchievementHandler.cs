using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace AchievementCore
{
    public class AchievementHandler : MonoBehaviour
    {
        [SerializeField] private GameObject box_prefab, filler;
        public GameObject achievement_box, ui;
        [SerializeField] private Sprite default_icon;
        private Text header, description_text;
        private Image Icon;
        private Animation anim;
        private bool playing = false;
        private List<string> queueID = new List<string>();
        public void StartSecondPass()
        {
            StartCoroutine(AchievementCore.SecondPassMenu());
        }
        private void Start()
        {
            //achievement_box = achievement_box.transform.GetChild(0).gameObject;
            header = achievement_box.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            description_text = achievement_box.transform.GetChild(1).GetChild(0).GetComponent<Text>();
            Icon = achievement_box.transform.GetChild(2).GetChild(0).GetComponent<Image>();
            anim = achievement_box.GetComponent<Animation>();
        }
        public void TriggerAchievement(string achievement_id)
        {
            if (AchievementIDHolder.achievements.ContainsKey(achievement_id))
            {
                AchievementIDHolder.AchievementData ad = AchievementIDHolder.achievements[achievement_id];
                if (AchievementIDHolder.unlocked_achievements.Contains(achievement_id)) return;
                AchievementIDHolder.unlocked_achievements.Add(achievement_id);
                AchievementIDHolder.locked_achievements.Remove(achievement_id);
                //MSCLoader.ModConsole.Print($"is icon null?: {ad.icon == null}");
                StartCoroutine(TriggerAchievementBox(ad));
                return;
            }
        }
        private void AddToQueue(string ID)
        {
            queueID.Add(ID);
        }
        private void NextAchievement()
        {
            if (queueID != null && queueID.Count > 0)
            {
                TriggerAchievementBox(AchievementIDHolder.achievements[queueID[0]]);
                RemoveFromQueue();
            }
            else return;
        }
        private void RemoveFromQueue()
        {
            queueID.Remove(queueID[0]);
        }
        private IEnumerator TriggerAchievementBox(AchievementIDHolder.AchievementData ad)
        {
            if (playing)
            {
                AddToQueue(GetIDAchievement(ad)); 
                StopCoroutine(TriggerAchievementBox(ad));
            }
            playing = true;
            header.text = ad.name.ToUpper();
            description_text.text = ad.description.ToUpper();
            Icon.sprite = ad.icon == null ? default_icon : ad.icon;
            anim.Play("in");
            yield return new WaitForSeconds(5f);
            anim.Play("out");
            yield return new WaitForSeconds(anim["out"].length + anim["out"].normalizedTime);
            playing = false;
            NextAchievement();
        }
        string GetIDAchievement(AchievementIDHolder.AchievementData ad)
        {
            string id = null;
            foreach (var pair in AchievementIDHolder.achievements)
            {
                if (pair.Value.Equals(ad))
                {
                    id = pair.Key;
                    break;
                }
            }
            return id;
        }
        void GenerateBox(string id, bool locked, bool hidden)
        {
            if (!ui.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).Find($"Achievement_{id}"))
            {
                AchievementIDHolder.AchievementData achievementData = AchievementIDHolder.achievements[id];
                GameObject inst = GameObject.Instantiate(box_prefab);
                inst.transform.SetParent(ui.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0));
                inst.name = $"Achievement_{id}";
                if (locked && !hidden)
                {
                    inst.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = achievementData.name.ToUpper();
                    inst.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = achievementData.description.ToUpper();
                    inst.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                    inst.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                    return;
                }
                else if (hidden && locked)
                {
                    inst.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "hidden achievement".ToUpper();
                    inst.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "content will be revealed after unlocking the achievement".ToUpper();
                    inst.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                    inst.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
                    return;
                }
                else
                {
                    inst.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = achievementData.name.ToUpper();
                    inst.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = achievementData.description.ToUpper();
                    if (achievementData.icon != null) inst.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = achievementData.icon;
                }
            }
        }
        public void GenerateAchievementList(string mod_id)
        {
            //remove existing boxes
            foreach (Transform child in ui.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0))
            {
                GameObject.Destroy(child.gameObject);
            }
            //generate unlocked
            foreach (string id in AchievementIDHolder.unlocked_achievements)
            {
                if (AchievementIDHolder.achievements.TryGetValue(id, out AchievementIDHolder.AchievementData achievementData))
                {
                    if (achievementData.mod_id == mod_id)
                    GenerateBox(id, false, false);
                }
            }

            //generate locked
            foreach (string id in AchievementIDHolder.locked_achievements)
            {
                if (AchievementIDHolder.achievements.TryGetValue(id, out AchievementIDHolder.AchievementData achievementData) && !achievementData.hidden)
                {
                    if (achievementData.mod_id == mod_id)
                        GenerateBox(id, true, false);
                }
            }

            //generate hidden
            foreach (string id in AchievementIDHolder.achievements.Keys)
            {
                AchievementIDHolder.AchievementData achievementData = AchievementIDHolder.achievements[id];
                if (achievementData.hidden && !AchievementIDHolder.locked_achievements.Contains(id))
                {
                    if (achievementData.mod_id == mod_id)
                        GenerateBox(id, false, true);
                }
                else if (achievementData.hidden && AchievementIDHolder.locked_achievements.Contains(id))
                {
                    if (achievementData.mod_id == mod_id)
                        GenerateBox(id, true, true);
                }
            }
            //add filler box so the last achievement box doesn't get cut in half by the list 
            GameObject.Instantiate(filler).transform.SetParent(ui.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0));
        }
    }
}