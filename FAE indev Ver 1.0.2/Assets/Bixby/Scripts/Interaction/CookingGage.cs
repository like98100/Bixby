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

    itemJsonData itemJsonData;//json������

    GameObject food;
    GameObject material_1;
    GameObject material_2;
    itemObject tempItem;

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "cook");//json�ε�
    }

    bool isInitialized = false;
    float dTime;
    // Start is called before the first frame update
    void Start()
    {
        if (!isInitialized)
        {
            dTime = Time.deltaTime;
        }
        tempItem = Instantiate(inventoryObject.Inst.getObj("itemPrefab"), Vector3.one * -999f, Quaternion.identity).GetComponent<itemObject>();
        food = num.gameObject.transform.parent.gameObject;
        material_1 = food.transform.GetChild(2).gameObject;
        material_2 = food.transform.GetChild(3).gameObject;
        CookInitialize();
        cookingWindow.transform.parent.gameObject.SetActive(false);
        Button closeBtn = this.transform.parent.GetChild(4).GetComponent<Button>();
        closeBtn.onClick.AddListener(() => UI_Control.Inst.windowSet(this.gameObject.transform.parent.gameObject));
    }

    //������ ������
    itemData cookData;

   
    // Update is called once per frame
    void Update()
    {
        if (start == true)
        {
            //������ �����̴� ����
            gageSlider.value += gageSpeed * 0.01f;
        }

        if (gageSlider.value == 1)
        {
            success_fail.text = "Fail";
            SoundManage.instance.PlaySFXSound(7, "System"); // ���� ����

            gageSlider.gameObject.SetActive(false);
            minSlider.gameObject.SetActive(false);
            maxSlider.gameObject.SetActive(false);
            cookingWindow.SetActive(false);
            buttonWindow.SetActive(!cookingWindow.activeSelf);

            gageSlider.value = 0;
            start = false;

            //������ ���� ��ũ��Ʈ
            //���п丮
            foreach (var item in itemJsonData.itemList)
            {
                if (item.itemID == 2000)
                {
                    cookData = item;
                }
            }

            Vector2 tempPos;
            tempPos = inventoryObject.Inst.emptyCell(cookData.xSize, cookData.ySize);
            if (!inventoryObject.Inst.itemGet(cookData.xSize, cookData.ySize, tempPos.x, tempPos.y, cookData))
            {
                inventoryObject.Inst.MakeFieldItem(cookData, GameObject.FindGameObjectWithTag("Player").transform.position);
            }
        }

        //�����̽��� ������ ����, ĳ���� �����̴°� ���ƾ���
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (gageSlider.value >= minSlider.value && gageSlider.value <= maxSlider.value)
            {
                success_fail.text = "Success";

                SoundManage.instance.PlaySFXSound(6, "System"); // ���� ����
            }
            else
            {
                success_fail.text = "Fail";
                SoundManage.instance.PlaySFXSound(7, "System"); // ���� ����
                //������ �丮 ����

                //������ ���� ��ũ��Ʈ
                //���п丮
                foreach (var item in itemJsonData.itemList)
                {
                    if (item.itemID == 2000)
                    {
                        cookData = item;
                    }
                }
            }

            Vector2 tempPos;
            tempPos = inventoryObject.Inst.emptyCell(cookData.xSize, cookData.ySize);
            if (!inventoryObject.Inst.itemGet(cookData.xSize, cookData.ySize, tempPos.x, tempPos.y, cookData))
            {
                inventoryObject.Inst.MakeFieldItem(cookData, GameObject.FindGameObjectWithTag("Player").transform.position);
            }

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
        List<int> FishIndex = new List<int>();
        List<int> MeatIndex = new List<int>();
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
                    case 1003:
                        FishIndex.Add(inventoryObject.Inst.items.items.IndexOf(item));
                        break;
                    case 1004:
                        MeatIndex.Add(inventoryObject.Inst.items.items.IndexOf(item));
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

        if (num.text == "�丶�� �ֽ�" && FruitIndex.Count >= 2)
        {
            foreach (var item in itemJsonData.itemList)
            {
                if (item.itemID == 2001)
                {
                    cookData = item;
                }
            }
            //�丮 ��� ����
            //���̴� �Ҹ��ϴ� ��� ��
            for (int i = 1; i > -1; i--)
            {
                inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[FruitIndex[i]], false);
            }
        }
        else if (num.text == "��ä��" && GreenIndex.Count >= 1 && RiceIndex.Count >= 1)
        {
            foreach (var item in itemJsonData.itemList)
            {
                if (item.itemID == 2002)
                {
                    cookData = item;
                }
            }
            //�丮 ��� ����
            inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[GreenIndex[0]], false);
            inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[RiceIndex[0]], false);
        }
        else if (num.text == "���� ����" && FishIndex.Count >= 2)
        {
            foreach (var item in itemJsonData.itemList)
            {
                if (item.itemID == 2003)
                {
                    cookData = item;
                }
            }
            //�丮 ��� ����
            for (int i = 1; i > -1; i--)
            {
                inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[FishIndex[i]], false);
            }
        }else if (num.text == "���� ���" && MeatIndex.Count >= 2)
        {
            foreach (var item in itemJsonData.itemList)
            {
                if (item.itemID == 2004)
                {
                    cookData = item;
                }
            }
            //�丮 ��� ����
            for (int i = 1; i > -1; i--)
            {
                inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[MeatIndex[i]], false);
            }
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

        SoundManage.instance.PlaySFXSound(4, "System"); // �丮 ����
    }

    public void InitializeImage()
    {
        food.SetActive(true);
        int itemId = -1;
        foreach (var item in itemJsonData.itemList)
        {
            if (item.itemName == num.text)
            {
                itemId = item.itemID;
                break;
            }
        }
        if (itemId >= 1000 && itemId < 2000)
            food.transform.GetChild(1).GetComponent<Image>().sprite = tempItem.GetSprites()[itemId - 1000];
        else if (itemId > 2000 && itemId < 3000)
            food.transform.GetChild(1).GetComponent<Image>().sprite = tempItem.GetSprites()[itemId - 2000 + 4];
        if (itemId == 2001)
            food.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 200f);
        else
            food.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = Vector2.one * 200f;
        switch (itemId)
        {
            case 2001:
                material_1.transform.GetChild(0).GetComponent<Text>().text
                = material_2.transform.GetChild(0).GetComponent<Text>().text
                = "�丶��";
                material_1.transform.GetChild(1).GetComponent<Image>().sprite
                = material_2.transform.GetChild(1).GetComponent<Image>().sprite 
                = tempItem.GetSprites()[0];
                break;
            case 2002:
                material_1.transform.GetChild(0).GetComponent<Text>().text = "����";
                material_2.transform.GetChild(0).GetComponent<Text>().text = "��";
                material_1.transform.GetChild(1).GetComponent<Image>().sprite = tempItem.GetSprites()[1];
                material_2.transform.GetChild(1).GetComponent<Image>().sprite = tempItem.GetSprites()[2];
                break;
            case 2003:
                material_1.transform.GetChild(0).GetComponent<Text>().text
                = material_2.transform.GetChild(0).GetComponent<Text>().text
                = "����";
                material_1.transform.GetChild(1).GetComponent<Image>().sprite
                = material_2.transform.GetChild(1).GetComponent<Image>().sprite
                = tempItem.GetSprites()[3];
                break;
            case 2004:
                material_1.transform.GetChild(0).GetComponent<Text>().text
                = material_2.transform.GetChild(0).GetComponent<Text>().text
                = "���°��";
                material_1.transform.GetChild(1).GetComponent<Image>().sprite
                = material_2.transform.GetChild(1).GetComponent<Image>().sprite
                = tempItem.GetSprites()[4];
                break;
            default:
                break;
        }
        success_fail.text = "";
    }
    public void CookInitialize()
    {
        num.text = "-";
        success_fail.text = "-";
        food.SetActive(false);
        cookingWindow.SetActive(false);
    }
}
