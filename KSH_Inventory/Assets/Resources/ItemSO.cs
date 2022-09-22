using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item
{
    public List<string> kind;//���� : ���(Equip), ����(Food), ����(Oddment), ����(Material)
    public int width;//�ʺ�
    public int height;//����
}
[CreateAssetMenu(fileName ="ItemSO",menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    public List<Item> itemComponents;
}