using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Control : MonoBehaviour
{
    public static UI_Control Inst { get; private set; }
    private void Awake() => Inst = this;
    [SerializeField] GameObject optionObj;
    public UI_Option Option;
    [SerializeField] GameObject inventory;
    public Speech Speech;
    public GameObject Map;
    [SerializeField] List<GameObject> windows;
    public GameObject OpenedWindow;
    [SerializeField] GameObject aimPoint;
    void Start()
    {
        Option.Set();
        Map.SetActive(false);
        OpenedWindow = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OptionWindow();
        if (Input.anyKeyDown)
        {
            string temp = Input.inputString;

            switch (temp)
            {
                case "i":
                    WindowSet(inventory);
                    break;
                case "m":
                    WindowSet(Map);
                    break;
            }
        }
        if (OpenedWindow == null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            aimPoint.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            aimPoint.SetActive(false);
        }
    }
    public void OptionWindow()
    {
        foreach (var item in windows)//¿œ¥‹ √¢ ¥›±‚
        {
            if (item.activeSelf)
            {
                WindowSet(item);
                return;
            }
        }
        WindowSet(optionObj);
    }
    public void WindowSet(GameObject window)//√¢ ø≠∞Ì ¥›±‚
    {
        if (!(OpenedWindow == null || OpenedWindow == window))
            return;
        window.SetActive(!window.activeSelf);
        Option.senseSet(window.activeSelf);
        if (window.activeSelf)
            OpenedWindow = window;
        else
            OpenedWindow = null;
    }
}
