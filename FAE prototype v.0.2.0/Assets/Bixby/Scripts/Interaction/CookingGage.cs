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

    [SerializeField] GameObject cookingWindow;
    [SerializeField] GameObject buttonWindow;

    private void Awake()
    {
        
    }
    float tttt;


    // Start is called before the first frame update
    void Start()
    {
        tttt = Time.deltaTime;
        cookingWindow.SetActive(false);
        //cookingWindow = this.transform.parent.GetChild(2).gameObject;
        //buttonWindow = this.transform.parent.GetChild(1).gameObject;
    }

    //������ ������
    itemData cookData;

   
    // Update is called once per frame
    void Update()
    {
        if (start == true)
        {
            //������ �����̴� ����
            gageSlider.value += gageSpeed * tttt;
        }

        if (gageSlider.value == 1)
        {
            success_fail.text = "Fail";
            gageSlider.gameObject.SetActive(false);
            minSlider.gameObject.SetActive(false);
            maxSlider.gameObject.SetActive(false);
            cookingWindow.SetActive(false);
            buttonWindow.SetActive(!cookingWindow.activeSelf);

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
                cookData.Left = -1; cookData.Up = -1; cookData.xSize = 2; cookData.ySize = 2;
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
            cookingWindow.SetActive(false);
            buttonWindow.SetActive(!cookingWindow.activeSelf);
            gageSlider.value = 0;
            start = false;
        }
    }

    //�丮�ϱ� ��ư ������ �� -> Cooking�� �ִ� ������ �޾ƿͼ� ��� �丮�� �ϴ��� �Ǵ��ؾ���
    public void Cooking()
    {
        if (start)
            return;
        //�ʱ�ȭ
        cookData = new itemData();
        //
        List<int> FruitIndex = new List<int>();
        List<int> GreenIndex = new List<int>();
        List<int> RiceIndex = new List<int>();
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
                        FruitIndex.Add(inventoryObject.Inst.items.items.IndexOf(item));
                        break;
                    case 1001:
                        GreenIndex.Add(inventoryObject.Inst.items.items.IndexOf(item));
                        break;
                    case 1002:
                        RiceIndex.Add(inventoryObject.Inst.items.items.IndexOf(item));
                        break;
                    default:
                        break;
                }
            }
        }
        print("�����");
        foreach (var item in FruitIndex)
        {
            print(inventoryObject.Inst.items.items[item].itemName);
            print(inventoryObject.Inst.itemObjects[item].GetComponent<itemObject>().ItemData.itemName);
        }

        cookData.itemName = num.text;

        if (num.text == "�����ֽ�" && FruitIndex.Count >= 2)
        {
            cookData.itemID = 2001;
            cookData.xSize = 1; cookData.ySize = 2;
            //�丮 ��� ����
            //���̴� �Ҹ��ϴ� ��� ��
            for (int i = 0; i < 2; i++)
            {
                inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[FruitIndex[i]], false);
            }

        }
        else if (num.text == "�ѽ�" && GreenIndex.Count >= 1 && RiceIndex.Count >= 1)
        {
            cookData.itemID = 2002;
            cookData.xSize = 2; cookData.ySize = 1;
            //�丮 ��� ����
            inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[GreenIndex[0]], false);
            inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[RiceIndex[0]], false);
            //for (int i = 0; i < 1; i++)
            //{
            //    inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[GreenIndex[i]], false);
            //}
            //for (int i = 0; i < 1; i++)
            //{
            //    inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[RiceIndex[i]], false);
            //}
        }
        else
        {
            success_fail.text = "-";
            return;
        }


        success_fail.text = "-";
        //�丮�����Ҷ� setActiveŰ��
        if (cookingWindow.activeSelf == false)
            cookingWindow.SetActive(true);
        buttonWindow.SetActive(!cookingWindow.activeSelf);
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
