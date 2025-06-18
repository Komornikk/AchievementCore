using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
class CanvasController : MonoBehaviour
{

    private GameObject disableUI, options_menu, console;
    public Scrollbar slider;
    public AchievementHandler ah;
    public GameObject ui;
    public Text mod_text;
    internal AudioClip boltscrew;
    private bool enable = false;
    public bool onLoad = false;
    string currentText = "";
    public HashSet<string> mod_ids = new HashSet<string>();

    public void Start()
    {
        GameObject[] g = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject go in g)
        {
            if (go.name.Equals("MSCLoader Console"))
            {
                console = go.transform.GetChild(0).gameObject;
                break;
            }
        }
        //console = FindObjectsOfType<MSCLoader.ConsoleView>()[0].transform.GetChild(0).gameObject;
        disableUI = transform.GetChild(0).gameObject;
    }
    public void FindOptionsMenu()
    {
        options_menu = GameObject.Find("Systems").transform.Find("OptionsMenu").gameObject;
    }
    public void ButtonPress(int side)
    {
        if (mod_ids.Count == 1)
        {
            //no need to execute further if it's just one mod
            return;
        }
        //get the current index
        int currentIndex = mod_ids.ToList().IndexOf(currentText);

        //get the next index
        int nextIndex = currentIndex + (side == 0 ? -1 : 1);

        //look if index is within the id count
        if (nextIndex < 0)
        {
            nextIndex = mod_ids.Count - 1; //wrap if index exceedes the id count
        }
        else if (nextIndex >= mod_ids.Count)
        {
            nextIndex = 0; //wrap to 0 if index exceedes the id count
        }

        //get next achievement
        string nextAchievementKey = mod_ids.ToList()[nextIndex];
        currentText = nextAchievementKey;
        ah.GenerateAchievementList(nextAchievementKey);
        mod_text.text = nextAchievementKey.ToUpper();
        slider.value = 1f;
        AudioSource.PlayClipAtPoint(boltscrew, Camera.main.transform.position, 0.4f);
    }
    public void GenerateBaseAchievementKey()
    {
        string nextAchievementKey = mod_ids.ToList()[mod_ids.ToList().IndexOf("base")];
        currentText = nextAchievementKey;
        mod_text.text = nextAchievementKey.ToUpper();
        ah.GenerateAchievementList(nextAchievementKey);
    }
    public void ToggleUI()
    {
        ui.SetActive(!enable);
        enable = !enable;
        AudioSource.PlayClipAtPoint(boltscrew, Camera.main.transform.position, 0.4f);
    }
    private void Update()
    {
        if (!onLoad) disableUI.SetActive(!console.activeSelf);
        if (onLoad && !console.activeSelf) disableUI.SetActive(options_menu.activeInHierarchy);
        else if (onLoad && console.activeSelf) disableUI.SetActive(!console.activeSelf);
    }
}