using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class itemObject : MonoBehaviour
{
    [SerializeField] GameObject image;//setup에서 설정 필요
    bool isHover;
    
    Vector3 zeroPos;
    Vector3 size;
    public Vector3 OriginPos;
    bool isEquip;
    GameObject equip;
    public itemData ItemData;
    public void Setup(float sizeX, float sizeY, float posX, float posY, bool isEquip, Vector3 zero, itemData itemData)
    {
        this.zeroPos = zero;
        float cell = inventoryObject.Inst.Cell;

        this.size = new Vector3(sizeX * cell, sizeY * cell, 1f);
        this.GetComponent<RectTransform>().sizeDelta = this.size;

        this.transform.localPosition = new Vector3(
                    zeroPos.x + posX * cell + size.x / 2f,
                    zeroPos.y - posY * cell - size.y / 2f);
        this.OriginPos = this.transform.localPosition;

        equip = this.transform.GetChild(0).gameObject;
        this.isEquip = isEquip;
        equip.SetActive(isEquip);

        this.ItemData = itemData;
        Color imageColor = new Color();
        switch (this.ItemData.itemID)
        {
            case 0:
                 imageColor = Color.red;
                break;
            case 1:
                imageColor = Color.green;
                break;
            case 2:
                imageColor = Color.blue;
                break;
            case 3:
                imageColor = Color.yellow;
                break;
            default:
                imageColor = Color.black;
                break;
        }
        imageColor.a = 0.5f;
        this.gameObject.GetComponent<Image>().color = imageColor;
    }
    #region 이벤트트리거
    public void Drag()
    {
        //print("드래그");
        this.transform.position = Input.mousePosition;//Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void Up()
    {
        if (ItemData.isSell)
        {
            if (ItemData.price * 1.5 < inventoryObject.Inst.Gold)
            { }
            else
            {
                inventoryObject.Inst.setItemPos(this.gameObject, OriginPos);
            }
            //구매과정
            return;
        }
        #region 위치이동
        float xPos = this.transform.localPosition.x;
        float yPos = this.transform.localPosition.y;
        float xSiz = this.size.x / 2f;
        float ySiz = this.size.y / 2f;
        float Cell = inventoryObject.Inst.Cell;
        float XSize = inventoryObject.Inst.XSize;
        float YSize = inventoryObject.Inst.YSize;
        float tempL = ((xPos - xSiz) + (Cell / 2f)) % Cell;
        Vector3 maxPos = zeroPos + (Vector3.right * XSize * Cell) + (Vector3.down * YSize * Cell);
        xPos -= tempL;

        if (tempL < -(Cell / 2f))
            xPos -= Cell;

        else if (tempL > (Cell / 2f))
            xPos += Cell;
        xPos = zeroPos.x % Cell == 0 ? xPos - Cell / 2f : xPos;

        float tempU = ((yPos + ySiz) + (Cell / 2f)) % Cell;
        yPos -= tempU;

        if (tempU < -(Cell / 2f))
            yPos -= Cell;

        else if (tempU > (Cell / 2f))
            yPos += Cell;
        yPos = zeroPos.y % Cell == 0 ? yPos + Cell / 2f : yPos;
        
        if (
            (xPos - xSiz) < zeroPos.x
            || (yPos + ySiz) > zeroPos.y
            || (xPos + xSiz) > maxPos.x
            || (yPos - ySiz) < maxPos.y
            )
        {
            if (!UI_Control.Inst.Shop.getWindow().activeSelf)
                inventoryObject.Inst.throwItem(this.gameObject, true);
        }
        else
        {
            inventoryObject.Inst.setItemPos(this.gameObject, new Vector3(xPos, yPos, 0f));
        }
        #endregion
    }
    public void Hover()
    {
        isHover = true;
        inventoryObject.Inst.itemHover(this);
    }
    public void Exit()
    {
        isHover = false;
        inventoryObject.Inst.itemExit();
    }
    public void Down()//마우스 클릭
    {
        if (Input.GetMouseButtonDown(0))
            inventoryObject.Inst.itemLeftDown(this);
        else if (Input.GetMouseButtonDown(1))
            inventoryObject.Inst.itemRightDown(this);
    }
    #endregion
    void Update()
    {   
        if (isHover)
        {
            inventoryObject.Inst.itemSummaryMove();
        }
    }
    public void equipItem()
    {
        isEquip = !isEquip;
        equip.SetActive(isEquip);
    }
}
