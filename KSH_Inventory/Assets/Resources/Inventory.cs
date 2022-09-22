using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemInfo
{
    public static int nextId = 0;//�� ������ ID ����
    public int itemID;//������ �ĺ� ��ȣ
    public int sizeX;//������
    public int sizeY;

    public ItemInfo(int wantSizeX, int wantSizeY)//�����ڿ� �ĺ���ȣ �� ������ ����
    {
        itemID = nextId++;
        sizeX = wantSizeX;
        sizeY = wantSizeY;
    }
}
public class ItemContain
{
    private int locaX = 0;//��ġ
    private int locaY = 0;
    private ItemInfo itemCurrent = null;//���� ������ ����
    public ItemContain(int wantLocaX, int wantLocaY, ItemInfo wantItem)//�����ڷ� ��ġ �� ������ ����
    {
        locaX = wantLocaX;
        locaY = wantLocaY;
        itemCurrent = wantItem;
    }
    #region get,set
    public ItemInfo getInfo() { return itemCurrent; }
    public int getLocaX() { return locaX; }
    public int getLocaY() { return locaY; }
    public int getSizeX() { return itemCurrent.sizeX; }
    public int getSizeY() { return itemCurrent.sizeY; }
    public void setLoca(int wantX, int wantY)
    {
        locaX = wantX;
        locaY = wantY;
    }
    #endregion
    public bool ItemOverlap(int targetLocaX, int targetLocaY, int targetSizeX, int targetSizeY)//������ ��ħ Ȯ��
    {
        return !(targetLocaX >= locaX+itemCurrent.sizeX
            ||targetLocaX+targetSizeX<=locaX
            ||targetLocaY >= locaY+itemCurrent.sizeY
            ||targetLocaY+targetSizeY<=locaY);
    }
    public bool ItemOverlap(int targetLocaX, int targetLocaY)//Ŭ�� ������ Ȯ�� *���� ���氡�ɼ� ����
    {
        return targetLocaX >= locaX
            && targetLocaX < locaX + itemCurrent.sizeX
            && targetLocaY >= locaY
            && targetLocaY < locaY + itemCurrent.sizeY;
    }
}
public class InventoryBase
{
    public List<ItemContain> itemList = new List<ItemContain>();//�κ��丮 �� ������
    public int inventorySizeX;//�κ��丮 ������
    public int inventorySizeY;
    public bool insertList(ItemContain wantItem, int wantX, int wantY)//�κ��丮 �� ������ �߰�
    {
        int targetIndex = 0;//����Ʈ �� ������ġ
        int existingLocaX;//���� ������ ��ġ
        int existingLocaY;
        for (int i = 0; i < itemList.Count; i++)//���� �����۵� ��ü ����
        {
            existingLocaY = itemList[i].getLocaY();//���� �������� Y�� ���
            if (wantY == existingLocaY)//y���� ���� ���
            {
                existingLocaX = itemList[i].getLocaX();//���� �������� X�� ���
                if (wantX < existingLocaX)//�߰� ������ | ���� ������ �� ���
                {
                    targetIndex = i;//�ش� ��ġ�� ������ �߰�, ���� �������� �ڷ� �и���.
                    break;
                }
                else if (wantX > existingLocaX)//���� ������ | �߰� ������ �� ���
                {
                    targetIndex = i + 1;//�� ���� ��ġ�� ������ �߰�, ���� ��ġ�� ������ Ȯ��
                }
                else//������ ���
                    return false;//�Ұ���!
            }
            else if (wantY < existingLocaY)//�߰� �������� ���� �������� �Ʒ��� ���
            { 
                targetIndex = i;//�ش� ��ġ�� ������ �߰�.
                break;
            }
            else//�߰� �������� ���� �������� ���� ���
                targetIndex = i + 1;//�ڷ� ������
        }
        //���� ���� �� �߰���ġ(targetIndex) Ȯ��
        if (targetIndex >= itemList.Count)//��ġ�� ���� ������ ���� ���(���� �������� ���)
            itemList.Add(wantItem);//���� �߰�
        else//�ƴ� ���
            itemList.Insert(targetIndex, wantItem);//�ش� ��ġ�� ����
        return true;
    }
    public ItemContain removeList(int wantIndex)//�ε��� pop
    {
        ItemContain result = itemList[wantIndex];
        itemList.RemoveAt(wantIndex);
        return result;
    }
    public ItemContain itemFind(int wantX, int wantY)//Ư�� ��ġ ������ pop
    {
        foreach (var item in itemList)
        {
            if (item.ItemOverlap(wantX, wantY))
                return item;
        }
        return null;
    }
    public ItemContain itemFind(int wantX, int wantY, out int returnIndex)//Ư�� ��ġ ������ pop, pop�� ������ �ε�����
    {
        foreach (var item in itemList)
        {
            if (item.ItemOverlap(wantX, wantY))
            {
                returnIndex = itemList.IndexOf(item);
                return item;
            }
        }
        returnIndex = -4;
        return null;
    }
}
public class Inventory : MonoBehaviour
{

}
