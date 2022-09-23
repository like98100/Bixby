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
    public int itemCollision(int wantX, int wantY, int wantSizeX, int wantSizeY, out int firstTouch)//범위 내 아이템 수 반환
    {
        if (wantY + wantSizeY > inventorySizeY)//인벤토리 범위를 넘으면 음수 반환
        {
            firstTouch = -4;
            return -1024;
        }
        int touchedItem = 0;//초기화
        firstTouch = -4;
        for (int i = 0; i < itemList.Count; i++)//리스트 반복
        {
            if (itemList[i].ItemOverlap(wantX, wantY, wantSizeX, wantSizeY))//기존 아이템과 범위가 겹칠 경우
            {
                ++touchedItem;//겹치는 수 늘리기
                if (firstTouch == -4)//기존에 닿았던 아이템이 없다면
                    firstTouch = i;//해당 아이템 표시
            }
        }
        return touchedItem;//겹친 수 반환
    }
    public bool itemPlace(ItemContain wantItem, int wantX, int wantY)//아이템 배치
    {
        int touchedItemIndex;
        if (itemCollision(wantX, wantY, wantItem.getSizeX(), wantItem.getSizeY(), out touchedItemIndex) > 0)
            return false;//충돌하는 아이템이 있다면 실패
        else if (wantY + wantItem.getSizeY() > inventorySizeY || wantX + wantItem.getSizeX() > inventorySizeX)
            return false;//인벤토리 밖이라면 실패
        else//성공이라면
        {
            insertList(wantItem, wantX, wantY);//리스트에 아이템 추가
            wantItem.setLoca(wantX, wantY);//해당 아이템 위치 지정
            return true;
        }
    }

    public void heightSave(List<int> heightList, int findStartIndex, int targetY)
    {//높이 리스트에 넣을 수 있을지 확인(y값 기준으로 추가)
        for (int i = findStartIndex; i < heightList.Count; i++)
        {//지정 인덱스부터 기존 높이들 리스트 확인
            if (targetY == heightList[i])
                return;//넣고자 하는 y값에 이미 기존 개체가 있다면 종료
            else if (targetY < heightList[i])
            {//넣고자 하는 y값이 기존 개체보다 아래라면
                heightList.Insert(i, targetY);//해당 위치에 넣고, 뒤로 밀어낸다
                return;
            }
        }
        heightList.Add(targetY);//위의 경우가 아니라면 가장 높은 위치이므로 가장 마지막에 추가
    }

    public bool itemPlaceFinder(int wantSizeX, int wantSizeY, out int returnLocaX, out int returnLocaY)
    { //XFinder와 heightSave를 활용, 아이템이 들어갈 장소 확인 및 반환
        if (itemList.Count <= 0)
        {//기존 아이템이 없다면 == 첫 아이템이라면
            returnLocaX = returnLocaY = 0;//가장 첫번째에 위치
            return true;//들어갈 장소 존재
        }
        else
        {
            int curFindX;//현재 찾는 x좌표
            int checkedIndex = 0;//확인된 아이템 위치
            List<XFinder> finderList = new List<XFinder>();//들어갈 수 있는 x범위 리스트
            List<int> heightList = new List<int>();//확인할 필요 있는 y위치 저장
            heightList.Add(0);//0부터 확인
            foreach (var item in heightList)
            {
                if (item + wantSizeY > inventorySizeY)
                {//확인한 y축이 인벤토리를 넘을 경우
                    returnLocaX = returnLocaY = 0;
                    return false;//실패
                }
                curFindX = 0;//찾는 x좌표 초기화
                int targetFianlY;//확인중인 아이템 최종 높이
                for (int i = checkedIndex; i < itemList.Count; i++)
                {//확인되지 않은 아이템 탐색
                    if (itemList[i].getLocaY() == item)
                    { //확인중인 y축과 해당 아이템의 y축이 같은 경우
                        targetFianlY = itemList[i].getLocaY() + itemList[i].getSizeY();//최종 높이 설정
                        heightSave(heightList, heightList.IndexOf(item), targetFianlY);//설정한 최종 높이를 확인한 높이에 추가

                    }
                }
            }
        }
        returnLocaX = returnLocaY = 0;
        return false;//위에서 아이템을 넣지 못했을 경우 : 실패
    }
}
public class XFinder//x위치 표시(아이템 사이 간격 저장)
{
    public int XMin, XMax;
    public XFinder(int wantXMin, int wantXMax)
    {
        XMin = wantXMin;
        XMax = wantXMax;
    }
}
public class Inventory : MonoBehaviour
{

}
