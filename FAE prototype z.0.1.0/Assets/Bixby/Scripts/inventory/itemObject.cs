using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class itemObject : MonoBehaviour
{
    [SerializeField] GameObject image;//setup에서 설정 필요
    bool isHover;
    
    Vector3 zeroPos;
    Vector3 maxPos;
    Vector3 size;
    public Vector3 OriginPos;
    bool isEquip;
    GameObject equip;
    public itemData ItemData;
    float cell;
    public void Setup(float sizeX, float sizeY, float posX, float posY, bool isEquip, float cell, Vector3 zero, itemData itemData)
    {
        this.zeroPos = zero;
        maxPos = zeroPos + (Vector3.right * inventoryObject.Inst.XSize * cell) + (Vector3.down * inventoryObject.Inst.YSize * cell);

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
        this.cell = cell;
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
    public void Drag()
    {
        //print("드래그");
        this.transform.position = Input.mousePosition;//Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void Up()
    {
        if (ItemData.isSell)
        {
            //구매과정
            
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            float xPos = this.transform.localPosition.x;
            float yPos = this.transform.localPosition.y;
            float xSiz = this.size.x / 2f;
            float ySiz = this.size.y / 2f;
            //위치 조정
            float tempL = ((xPos - xSiz) + (cell / 2f)) % cell;
            xPos -= tempL;
            if (tempL < -(cell / 2f))
                xPos -= cell;

            else if (tempL > (cell / 2f))
                xPos += cell;
            xPos = this.zeroPos.x % cell == 0 ? xPos - cell / 2f : xPos;

            float tempU = ((yPos + ySiz) + (cell / 2f)) % cell;
            yPos -= tempU;
            if (tempU < -(cell / 2f))
                yPos -= cell;

            else if (tempU > (cell / 2f))
                yPos += cell;
            yPos = this.zeroPos.y % cell == 0 ? yPos + cell / 2f : yPos;

            if (
                (xPos - xSiz) < this.zeroPos.x
                || (yPos + ySiz) > this.zeroPos.y
                || (xPos + xSiz) > this.maxPos.x
                || (yPos - ySiz) < this.maxPos.y
                )
            {
                inventoryObject.Inst.throwItem(this.gameObject, true);
            }
            else
            {
                Vector3 temp = new Vector3(xPos, yPos, 0f);
                inventoryObject.Inst.setItemPos(this.gameObject, temp);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            foreach (var item in ItemData.tag)
            {
                if (item == "equip")
                {
                    isEquip = !isEquip;
                    equip.SetActive(isEquip);
                    break;
                }
                if (item == "food")
                {
                    switch (ItemData.itemID)
                    {//id에 따라 food 효과 조정
                        case 3:
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().Stamina += 10f;
                            break;
                        default:
                            break;
                    }
                    inventoryObject.Inst.throwItem(this.gameObject, false);
                }
            }
        }
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
    public void Down()//클릭시
    {
        if (Input.GetMouseButtonDown(0))
            inventoryObject.Inst.itemLeftDown(this);
    }
    void Update()
    {   
       if(isHover)
        {
            inventoryObject.Inst.itemSummaryMove();
        }
    }
}
