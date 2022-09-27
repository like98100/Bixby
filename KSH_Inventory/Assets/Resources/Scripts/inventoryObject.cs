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

    public void setItemPos(GameObject item)
    {
        itemObject itemObj = item.GetComponent<itemObject>();
        item.transform.localPosition = item.GetComponent<itemObject>().originPos;
        float left = ((item.transform.localPosition.x - item.GetComponent<RectTransform>().rect.width / 2f) / cell) + (xSize / 2f);
        float up = ((item.transform.localPosition.y + item.GetComponent<RectTransform>().rect.height / 2f) / cell) - (ySize / 2f);
        up *= -1f;
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
