using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    itemJsonData itemJsonData;//json������
    List<itemData> shopData;
    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "shop");//json�ε�
        shopData = itemJsonData.itemList;
        //json���� ���� ������ ������ ��
    }
    public void setUp()
    {
        //����ȭ�� �� ��
    }
}
