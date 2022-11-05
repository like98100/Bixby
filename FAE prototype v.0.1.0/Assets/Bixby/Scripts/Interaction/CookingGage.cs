using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CookingGage : MonoBehaviour
{
    public Slider gageSlider; //������ �����̴�
    public Slider minSlider; //�ּҰ� �����̴�
    public Slider maxSlider; //�ִ밪 �����̴�


    public Text success_fail; //���� ���� �ؽ�Ʈ ���߿� ����
    public Text num; //���� �丮Text

    public int thisCook; //� �丮����


    public bool cookingState; //�丮�� �������� �ƴ���

    public bool start = false; //�����ߴ��� �ƴ���
    public float gageSpeed = 1.0f; //������ �ӵ�
    public float min_min = 0.4f; //�ּҰ��� �ּҰ�
    public float min_max = 0.7f; //�ּҰ��� �ִ밪
    public float sliderInterval = 0.2f; //�ּҰ� �ִ밪 ���� ����

    

    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
       
    }

    //������ ������
    itemData cookData;

    // Update is called once per frame
    void Update()
    {


        if (start == true)
        {
            //������ �����̴� ����
            gageSlider.value += gageSpeed * Time.deltaTime;
        }

        if (gageSlider.value == 1)
        {
            success_fail.text = "Fail";
            gageSlider.gameObject.SetActive(false);
            minSlider.gameObject.SetActive(false);
            maxSlider.gameObject.SetActive(false);
            gageSlider.value = 0;
            start = false;

            //������ ���� ��ũ��Ʈ
            cookData.itemID = 2003; cookData.tag = new string[] { "food", "cooked" }; cookData.itemName = "������ �丮";
            cookData.Left = -1; cookData.Up = -1; cookData.xSize = 3; cookData.ySize = 3;
            cookData.isEquip = false; cookData.isSell = false;
            cookData.price = 2; //���߿� ���� ����

            Vector2 tempPos;
            tempPos = inventoryObject.Inst.emptyCell(cookData.xSize, cookData.ySize);
            inventoryObject.Inst.itemGet(cookData.xSize, cookData.ySize, tempPos.x, tempPos.y, cookData);
        }

        //�����̽��� ������ ����, ĳ���� �����̴°� ���ƾ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gageSlider.value >= minSlider.value && gageSlider.value <= maxSlider.value)
            {
                success_fail.text = "Success";
                //������ �丮 ����

                
                //������ ���� ��ũ��Ʈ
                cookData.tag = new string[] { "food", "cooked" }; 
                cookData.Left = -1; cookData.Up = -1; 
                cookData.isEquip = false; cookData.isSell = false;
                cookData.price = 2; //���߿� ���� ����
            }
            else
            {
                success_fail.text = "Fail";
                //������ �丮 ����

                //������ ���� ��ũ��Ʈ
                cookData.itemID = 2003; cookData.tag = new string[] { "food", "cooked" }; cookData.itemName = "������ �丮";
                cookData.Left = -1; cookData.Up = -1; cookData.xSize = 3; cookData.ySize = 3;
                cookData.isEquip = false; cookData.isSell = false;
                cookData.price = 2; //���߿� ���� ����
            }

            Vector2 tempPos;
            tempPos = inventoryObject.Inst.emptyCell(cookData.xSize, cookData.ySize);
            inventoryObject.Inst.itemGet(cookData.xSize, cookData.ySize, tempPos.x, tempPos.y, cookData);

            //���ð� ��� �ְ� �����̴� ����
            gageSlider.gameObject.SetActive(false);
            minSlider.gameObject.SetActive(false);
            maxSlider.gameObject.SetActive(false);
            gageSlider.value = 0;
            start = false;
        }
    }

    //�丮�ϱ� ��ư ������ �� -> Cooking�� �ִ� ������ �޾ƿͼ� ��� �丮�� �ϴ��� �Ǵ��ؾ���
    public void Cooking()
    {
        //�ʱ�ȭ
        cookData = new itemData();
        //
        int FruitCount = 0;
        int GreenonionCount = 0;
        int RiceCount = 0;

        foreach (var item in inventoryObject.Inst.items.items)
        {
            if (item.tag.Length != 2)
            {
                continue;
            }
            if (item.tag[1] == "harvest")
            {
                switch (item.itemID)
                {
                    case 1000:
                        FruitCount++;
                        break;
                    case 1001:
                        GreenonionCount++;
                        break;
                    case 1002:
                        RiceCount++;
                        break;
                    default:
                        break;
                }
            }
        }
        

        cookData.itemName = num.text;

        if (num.text == "�����ֽ�" && FruitCount >= 2)
        {
            cookData.itemID = 2001;
            cookData.xSize = 1; cookData.ySize = 2;
            //�丮 ��� ����
            //���̴� �Ҹ��ϴ� ��� ��
            for (int i = 0; i < 2; i++)
            {
                foreach (var item in inventoryObject.Inst.itemObjects)
                {
                    if (item.tag.Length != 2)
                    {
                        continue;
                    }

                    if (item.GetComponent<itemObject>().ItemData.tag[1] == "harvest" && item.GetComponent<itemObject>().ItemData.itemID == 1000)
                    {
                        FruitCount--;
                        inventoryObject.Inst.throwItem(item, false);
                        break;
                    }
                }
            }
        }
        else if (num.text == "�ѽ�" && GreenonionCount >= 1 && RiceCount >= 1)
        {
            cookData.itemID = 2002;
            cookData.xSize = 2; cookData.ySize = 1;
            //�丮 ��� ����
            foreach (var item in inventoryObject.Inst.itemObjects)
            {
                if (item.tag.Length != 2)
                {
                    continue;
                }

                if (item.GetComponent<itemObject>().ItemData.tag[1] == "harvest" && item.GetComponent<itemObject>().ItemData.itemID == 1001)
                {
                    GreenonionCount--;
                    inventoryObject.Inst.throwItem(item, false);
                    break;
                }
            }
            foreach (var item in inventoryObject.Inst.itemObjects)
            {
                if (item.tag.Length != 2)
                {
                    continue;
                }

                if (item.GetComponent<itemObject>().ItemData.tag[1] == "harvest" && item.GetComponent<itemObject>().ItemData.itemID == 1002)
                {
                    RiceCount--;
                    inventoryObject.Inst.throwItem(item, false);
                    break;
                }
            }
        }
        else
        {
            success_fail.text = "-";
            return;
        }


        success_fail.text = "-";
        //�丮�����Ҷ� setActiveŰ��
        if (gageSlider.gameObject.activeSelf == false)
        {
            gageSlider.gameObject.SetActive(true);
        }
        if (minSlider.gameObject.activeSelf == false)
        {
            minSlider.gameObject.SetActive(true);

            //�������� �� �ֱ�
            minSlider.value = Random.Range(min_min, min_max);
            maxSlider.value = minSlider.value + sliderInterval;
        }
        if (maxSlider.gameObject.activeSelf == false)
        {
            maxSlider.gameObject.SetActive(true);
        }
        //�丮����
        start = true;
    }
}
