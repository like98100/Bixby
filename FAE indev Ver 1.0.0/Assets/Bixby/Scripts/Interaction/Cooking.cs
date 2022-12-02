using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooking : MonoBehaviour
{
    //�����̸� �ٲ����
    public Text num;
    public int myNum; //� �丮����

    public GameObject cookPanel;

    public GameObject Player; //�÷��̾� ������Ʈ
    bool isPlayerClose;
    public enum COOK
    {
        NONE = 0,
        FRUITJUICE = 1,
        KOREANFOOD = 2,
    }
    GameObject notify;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        isPlayerClose = false;
        notify = this.GetComponent<UnityEngine.UI.Button>() ? null : this.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (notify != null)
        {
            Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
            camRotate += Vector3.right * 90f + Vector3.forward * 180f;
            notify.transform.rotation = Quaternion.Euler(camRotate);
        }
        //�丮 â ���� -> ui���̶� ���� �� �� ������Ʈ��.
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f)
        {
            //fŰ ����
            if (!inventoryObject.Inst.FieldFKey.activeSelf&&!isPlayerClose)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
                isPlayerClose = true;
            }
            if (isPlayerClose)
            {
                var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
                inventoryObject.Inst.FieldFKey.transform.position = wantedPos + Vector3.right * 200f;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                UI_Control.Inst.windowSet(cookPanel);
                inventoryObject.Inst.FieldFKey.SetActive(false);
                ////UIâ�� ��������
                //if (cookPanel.gameObject.activeSelf == true)
                //{
                //    cookPanel.gameObject.SetActive(false);
                //}
                //else
                //{
                //    cookPanel.gameObject.SetActive(true);
                //    Destroy(inventoryObject.Inst.FieldFKey);
                //    inventoryObject.Inst.FieldFKey = null;
                //}
            }
        }
        else if(isPlayerClose)
        {
            inventoryObject.Inst.FieldFKey.SetActive(false);
            isPlayerClose = false;
        }
            
    }

  
    //�����ؾ���
    public void openCook()
    {
        //�丮 UI �ش��ϴ°� �߰�

        if (myNum == 1)
        {
            num.text = "�����ֽ�";
        }
        else if (myNum == 2)
        {
            num.text = "��ä��";
        }
    }
}
