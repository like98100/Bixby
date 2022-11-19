using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    itemJsonData itemJsonData;//json������
    List<itemData> shopData;
    GameObject shopWindow;
    [SerializeField] float shopXSize;
    [SerializeField] float shopYSize;
    List<GameObject> shopItems;
    void Awake()
    {
        shopWindow = GameObject.Find("Shop");
        shopWindow.SetActive(false);
    }
    private void Start()
    {
        if (!json.FileExist(Application.dataPath, "items"))
        {
            itemJsonData = new itemJsonData();
            string tempData = json.ObjectToJson(itemJsonData);
            json.CreateJsonFile(Application.dataPath, "items", tempData);
        }
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "shop");//json�ε�
        shopData = itemJsonData.itemList;

        shopWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(shopXSize * inventoryObject.Inst.Cell, shopYSize * inventoryObject.Inst.Cell);//����â ũ��
        float Cell = inventoryObject.Inst.Cell;
        Vector3 zero = new Vector3(shopXSize * Cell / -2f, shopYSize * Cell / 2f, 0f);//json����� ��ǥ 0,0�� ���� ��ġ
        for (int i = 0; i < shopYSize; i++)
        {
            for (int j = 0; j < shopXSize; j++)
            {
                GameObject temp = Instantiate(inventoryObject.Inst.getObj("Cell"), shopWindow.transform.GetChild(0));
                temp.GetComponent<RectTransform>().sizeDelta = new Vector2(Cell * 0.9f, Cell * 0.9f);
                temp.transform.localPosition = new Vector3(j * Cell + (zero.x + Cell / 2f), i * -Cell + (zero.y - Cell / 2f), 0f);
            }
        }//ĭ ����
        List<itemData> tempList = new List<itemData>();
        foreach (var item in shopData)
        {
            GameObject temp = Instantiate(inventoryObject.Inst.getObj("itemPrefab"), shopWindow.transform);//�ν��Ͻ�
            itemObject tempItem = temp.GetComponent<itemObject>();//������Ʈ ����
            
            float newXSize = item.xSize;
            float newYSize = item.ySize;
            List<Vector2> existCell = inventoryObject.Inst.existCells(tempList);
            List<Vector2> emptyCell = new List<Vector2>();
            for (int j = 0; j < shopYSize; j++) //ĭ ����
            {
                for (int i = 0; i < shopXSize; i++)
                {
                    if (existCell.Contains(new Vector2(i, j)))
                        continue;
                    emptyCell.Add(new Vector2(i, j));
                }
            }
            foreach (var item_ in emptyCell)
            {
                bool isOkay = true;
                for (int i = 0; i < newYSize; i++)
                {
                    Vector2 tempPoint = item_ + Vector2.up * i;
                    for (int j = 0; j < newXSize; j++)
                    {
                        if (!emptyCell.Contains(tempPoint + Vector2.right * j))
                            isOkay = false;
                    }
                }
                if (isOkay)
                {
                    item.Left = item_.x;
                    item.Up = item_.y;
                }
            }
            tempItem.Setup(item.xSize, item.ySize, item.Left, item.Up, false, zero, item);
            tempList.Add(tempItem.ItemData);
        }//������ ����ȭ
    }
    public GameObject getWindow()
    {
        return shopWindow;
    }
    public void SetUp()
    {
        UI_Control.Inst.windowClose();
        UI_Control.Inst.windowSet(shopWindow);
        shopItems = new List<GameObject>();
        foreach (Transform item in shopWindow.transform)
        {
            if (item.gameObject.name != "cells")
                shopItems.Add(item.gameObject);
        }
        foreach (var item in shopItems)
        {
            item.SetActive(true);
            if (item.transform.parent.name == "Shop")
                item.GetComponent<itemObject>().ItemData.isSell = true;
        }
    }
}
