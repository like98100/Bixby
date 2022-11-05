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

    public enum COOK
    {
        NONE = 0,
        FRUITJUICE = 1,
        KOREANFOOD = 2,
    }


    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�丮 â ���� -> ui���̶� ���� �� �� ������Ʈ��.
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f)
        {
            //fŰ ����
            if (inventoryObject.Inst.FieldFKey == null)
            {
                inventoryObject.Inst.FieldFKey = Instantiate(inventoryObject.Inst.getObj("KeyF"), GameObject.Find("Canvas").transform);
                var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
                inventoryObject.Inst.FieldFKey.transform.position = wantedPos + Vector3.right * 200f;
            }


            if (Input.GetKeyDown(KeyCode.F))
            {
                //UIâ�� ��������
                if (cookPanel.gameObject.activeSelf == true)
                {
                    cookPanel.gameObject.SetActive(false);
                }
                else
                {
                    cookPanel.gameObject.SetActive(true);
                    Destroy(inventoryObject.Inst.FieldFKey);
                    inventoryObject.Inst.FieldFKey = null;
                }
            }
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
            num.text = "�ѽ�";
        }
    }
}
