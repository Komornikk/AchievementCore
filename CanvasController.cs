using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace AchievementCore
{
    public class CanvasController : MonoBehaviour
    {

        private GameObject button, options_menu, console;
        public AchievementHandler ah;
        public GameObject ui;
        public Text mod_text;
        private bool enable = false;
        public bool onLoad = false;
        string currentText = "";
        HashSet<string> mod_ids = new HashSet<string>();

        public void Start()
        {
            //List<string> achievementKeys = new List<string>(AchievementIDHolder.achievements.Keys);
            ui = transform.GetChild(1).gameObject;
            console = FindObjectsOfType<MSCLoader.ConsoleView>()[0].transform.GetChild(0).gameObject;
            foreach (string s in AchievementIDHolder.achievements.Keys)
            {
                if (!mod_ids.Contains(s)) mod_ids.Add(AchievementIDHolder.achievements[s].mod_id);
            }
            if (mod_text.text == "NULLNULLNULL") GetRandomAchievementKey();
        }
        public void FindOptionsMenu()
        {
            options_menu = GameObject.Find("Systems").transform.Find("OptionsMenu").gameObject;
        }
        /*
        private List<string> GetAllModIDs()
        {
            List<string> achievementKeys = new List<string>();
            foreach (string s in mod_ids)
            {

            }
        }
        */
        public void ButtonPress(int side)
        {
            // Get the list of achievement keys for the given mod_id
            //List<string> achievementKeys = AchievementIDHolder.achievements.Keys.Where(key => AchievementIDHolder.achievements[key].mod_id == mod_id).ToList();

            // Find the index of the current mod_text in the achievementKeys list
            int currentIndex = mod_ids.ToList().IndexOf(currentText);

            // Calculate the index of the next mod_text based on the button press
            int nextIndex = currentIndex + (side == 0 ? -1 : 1);

            // Ensure the nextIndex is within the valid range of indices
            if (nextIndex < 0)
            {
                nextIndex = mod_ids.Count - 1; // Wrap around to the last index
            }
            else if (nextIndex >= mod_ids.Count)
            {
                nextIndex = 0; // Wrap around to the first index
            }

            // Get the next achievement key based on the nextIndex
            string nextAchievementKey = mod_ids.ToList()[nextIndex];
            currentText = nextAchievementKey;
            ah.GenerateAchievementList(nextAchievementKey);
            mod_text.text = nextAchievementKey.ToUpper();
        }
        /*
        public void ButtonPress(int side)
        {
            //get the next achievement key
            string currentModText = mod_text.text;
            string nextAchievementKey = GetNextAchievementKey(currentText, side);
            //update the text
            mod_text.text = AchievementIDHolder.achievements[nextAchievementKey].mod_id.ToUpper();
            ah.GenerateAchievementList(AchievementIDHolder.achievements[nextAchievementKey].mod_id);
        }
        */
        private void GetRandomAchievementKey()
        {
            //List<string> achievementKeys = new List<string>(AchievementIDHolder.achievements.Keys);
            int index = UnityEngine.Random.Range(0, mod_ids.Count);
            //MSCLoader.ModConsole.Error(achievementKeys.Count.ToString());
            string nextAchievementKey = mod_ids.ToList()[index];
            currentText = nextAchievementKey;
            mod_text.text = nextAchievementKey.ToUpper();
            ah.GenerateAchievementList(nextAchievementKey);
        }
        public void ToggleUI()
        {
            ui.SetActive(!enable);
            enable = !enable;
        }
        private void Update()
        {
            if (!onLoad) button.SetActive(!console.activeSelf);
            if (onLoad) button.SetActive(options_menu.activeInHierarchy);
        }
    }
}