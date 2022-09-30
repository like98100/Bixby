using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Control : MonoBehaviour
{
    [SerializeField] GameObject optionObj;
    UI_Option option;
    [SerializeField] GameObject inventory;
    void Start()
    {
        optionObj.SetActive(false);
        option = optionObj.GetComponent<UI_Option>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            optionWindow();
        if(Input.GetKeyDown(KeyCode.I))
            inventory.SetActive(!inventory.activeSelf);
    }
    public void optionWindow()
    {
        if (optionObj.activeSelf)
            option.senseSetup();
        else
            option.senseStop();
        optionObj.SetActive(!optionObj.activeSelf);
    }
}
