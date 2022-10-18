using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    itemJsonData itemJsonData;//json데이터
    List<itemData> shopData;
    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "shop");//json로드
        shopData = itemJsonData.itemList;
        //json에서 상점 데이터 가져올 것
    }
    public void setUp()
    {
        //상점화면 열 것
    }
}
