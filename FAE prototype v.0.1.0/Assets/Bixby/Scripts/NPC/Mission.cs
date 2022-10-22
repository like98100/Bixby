using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    GameObject missionWindow;
    UnityEngine.UI.Text missionTitle;
    UnityEngine.UI.Text missionText;
    void Start()
    {
        missionWindow = GameObject.Find("Mission");
        missionTitle = missionWindow.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        missionText = missionWindow.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>();
        missionTitle.text = "테스트 이름";
        missionText.text = "테스트용 내용";
    }

    // Update is called once per frame
    void Update()
    {
        if (UI_Control.Inst.OpenedWindow == null)
            missionWindow.SetActive(true);
        else
            missionWindow.SetActive(!(UI_Control.Inst.OpenedWindow.name == "Inventory"
                                    || UI_Control.Inst.OpenedWindow.name == "Shop"));
    }
}
