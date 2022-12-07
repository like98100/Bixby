using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CookingGage : MonoBehaviour
{
    public Slider gageSlider; //게이지 슬라이더
    public Slider minSlider; //최소값 슬라이더
    public Slider maxSlider; //최대값 슬라이더


    public Text success_fail; //성공 실패 텍스트 나중에 변경
    public Text num; //현재 요리Text

    public int thisCook; //어떤 요리인지


    public bool cookingState; //요리가 가능한지 아닌지

    public bool start = false; //시작했는지 아닌지
    public float gageSpeed = 1.0f; //게이지 속도
    public float min_min = 0.4f; //최소값의 최소값
    public float min_max = 0.7f; //최소값의 최대값
    public float sliderInterval = 0.2f; //최소값 최대값 사이 길이

    [SerializeField] GameObject cookingWindow;
    [SerializeField] GameObject buttonWindow;

    itemJsonData itemJsonData;//json데이터

    GameObject food;
    GameObject material_1;
    GameObject material_2;
    itemObject tempItem;

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "cook");//json로드
    }
    float dtime;


    // Start is called before the first frame update
    void Start()
    {
        dtime = Time.deltaTime;
        tempItem = Instantiate(inventoryObject.Inst.getObj("itemPrefab"), Vector3.one * -999f, Quaternion.identity).GetComponent<itemObject>();
        food = num.gameObject.transform.parent.gameObject;
        material_1 = food.transform.GetChild(2).gameObject;
        material_2 = food.transform.GetChild(3).gameObject;
        CookInitialize();
        cookingWindow.transform.parent.gameObject.SetActive(false);
    }

    //아이템 데이터
    itemData cookData;

   
    // Update is called once per frame
    void Update()
    {
        if (start == true)
        {
            //게이지 슬라이더 증가
            gageSlider.value += gageSpeed * dtime;
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

            //아이템 지정 스크립트
            //실패요리
            foreach (var item in itemJsonData.itemList)
            {
                if (item.itemID == 2003)
                {
                    cookData = item;
                }
            }

            Vector2 tempPos;
            tempPos = inventoryObject.Inst.emptyCell(cookData.xSize, cookData.ySize);
            inventoryObject.Inst.itemGet(cookData.xSize, cookData.ySize, tempPos.x, tempPos.y, cookData);
        }

        //스페이스바 누르면 멈춤, 캐릭터 움직이는거 막아야함
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gageSlider.value >= minSlider.value && gageSlider.value <= maxSlider.value)
            {
                success_fail.text = "Success";
            }
            else
            {
                success_fail.text = "Fail";
                //실패한 요리 생성

                //아이템 지정 스크립트
                //실패요리
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
            inventoryObject.Inst.itemGet(cookData.xSize, cookData.ySize, tempPos.x, tempPos.y, cookData);

            //대기시간 잠시 주고 슬라이더 삭제
            gageSlider.gameObject.SetActive(false);
            minSlider.gameObject.SetActive(false);
            maxSlider.gameObject.SetActive(false);
            cookingWindow.SetActive(false);
            buttonWindow.SetActive(!cookingWindow.activeSelf);
            gageSlider.value = 0;
            start = false;
        }
    }

    //요리하기 버튼 눌렀을 때 -> Cooking에 있는 변수를 받아와서 어던 요리를 하는지 판단해야함
    public void Cooking()
    {
        if (start)
            return;
        //초기화
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
        print("실행됨");
        foreach (var item in FruitIndex)
        {
            print(inventoryObject.Inst.items.items[item].itemName);
            print(inventoryObject.Inst.itemObjects[item].GetComponent<itemObject>().ItemData.itemName);
        }

        cookData.itemName = num.text;

        if (num.text == "토마토 주스" && FruitIndex.Count >= 2)
        {
            foreach (var item in itemJsonData.itemList)
            {
                if (item.itemID == 2001)
                {
                    cookData = item;
                }
            }
            //요리 재료 제거
            //길이는 소모하는 재료 수
            for (int i = 1; i > -1; i--)
            {
                inventoryObject.Inst.throwItem(inventoryObject.Inst.itemObjects[FruitIndex[i]], false);
            }
        }
        else if (num.text == "야채죽" && GreenIndex.Count >= 1 && RiceIndex.Count >= 1)
        {
            foreach (var item in itemJsonData.itemList)
            {
                if (item.itemID == 2002)
                {
                    cookData = item;
                }
            }
            //요리 재료 제거
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
        //요리시작할때 setActive키기
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

            //랜덤으로 값 주기
            minSlider.value = Random.Range(min_min, min_max);
            maxSlider.value = minSlider.value + sliderInterval;
        }
        if (maxSlider.gameObject.activeSelf == false)
        {
            maxSlider.gameObject.SetActive(true);
        }
        //요리시작
        start = true;
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
                = "토마토";
                material_1.transform.GetChild(1).GetComponent<Image>().sprite
                = material_2.transform.GetChild(1).GetComponent<Image>().sprite 
                = tempItem.GetSprites()[0];
                break;
            case 2002:
                material_1.transform.GetChild(0).GetComponent<Text>().text = "양파";
                material_2.transform.GetChild(0).GetComponent<Text>().text = "쌀";
                material_1.transform.GetChild(1).GetComponent<Image>().sprite = tempItem.GetSprites()[1];
                material_2.transform.GetChild(1).GetComponent<Image>().sprite = tempItem.GetSprites()[2];
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
