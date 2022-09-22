using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemInfo
{
    public static int nextId = 0;//각 아이템 ID 조정
    public int itemID;//아이템 식별 번호
    public int sizeX;//사이즈
    public int sizeY;

    public ItemInfo(int wantSizeX, int wantSizeY)//생성자에 식별번호 및 사이즈 지정
    {
        itemID = nextId++;
        sizeX = wantSizeX;
        sizeY = wantSizeY;
    }
}
public class ItemContain
{
    private int locaX = 0;//위치
    private int locaY = 0;
    private ItemInfo itemCurrent = null;//현재 아이템 정보
    public ItemContain(int wantLocaX, int wantLocaY, ItemInfo wantItem)//생성자로 위치 및 아이템 지정
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
    public bool ItemOverlap(int targetLocaX, int targetLocaY, int targetSizeX, int targetSizeY)//아이템 겹침 확인
    {
        return !(targetLocaX >= locaX+itemCurrent.sizeX
            ||targetLocaX+targetSizeX<=locaX
            ||targetLocaY >= locaY+itemCurrent.sizeY
            ||targetLocaY+targetSizeY<=locaY);
    }
    public bool ItemOverlap(int targetLocaX, int targetLocaY)//클릭 아이템 확인 *이후 변경가능성 있음
    {
        return targetLocaX >= locaX
            && targetLocaX < locaX + itemCurrent.sizeX
            && targetLocaY >= locaY
            && targetLocaY < locaY + itemCurrent.sizeY;
    }
}
public class InventoryBase
{
    public List<ItemContain> itemList = new List<ItemContain>();//인벤토리 내 아이템
    public int inventorySizeX;//인벤토리 사이즈
    public int inventorySizeY;
    public bool insertList(ItemContain wantItem, int wantX, int wantY)//인벤토리 내 아이템 추가
    {
        int targetIndex = 0;//리스트 내 최종위치
        int existingLocaX;//기존 아이템 위치
        int existingLocaY;
        for (int i = 0; i < itemList.Count; i++)//기존 아이템들 전체 돌기
        {
            existingLocaY = itemList[i].getLocaY();//기존 아이템의 Y값 대기
            if (wantY == existingLocaY)//y값이 같을 경우
            {
                existingLocaX = itemList[i].getLocaX();//기존 아이템의 X값 대기
                if (wantX < existingLocaX)//추가 아이템 | 기존 아이템 의 경우
                {
                    targetIndex = i;//해당 위치에 아이템 추가, 기존 아이템은 뒤로 밀린다.
                    break;
                }
                else if (wantX > existingLocaX)//기존 아이템 | 추가 아이템 의 경우
                {
                    targetIndex = i + 1;//그 다음 위치에 아이템 추가, 다음 위치의 아이템 확인
                }
                else//동일할 경우
                    return false;//불가능!
            }
            else if (wantY < existingLocaY)//추가 아이템이 기존 아이템의 아래일 경우
            { 
                targetIndex = i;//해당 위치에 아이템 추가.
                break;
            }
            else//추가 아이템이 기존 아이템의 위일 경우
                targetIndex = i + 1;//뒤로 물린다
        }
        //위의 과정 중 추가위치(targetIndex) 확정
        if (targetIndex >= itemList.Count)//위치가 기존 개수를 넘을 경우(가장 마지막의 경우)
            itemList.Add(wantItem);//새로 추가
        else//아닐 경우
            itemList.Insert(targetIndex, wantItem);//해당 위치에 삽입
        return true;
    }
    public ItemContain removeList(int wantIndex)//인덱스 pop
    {
        ItemContain result = itemList[wantIndex];
        itemList.RemoveAt(wantIndex);
        return result;
    }
    public ItemContain itemFind(int wantX, int wantY)//특정 위치 아이템 pop
    {
        foreach (var item in itemList)
        {
            if (item.ItemOverlap(wantX, wantY))
                return item;
        }
        return null;
    }
    public ItemContain itemFind(int wantX, int wantY, out int returnIndex)//특정 위치 아이템 pop, pop한 아이템 인덱스도
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
