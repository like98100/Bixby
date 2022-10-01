using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Control : MonoBehaviour
{
    public static UI_Control Inst { get; private set; }
    private void Awake() => Inst = this;
    [SerializeField] GameObject optionObj;
    public UI_Option option;
    [SerializeField] GameObject inventory;
    public Speech speech;
    public GameObject map;
    [SerializeField] List<GameObject> windows;
    GameObject openedWindow;
    void Start()
    {
        option.Set();
        map.SetActive(false);
        openedWindow = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            optionWindow();
        if (Input.anyKeyDown)
        {
            string temp = Input.inputString;
            
            switch (temp)
            {
                case "i":
                    windowSet(inventory);
                    break;
                case "m":
                    windowSet(map);
                    break;
            }
        }

    }
    public void optionWindow()//옵션창 열고 닫기
    {
        foreach (var item in windows)//일단 창 닫기
        {
            if (item.activeSelf)
            {
                windowSet(item);
                return;
            }
        }
        windowSet(optionObj);
    }

    public void windowSet(GameObject window)//창 열고 닫기
    {
        if (!(openedWindow == null || openedWindow == window))
            return;
        window.SetActive(!window.activeSelf);
        option.senseSet(window.activeSelf);
        if (window.activeSelf)
            openedWindow = window;
        else
            openedWindow = null;
    }
}
