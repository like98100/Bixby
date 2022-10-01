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
    void Start()
    {
        option = optionObj.GetComponent<UI_Option>();
        option.Set();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            optionWindow();
        if(Input.GetKeyDown(KeyCode.I))
            inventory.SetActive(!inventory.activeSelf);
    }
    public void optionWindow()//옵션창 열고 닫기
    {
        if (optionObj.activeSelf)
            option.senseSetup();//닫을때 옵션에서 설정한 감도값 설정
        else
            option.senseStop();//열때 감도값 0으로 설정
        optionObj.SetActive(!optionObj.activeSelf);
    }
}
