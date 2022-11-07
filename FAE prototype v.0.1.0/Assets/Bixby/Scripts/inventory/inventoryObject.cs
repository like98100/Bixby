using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryObject : MonoBehaviour
{
    public static inventoryObject Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
        items = (itemSO)ScriptableObject.CreateInstance("itemSO");
        //itemPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Bixby/Prefab/UI/item.prefab", typeof(GameObject)) as GameObject;
        inventoryCanvas = GameObject.Find("Inventory");
        inventoryObj = inventoryCanvas.transform.GetChild(0).gameObject;
        //cellObj = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Bixby/Prefab/UI/cell.prefab", typeof(GameObject)) as GameObject;
        goldObj = inventoryCanvas.transform.GetChild(1).gameObject;
        //fieldItemPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Bixby/Prefab/Item/fieldItem.prefab", typeof(GameObject)) as GameObject;
        itemSummary = inventoryCanvas.transform.GetChild(2).gameObject;
        itemDescription = inventoryCanvas.transform.GetChild(3).gameObject;
        closeBtn = inventoryCanvas.transform.GetChild(4).gameObject.GetComponent<Button>();
        closeBtn.onClick.AddListener(() => UI_Control.Inst.windowClose());
    }
    //�̰� �����
    public itemSO items;//itemSO, �κ��丮 �� �����۵��� �� ���� items�� �����صд�
    [SerializeField] GameObject itemPrefab;//������ ������, �κ��丮 ����, Ȥ�� ������ ȹ���Ҷ� �ν���Ʈ�� ���
    itemJsonData itemJsonData;//json������
    GameObject inventoryCanvas;
    GameObject inventoryObj;
    [SerializeField] GameObject cellObj;
    GameObject goldObj;
    public float Cell;
    public float XSize;
    public float YSize;
    //�̰� �����
    public List<GameObject> itemObjects;
    Vector3 zero;
    public int Gold;
    [SerializeField] GameObject fieldItemPrefab;
    public GameObject FieldFKey;
    [SerializeField] GameObject keyF;
    GameObject itemSummary;
    GameObject itemDescription;
    Button closeBtn;
    void Start()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "items");//json�ε�
        Gold = itemJsonData.gold;//��� �� �ε�
        goldObj.GetComponent<RectTransform>().sizeDelta = new Vector2(XSize * Cell, 50f);//���â ũ��
        goldObj.transform.localPosition = new Vector3(0f, -1 * (inventoryObj.GetComponent<RectTransform>().rect.height / 2f + 25f), 0f);//���â ��ġ
        goldSet();
        items.items = new List<itemData>();
        foreach (var item in itemJsonData.itemList)
            items.items.Add(item);//itemSO ������ �߰�

        inventoryObj.GetComponent<RectTransform>().sizeDelta = new Vector2(XSize * Cell, YSize * Cell);//�κ�â ũ��
        closeBtn.gameObject.transform.position
            = inventoryObj.transform.position
            + Vector3.up * (inventoryObj.GetComponent<RectTransform>().rect.height / 2f + Cell / 2f)
            + Vector3.right * (inventoryObj.GetComponent<RectTransform>().rect.width / 2f + Cell / 2f);
        zero = new Vector3(XSize * Cell / -2f, YSize * Cell / 2f, 0f);//json����� ��ǥ 0,0�� ���� ��ġ
        for (int i = 0; i < YSize; i++)
        {
            for (int j = 0; j < XSize; j++)
            {
                GameObject temp = Instantiate(cellObj, inventoryObj.transform);
                temp.GetComponent<RectTransform>().sizeDelta = new Vector2(Cell * 0.9f, Cell * 0.9f);
                temp.transform.localPosition = new Vector3(j * Cell + (zero.x + Cell / 2f), i * -Cell + (zero.y - Cell / 2f), 0f);
            }
        }//ĭ ����
        itemObjects = new List<GameObject>();
        foreach (var item in items.items)
        {
            GameObject temp = Instantiate(itemPrefab, inventoryCanvas.transform);
            itemObject tempItem = temp.GetComponent<itemObject>();
            tempItem.Setup(item.xSize, item.ySize, item.Left, item.Up, item.isEquip, zero, item);
            itemObjects.Add(temp);
        }//������ ����ȭ
        inventoryCanvas.SetActive(false);//���۽� �κ�â ��������
        FieldFKey = Instantiate(keyF, GameObject.Find("Canvas").transform);
        FieldFKey.SetActive(false);
        itemSummary.SetActive(false);
        itemDescription.SetActive(false);
    }
    private void Update()
    {
        if (FieldFKey.activeSelf)
        {
            Vector3 wantedPos = Camera.main.WorldToScreenPoint(GameObject.FindGameObjectWithTag("Player").transform.position);
            FieldFKey.transform.position = wantedPos + Vector3.right * 200f + Vector3.up * 200f;
        }
    }
    public void setItemPos(GameObject item, Vector3 newOrigonPos)//�κ� �� ������ ��ġ �̵��� ����(��ħȮ��)
    {
        itemObject itemObj = item.GetComponent<itemObject>();
        float left = ((newOrigonPos.x - item.GetComponent<RectTransform>().rect.width / 2f) / Cell) + (XSize / 2f);
        float up = ((newOrigonPos.y + item.GetComponent<RectTransform>().rect.height / 2f) / Cell) - (YSize / 2f);
        up *= -1f;
        int itemIndex = items.items.IndexOf(itemObj.ItemData);
        foreach (var Item in items.items)
        {
            if (items.items.IndexOf(Item) != itemIndex)
            {
                if (isCrash(Item.Left, Item.xSize, left, itemObj.ItemData.xSize, Item.Up, Item.ySize, up, itemObj.ItemData.ySize))
                {
                    item.transform.localPosition = item.GetComponent<itemObject>().OriginPos;
                    return;
                }
            }
        }
        item.transform.localPosition = item.GetComponent<itemObject>().OriginPos = newOrigonPos;
        foreach (var Item in items.items)
        {
            if (items.items.IndexOf(Item) == itemIndex)
            {
                Item.Left = left;
                Item.Up = up;
            }
        }
        jsonSave();
    }
    bool isCrash(float leftA, float xSizeA, float leftB, float xSizeB, float upA, float ySizeA, float upB, float ysizeB)//�浹Ȯ��,���� �������� A
    {
        float tempL = leftA < leftB ? leftB : leftA;
        float tempR = leftA + xSizeA > leftB + xSizeB ? leftB + xSizeB : leftA + xSizeA;
        float tempU = upA < upB ? upB : upA;
        float tempD = upA + ySizeA > upB + ysizeB ? upB + ysizeB : upA + ySizeA;
        return tempL < tempR && tempU < tempD;
    }
    //�̰� �����
    public void jsonSave()//json����
    {
        itemJsonData temp = new itemJsonData();
        temp.itemList = new List<itemData>();
        foreach (var Item in items.items)
        {
            temp.itemList.Add(Item);
        }
        goldSet();
        temp.gold = this.Gold;
        string tempStr = json.ObjectToJson(temp);
        json.CreateJsonFile(Application.dataPath, "items", tempStr);
    }
    public void getFieldItem(GameObject newItem)//�ʵ忡�� �ű� ������ ȹ��
    {
        itemData newData = newItem.GetComponent<fieldItem>().ItemData;
        float newXSize = newData.xSize;
        float newYSize = newData.ySize;
        Vector2 tempPos = emptyCell(newXSize, newYSize);
        if (tempPos != Vector2.zero - Vector2.one)
        {
            itemGet(newXSize, newYSize, tempPos.x, tempPos.y, newData);
            Destroy(newItem);
            jsonSave();
            FieldFKey.SetActive(false);
        }
        //�˸�â ����� else���� ������ ȹ�� �Ұ� �˸� ��
    }
    public List<Vector2> existCells(List<itemData> itemList)//������ ����Ʈ �� ������ ���� ĭ Ȯ��
    {
        List<Vector2> result = new List<Vector2>();
        foreach (var item in itemList)
        {
            List<Vector2> existItem = new List<Vector2>();
            for (float j = item.Up; j <= item.Up + item.ySize; j++)
            {
                for (float i = item.Left; i <= item.Left + item.xSize; i++)
                {
                    existItem.Add(new Vector2(i, j));
                }
            }
            foreach (var cells in existItem)
            {
                if (existItem.Contains(new Vector2(cells.x, cells.y))
                    && existItem.Contains(new Vector2(cells.x + 1f, cells.y))
                    && existItem.Contains(new Vector2(cells.x, cells.y + 1f))
                    && existItem.Contains(new Vector2(cells.x + 1f, cells.y + 1f)))
                    result.Add(cells);
            }
        }
        return result;
    }
    public void throwItem(GameObject itemObj, bool isLeft)//�κ��丮���� ������ ���� ��
    {
        items.items.Remove(itemObj.GetComponent<itemObject>().ItemData);
        itemObjects.Remove(itemObj.gameObject);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject temp = isLeft ? MakeFieldItem(itemObj.GetComponent<itemObject>().ItemData, player.transform.position) : null;
        Destroy(itemObj);
        jsonSave();
        itemSummary.SetActive(false);
    }
    public GameObject MakeFieldItem(itemData data, Vector3 position)//�ʵ忡 ������ ����
    {
        GameObject temp = Instantiate(fieldItemPrefab, position, Quaternion.identity);
        temp.GetComponent<fieldItem>().setup(data);
        return temp;
    }
    public void itemGet(float itemSizeX, float itemSizeY, float itemX, float itemY, itemData itemData)//������ ȹ��
    {
        GameObject temp = Instantiate(itemPrefab, inventoryCanvas.transform);
        itemObject tempItem = temp.GetComponent<itemObject>();
        itemData newData = new itemData();
        newData.itemID = itemData.itemID; newData.tag = itemData.tag; newData.itemName = itemData.itemName;
        newData.Left = itemX;newData.Up = itemY;newData.xSize = itemSizeX;newData.ySize = itemSizeY;
        newData.isEquip = false;newData.price = itemData.price;newData.isSell = false;
        tempItem.Setup(itemSizeX, itemSizeY, itemX, itemY, false, zero, newData);
        itemObjects.Add(temp);
        tempItem.ItemData.Left = itemX;
        tempItem.ItemData.Up = itemY;
        items.items.Add(tempItem.ItemData);

        QuestObject quest = GameObject.Find("GameManager").GetComponent<QuestObject>();

        if (quest.GetQuestKind() == QuestKind.cook
            && itemData.itemID == quest.GetObjectId())
            quest.SetObjectIndex(quest.GetObjectIndex() + 1);
    }
    public Vector2 emptyCell(float newXSize, float newYSize)//�κ��丮���� �ش� ũ���� �������� ���� �� �ִ� ��ġ
    {
        List<Vector2> existCell = new List<Vector2>();
        existCell = existCells(items.items);
        List<Vector2> emptyCell = new List<Vector2>();
        for (int j = 0; j < YSize; j++) //ĭ ����
        {
            for (int i = 0; i < XSize; i++)
            {
                if (existCell.Contains(new Vector2(i, j)))
                    continue;
                emptyCell.Add(new Vector2(i, j));
            }
        }
        foreach (var item in emptyCell)
        {
            bool isOkay = true;
            for (int i = 0; i < newYSize; i++)
            {
                Vector2 tempPoint = item + Vector2.up * i;
                for (int j = 0; j < newXSize; j++)
                {
                    if (!emptyCell.Contains(tempPoint + Vector2.right * j))
                        isOkay = false;
                }
            }
            if (isOkay)
            {
                return new Vector2(item.x, item.y);
            }
        }
        return Vector2.zero - Vector2.one;
    }
    public void goldSet()
    {
        goldObj.transform.GetChild(0).GetComponent<Text>().text = "G " + Gold.ToString();//��� �� ����
    }
    #region ������ ���콺 ����
    public void itemHover(itemObject itemObj)//�����ۿ� ���콺 �÷��� ��
    {
        itemSummary.SetActive(true);
        itemSummary.transform.SetAsLastSibling();
        itemSummary.transform.GetChild(0).GetComponent<Text>().text = itemObj.ItemData.itemName;
    }
    public void itemExit()//���콺�� �����ۿ��� ������ ��
    {
        itemSummary.SetActive(false);
    }
    public void itemSummaryMove()//itemSummary �̵�
    {
        itemSummary.transform.position = Input.mousePosition + Vector3.right * 150f;
    }
    public void itemLeftDown(itemObject itemObj)//�������� ��Ŭ�� ���� ��
    {
        #region ����
        itemDescription.SetActive(true);
        itemDescription.transform.GetChild(0).GetComponent<Text>().text = itemObj.ItemData.itemName;
        bool isFood = false;
        string description = "";
        foreach (var item in itemObj.ItemData.tag)
        {
            if (item == "food")
            {
                isFood = true;
                break;
            }
        }
        if (isFood)
            switch (itemObj.ItemData.itemID)
            {
                case 3:
                    description = "���¹̳��� ȸ���� �� ����";
                    break;
            }
        else
            switch (itemObj.ItemData.itemID)
            {
                default:
                    break;
            }
        if (itemObj.ItemData.isSell)
            description = description + "\n" + Mathf.FloorToInt(itemObj.ItemData.price * 1.5f);
        itemDescription.transform.GetChild(1).GetComponent<Text>().text = description;
        #endregion
    }
    public void itemRightDown(itemObject itemObj)//�������� ��Ŭ�� ���� ��
    {
        if (itemObj.ItemData.isSell)
            return;//�Ǹ� ��ǰ�� ��Ŭ���ص� ������ ����� �������� ����
        foreach (var item in itemObj.ItemData.tag)
        {
            if (item == "equip")
            {
                itemObj.equipItem();
                break;
            }
            if (item == "food")
            {
                switch (itemObj.ItemData.itemID)
                {//id�� ���� food ȿ�� ����
                    case 3:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().Stamina += 10f;
                        break;
                    case 2001:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().Health += 10f;
                        break;
                    case 2002:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().Health += 20f;
                        break;
                    default:
                        break;
                }
                throwItem(itemObj.gameObject, false);
            }
        }
    }
    #endregion
    #region get;set;
    public GameObject getObj(string objectKind)
    {
        switch (objectKind)
        {
            case "KeyF":
                return keyF;
            case "Cell":
                return cellObj;
            case "itemPrefab":
                return itemPrefab;
            default:
                return null;
        }
    }
    #endregion
}
