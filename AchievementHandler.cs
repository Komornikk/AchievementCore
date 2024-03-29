﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
class AchievementHandler : MonoBehaviour
{
    [SerializeField] GameObject box_prefab, filler, progress_bar;
    public GameObject achievement_box, ui;
    Sprite default_icon;
    Text header, description_text;
    Image Icon;
    Animation anim;
    private bool playing = false;
    private List<string> queueID = new List<string>();
    public void StartSecondPass()
    {
        StartCoroutine(AchievementCore.SecondPassMenu());
    }
    public void Start()
    {
        header = achievement_box.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        description_text = achievement_box.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        Icon = achievement_box.transform.GetChild(2).GetChild(0).GetComponent<Image>();
        progress_bar = ui.transform.GetChild(0).GetChild(1).GetChild(3).gameObject;
        anim = achievement_box.GetComponent<Animation>();
    }
    public void TriggerAchievement(string achievement_id)
    {
        if (AchievementCore.scb.GetValue())
        {
            TriggerAchievement(achievement_id, true);
        }
        else if (AchievementIDHolder.achievements.ContainsKey(achievement_id))
        {
            AchievementData ad = AchievementIDHolder.achievements[achievement_id];
            if (AchievementIDHolder.unlocked_achievements.Contains(achievement_id)) return;
            AchievementIDHolder.unlocked_achievements.Add(achievement_id);
            AchievementIDHolder.locked_achievements.Remove(achievement_id);
            //MSCLoader.ModConsole.Print($"is icon null?: {ad.icon == null}");
            StartCoroutine(TriggerAchievementBox(ad));
            return;
        }
    }
    public void TriggerAchievement(string achievement_id, bool silent)
    {
        if (!silent) TriggerAchievement(achievement_id);
        else
        {
            if ((AchievementIDHolder.achievements.ContainsKey(achievement_id)))
            {
                if (AchievementIDHolder.unlocked_achievements.Contains(achievement_id)) return;
                AchievementIDHolder.unlocked_achievements.Add(achievement_id);
                AchievementIDHolder.locked_achievements.Remove(achievement_id);
            }
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
    private IEnumerator TriggerAchievementBox(AchievementData ad)
    {
        if (playing)
        {
            AddToQueue(GetIDAchievement(ad));
            StopCoroutine(TriggerAchievementBox(ad));
        }
        playing = true;
        header.text = ad.name.ToUpper();
        description_text.text = ad.description.ToUpper();
        Icon.sprite = ad.icon ?? default_icon;
        anim.Play("in");
        yield return new WaitForSeconds(5f);
        anim.Play("out");
        yield return new WaitForSeconds(anim["out"].length + anim["out"].normalizedTime);
        playing = false;
        NextAchievement();
    }
    string GetIDAchievement(AchievementData ad)
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
        Transform boxParent = ui.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0);
        Transform existingBox = boxParent.Find($"Achievement_{id}");
        if (existingBox != null) return;

        AchievementData achievementData = AchievementIDHolder.achievements[id];
        GameObject inst = GameObject.Instantiate(box_prefab);
        inst.transform.SetParent(boxParent);
        inst.name = $"Achievement_{id}";
        inst.transform.localScale = Vector3.one;

        Text textName = inst.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        Text textDescription = inst.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        Image imageIcon = inst.transform.GetChild(2).GetChild(0).GetComponent<Image>();

        if (locked && !hidden)
        {
            textName.text = achievementData.name.ToUpper();
            textDescription.text = achievementData.description.ToUpper();
        }
        else if (hidden && locked)
        {
            textName.text = "hidden achievement".ToUpper();
            textDescription.text = "content will be revealed after unlocking the achievement.".ToUpper();
            inst.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            inst.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            textName.text = achievementData.name.ToUpper();
            textDescription.text = achievementData.description.ToUpper();
            if (achievementData.icon != null) imageIcon.sprite = achievementData.icon;
        }

        if (locked && !hidden)
        {
            inst.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            inst.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        }
        else if (hidden && locked)
        {
            inst.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            inst.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            if (achievementData.icon != null)
            {
                imageIcon.GetComponent<Image>().sprite = achievementData.icon;
            }
        }
    }
    public void GenerateAchievementList(string mod_id)
    {
        Transform boxParent = ui.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0);

        // remove existing boxes
        foreach (Transform child in boxParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        // generate progress bar
        float unlockedCount = 0;
        float lockedHiddenCount = 0;
        //count how many achievements are unlocked
        foreach (var pair in AchievementIDHolder.achievements)
        {
            AchievementData data = pair.Value;
            if (data.mod_id == mod_id)
            {
                if (AchievementIDHolder.unlocked_achievements.Contains(pair.Key))
                {
                    unlockedCount++;
                }
                else if (AchievementIDHolder.locked_achievements.Contains(pair.Key) || data.hidden)
                {
                    lockedHiddenCount++;
                }
            }
        }
        // calculate the percentage and set the progress
        float percentage = unlockedCount / (unlockedCount + lockedHiddenCount);
        if (AchievementCore.DEBUG) MSCLoader.ModConsole.Print(percentage.ToString());
        progress_bar.transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = percentage;
        progress_bar.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = percentage == 1f ? "100%" : Mathf.Round(percentage * 100).ToString("0\\%");
        progress_bar.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = $"YOU'VE COMPLETED <color=yellow>{unlockedCount}</color> OUT OF <color=yellow>{unlockedCount + lockedHiddenCount}</color>!";

        // generate unlocked achievements
        foreach (string id in AchievementIDHolder.unlocked_achievements)
        {
            if (AchievementIDHolder.achievements.TryGetValue(id, out AchievementData achievementData) && achievementData.mod_id == mod_id)
            {
                GenerateBox(id, false, false);
            }
        }

        // generate locked achievements (excluding hidden)
        foreach (string id in AchievementIDHolder.locked_achievements)
        {
            if (AchievementIDHolder.achievements.TryGetValue(id, out AchievementData achievementData) && !achievementData.hidden && achievementData.mod_id == mod_id)
            {
                GenerateBox(id, true, false);
            }
        }

        //generate hidden
        foreach (string id in AchievementIDHolder.achievements.Keys)
        {
            AchievementData achievementData = AchievementIDHolder.achievements[id];
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

        // add filler box so the last achievement box doesn't get cut in half by the list 
        GameObject.Instantiate(filler).transform.SetParent(boxParent);
    }
}