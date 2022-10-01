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
    public void optionWindow()//�ɼ�â ���� �ݱ�
    {
        if (optionObj.activeSelf)
            option.senseSetup();//������ �ɼǿ��� ������ ������ ����
        else
            option.senseStop();//���� ������ 0���� ����
        optionObj.SetActive(!optionObj.activeSelf);
    }
}
