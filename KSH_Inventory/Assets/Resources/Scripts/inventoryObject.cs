using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryObject : MonoBehaviour
{
    public static inventoryObject Inst { get; private set; }
    private void Awake() => Inst = this;
    [SerializeField] itemSO items;
    [SerializeField] GameObject itemPrefab;
    itemJsonData itemJsonData;
    [SerializeField] GameObject inventoryCanvas;
    public float cell;
    public float xSize;
    public float ySize;
    List<GameObject> itemObjects;
    Vector3 zero;
    void Start()
    {
        zero = new Vector3(xSize * cell / -2f, ySize * cell / 2f, 0f);
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "items");
        items.items = new List<itemData>();
        foreach (var item in itemJsonData.itemList)
            items.items.Add(item);
        itemObjects = new List<GameObject>();
        foreach (var item in items.items)
        {
            GameObject temp = Instantiate(itemPrefab, inventoryCanvas.transform);
            itemObject tempItem = temp.GetComponent<itemObject>();
            tempItem.Setup(item.xSize, item.ySize, item.Left, item.Up, item.isEquip, cell, zero, item);
            itemObjects.Add(temp);
        }
    }

    public void setItemPos(GameObject item, Vector3 newOrigonPos)
    {
        itemObject itemObj = item.GetComponent<itemObject>();
        float left = ((newOrigonPos.x - item.GetComponent<RectTransform>().rect.width / 2f) / cell) + (xSize / 2f);
        float up = ((newOrigonPos.y + item.GetComponent<RectTransform>().rect.height / 2f) / cell) - (ySize / 2f);
        up *= -1f;
        foreach (var Item in items.items)
        {
            if (Item.itemID == itemObj.itemData.itemID)
            { }
            else
            {
                float tempL = Item.Left < left ? left : Item.Left;
                float tempR = Item.Left + Item.xSize > left + itemObj.itemData.xSize ? left + itemObj.itemData.xSize : Item.Left + Item.xSize;
                float tempU = Item.Up < up ? up : Item.Up;
                float tempD = Item.Up + Item.ySize > up + itemObj.itemData.ySize ? up + itemObj.itemData.ySize : Item.Up + Item.ySize;
                if (tempL < tempR && tempU < tempD)
                {
                    print("ºÎµúÈû");
                    item.transform.localPosition = item.GetComponent<itemObject>().originPos;
                    return;
                }
            }
        }
        item.transform.localPosition = item.GetComponent<itemObject>().originPos = newOrigonPos;
        foreach (var Item in items.items)
        {
            if (Item.itemID == itemObj.itemData.itemID)
            {
                Item.Left = left;
                Item.Up = up;
            }
        }
        itemJsonData temp = new itemJsonData();
        temp.itemList = new List<itemData>();
        foreach (var Item in items.items)
        {
            temp.itemList.Add(Item);
        }
        string tempStr = json.ObjectToJson(temp);
        json.CreateJsonFile(Application.dataPath, "items", tempStr);
    }
}
