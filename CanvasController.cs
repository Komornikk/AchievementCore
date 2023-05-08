using System.Collections;
using UnityEngine;

namespace AchievementCore
{
    public class CanvasController : MonoBehaviour
    {

        private GameObject button, options_menu, console;
        public GameObject ui;
        private bool enable = false;
        public bool onLoad = false;

        private void Start()
        {
            button = transform.GetChild(0).gameObject;
            ui = transform.GetChild(1).gameObject;
            console = FindObjectsOfType<MSCLoader.ConsoleView>()[0].transform.GetChild(0).gameObject;
        }
        public void FindOptionsMenu()
        {
            options_menu = GameObject.Find("Systems").transform.Find("OptionsMenu").gameObject;
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