using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryObject : MonoBehaviour
{
    public static inventoryObject Inst { get; private set; }
    private void Awake() => Inst = this;
    [SerializeField] itemSO items;
    [SerializeField] GameObject itemPrefab;
    ItemJsonData itemJsonData;
    [SerializeField] GameObject inventoryCanvas;
    [SerializeField] GameObject inventoryObj;
    [SerializeField] GameObject cellObj;
    [SerializeField] GameObject goldObj;
    public float Cell;
    public float XSize;
    public float YSize;
    List<GameObject> itemObjects;
    Vector3 zero;
    int gold;
    [SerializeField] GameObject fieldItemPrefab;
    public GameObject FieldFKey;
    void Start()
    {
        inventoryObj.GetComponent<RectTransform>().sizeDelta = new Vector2(XSize * Cell, YSize * Cell);//�κ�â ũ��
        zero = new Vector3(XSize * Cell / -2f, YSize * Cell / 2f, 0f);//json����� ��ǥ 0,0�� ���� ��ġ
        for (int i = 0; i < YSize; i++)
        {
            for (int j = 0; j < YSize; j++)
            {
                GameObject temp = Instantiate(cellObj, inventoryObj.transform);
                temp.GetComponent<RectTransform>().sizeDelta = new Vector2(Cell * 0.9f, Cell * 0.9f);
                temp.transform.localPosition = new Vector3(j * Cell + (zero.x + Cell / 2f), i * -Cell + (zero.y - Cell / 2f), 0f);
            }
        }//ĭ ����
        itemJsonData = Json.LoadJsonFile<ItemJsonData>(Application.dataPath, "items");//json�ε�
        gold = itemJsonData.Gold;//��� �� �ε�
        goldObj.GetComponent<RectTransform>().sizeDelta = new Vector2(XSize * Cell, 50f);//���â ũ��
        goldObj.transform.localPosition = new Vector3(0f, -1 * (inventoryObj.GetComponent<RectTransform>().rect.height / 2f + 25f), 0f);//���â ��ġ
        goldObj.transform.GetChild(0).GetComponent<Text>().text = "G " + gold.ToString();//��� �� ����
        items.items = new List<ItemData>();
        foreach (var item in itemJsonData.ItemList)
            items.items.Add(item);//itemSO ������ �߰�
        itemObjects = new List<GameObject>();
        foreach (var item in items.items)
        {
            GameObject temp = Instantiate(itemPrefab, inventoryCanvas.transform);
            itemObject tempItem = temp.GetComponent<itemObject>();
            tempItem.Setup(item.XSize, item.YSize, item.Left, item.Up, item.IsEquip, Cell, zero, item);
            itemObjects.Add(temp);
        }//������ ����ȭ
        inventoryCanvas.SetActive(false);//���۽� �κ�â ��������
        FieldFKey = null;
    }

    public void SetItemPos(GameObject item, Vector3 newOrigonPos)//�κ� �� ������ ��ġ �̵��� ����(��ħȮ��)
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
                if (isCrash(Item.Left, Item.XSize, left, itemObj.ItemData.XSize, Item.Up, Item.YSize, up, itemObj.ItemData.YSize))
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
        ItemJsonData temp = new ItemJsonData();
        temp.ItemList = new List<ItemData>();
        foreach (var Item in items.items)
        {
            temp.ItemList.Add(Item);
        }
        int gold = this.gold;
        temp.Gold = gold;
        string tempStr = Json.ObjectToJson(temp);
        Json.CreateJsonFile(Application.dataPath, "items", tempStr);
    }
    public void GetFieldItem(GameObject newItem)//�ʵ忡�� �ű� ������ ȹ��
    {
        ItemData newData = newItem.GetComponent<fieldItem>().ItemData;
        float newXSize = newData.XSize;
        float newYSize = newData.YSize;
        List<Vector2> existCell = new List<Vector2>();
        existCell = ExistCells();
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
        foreach (var item in emptyCell)//�̷��� ã�� ����� ��Ʈ��ŷ�̶�� �Ѵ�(�� �����)
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
                tempItem.Setup(newData.XSize, newData.YSize, item.x, item.y, newData.IsEquip, Cell, zero, newData);
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
    public List<Vector2> ExistCells()//�κ��丮 �� ������ ���� ĭ Ȯ��
    {
        List<Vector2> result = new List<Vector2>();
        foreach (var item in items.items)
        {
            List<Vector2> existItem = new List<Vector2>();
            for (float j = item.Up; j <= item.Up + item.YSize; j++)
            {
                for (float i = item.Left; i <= item.Left + item.XSize; i++)
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
    public void ItemHover(itemObject itemObj)//�κ��丮���� �����ۿ� ���콺�� �÷����� �� ����
    {
        print("�� �Ķ���� ǥ��");
    }
    public void ThrowItem(GameObject itemObj)//�κ��丮���� ������ ���� ��
    {
        items.items.Remove(itemObj.GetComponent<itemObject>().ItemData);
        itemObjects.Remove(itemObj.gameObject);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject temp = MakeFieldItem(itemObj.GetComponent<itemObject>().ItemData, player.transform.position);
        Destroy(itemObj);
        jsonSave();
    }

    public GameObject MakeFieldItem(ItemData data, Vector3 position)//�ʵ忡 ������ ����
    {
        GameObject temp = Instantiate(fieldItemPrefab, position, Quaternion.identity);
        temp.GetComponent<fieldItem>().Setup(data);
        return temp;
    }
}
