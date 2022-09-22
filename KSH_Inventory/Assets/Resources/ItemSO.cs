using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item
{
    public List<string> kind;//종류 : 장비(Equip), 음식(Food), 잡템(Oddment), 소재(Material)
    public int width;//너비
    public int height;//높이
}
[CreateAssetMenu(fileName ="ItemSO",menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    public List<Item> itemComponents;
}