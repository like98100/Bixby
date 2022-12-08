using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    GameObject missionWindow;
    TMPro.TextMeshProUGUI missionTitle;
    TMPro.TextMeshProUGUI missionText;
    void Awake()
    {
        missionWindow = GameObject.Find("Mission");
        missionTitle = missionWindow.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        missionText = missionWindow.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
    }
    private void Start()
    {
        QuestObject.manager.MissionSet();
    }
    // Update is called once per frame
    void Update()
    {
        if (missionTitle.text == "")
            return;
        if (UI_Control.Inst.OpenedWindow == null)
            missionWindow.SetActive(true);
        else
            missionWindow.SetActive(!(UI_Control.Inst.OpenedWindow.name == "Inventory"
                                    || UI_Control.Inst.OpenedWindow.name == "Shop"
                                    || UI_Control.Inst.OpenedWindow.name == "Speech"
                                    || UI_Control.Inst.OpenedWindow.transform.parent.gameObject.name == "COOK"));
    }
    public void misssionSet(string missionTitle, string missionText)
    {
        this.missionTitle.text = missionTitle;
        this.missionText.text = missionText;
    }
}
