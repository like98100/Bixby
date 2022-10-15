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
    }
    itemSO items;//itemSO, �κ��丮 �� �����۵��� �� ���� items�� �����صд�
    [SerializeField] GameObject itemPrefab;//������ ������, �κ��丮 ����, Ȥ�� ������ ȹ���Ҷ� �ν���Ʈ�� ���
    itemJsonData itemJsonData;//json������
    GameObject inventoryCanvas;
    GameObject inventoryObj;
    [SerializeField] GameObject cellObj;
    GameObject goldObj;
    [SerializeField] float cell;
    public float XSize;
    public float YSize;
    List<GameObject> itemObjects;
    Vector3 zero;
    public int Gold;
    [SerializeField] GameObject fieldItemPrefab;
    public GameObject FieldFKey;
    public GameObject KeyF;
    void Start()
    {
        inventoryObj.GetComponent<RectTransform>().sizeDelta = new Vector2(XSize * cell, YSize * cell);//�κ�â ũ��
        zero = new Vector3(XSize * cell / -2f, YSize * cell / 2f, 0f);//json����� ��ǥ 0,0�� ���� ��ġ
        for (int i = 0; i < YSize; i++)
        {
            for (int j = 0; j < XSize; j++)
            {
                GameObject temp = Instantiate(cellObj, inventoryObj.transform);
                temp.GetComponent<RectTransform>().sizeDelta = new Vector2(cell * 0.9f, cell * 0.9f);
                temp.transform.localPosition = new Vector3(j * cell + (zero.x + cell / 2f), i * -cell + (zero.y - cell / 2f), 0f);
            }
        }//ĭ ����
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "items");//json�ε�
        Gold = itemJsonData.gold;//��� �� �ε�
        goldObj.GetComponent<RectTransform>().sizeDelta = new Vector2(XSize * cell, 50f);//���â ũ��
        goldObj.transform.localPosition = new Vector3(0f, -1 * (inventoryObj.GetComponent<RectTransform>().rect.height / 2f + 25f), 0f);//���â ��ġ
        goldObj.transform.GetChild(0).GetComponent<Text>().text = "G " + Gold.ToString();//��� �� ����
        items.items = new List<itemData>();
        foreach (var item in itemJsonData.itemList)
            items.items.Add(item);//itemSO ������ �߰�
        itemObjects = new List<GameObject>();
        foreach (var item in items.items)
        {
            GameObject temp = Instantiate(itemPrefab, inventoryCanvas.transform);
            itemObject tempItem = temp.GetComponent<itemObject>();
            tempItem.Setup(item.xSize, item.ySize, item.Left, item.Up, item.isEquip, cell, zero, item);
            itemObjects.Add(temp);
        }//������ ����ȭ
        inventoryCanvas.SetActive(false);//���۽� �κ�â ��������
        FieldFKey = null;
    }

    public void setItemPos(GameObject item, Vector3 newOrigonPos)//�κ� �� ������ ��ġ �̵��� ����(��ħȮ��)
    {
        itemObject itemObj = item.GetComponent<itemObject>();
        float left = ((newOrigonPos.x - item.GetComponent<RectTransform>().rect.width / 2f) / cell) + (XSize / 2f);
        float up = ((newOrigonPos.y + item.GetComponent<RectTransform>().rect.height / 2f) / cell) - (YSize / 2f);
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
    void jsonSave()//json����
    {
        itemJsonData temp = new itemJsonData();
        temp.itemList = new List<itemData>();
        foreach (var Item in items.items)
        {
            temp.itemList.Add(Item);
        }
        int gold = this.Gold;
        temp.gold = gold;
        string tempStr = json.ObjectToJson(temp);
        json.CreateJsonFile(Application.dataPath, "items", tempStr);
    }
    public void getFieldItem(GameObject newItem)//�ʵ忡�� �ű� ������ ȹ��
    {
        itemData newData = newItem.GetComponent<fieldItem>().ItemData;
        float newXSize = newData.xSize;
        float newYSize = newData.ySize;
        List<Vector2> existCell = new List<Vector2>();
        existCell = existCells();
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
                GameObject temp = Instantiate(itemPrefab, inventoryCanvas.transform);
                itemObject tempItem = temp.GetComponent<itemObject>();
                tempItem.Setup(newData.xSize, newData.ySize, item.x, item.y, newData.isEquip, cell, zero, newData);
                itemObjects.Add(temp);
                tempItem.ItemData.Left = item.x;
                tempItem.ItemData.Up = item.y;
                items.items.Add(tempItem.ItemData);
                Destroy(newItem);
                jsonSave();
                Destroy(FieldFKey);
                FieldFKey = null;
                return;
            }
        }
    }
    public List<Vector2> existCells()//�κ��丮 �� ������ ���� ĭ Ȯ��
    {
        List<Vector2> result = new List<Vector2>();
        foreach (var item in items.items)
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
    public void itemHover(itemObject itemObj)//�κ��丮���� �����ۿ� ���콺�� �÷����� �� ����
    {
        print("�� �Ķ���� ǥ��");
    }
    public void throwItem(GameObject itemObj, bool isLeft)//�κ��丮���� ������ ���� ��
    {
        items.items.Remove(itemObj.GetComponent<itemObject>().ItemData);
        itemObjects.Remove(itemObj.gameObject);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject temp = isLeft ? MakeFieldItem(itemObj.GetComponent<itemObject>().ItemData, player.transform.position) : null;
        Destroy(itemObj);
        jsonSave();
    }

    public GameObject MakeFieldItem(itemData data, Vector3 position)//�ʵ忡 ������ ����
    {
        GameObject temp = Instantiate(fieldItemPrefab, position, Quaternion.identity);
        temp.GetComponent<fieldItem>().setup(data);
        return temp;
    }
}
