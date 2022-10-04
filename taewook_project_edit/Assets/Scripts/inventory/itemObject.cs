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
    public Vector3 Size;
    public Vector3 OriginPos;
    bool isEquip;
    [SerializeField] GameObject equip;
    public ItemData ItemData;
    float cell;
    int temp = 0;
    public void Setup(float sizeX, float sizeY, float posX, float posY, bool isEquip, float cell, Vector3 zero, ItemData itemData)
    {
        this.zeroPos = zero;
        maxPos = zeroPos + (Vector3.right * inventoryObject.Inst.XSize * cell) + (Vector3.down * inventoryObject.Inst.YSize * cell);

        this.Size = new Vector3(sizeX * cell, sizeY * cell, 1f);
        this.GetComponent<RectTransform>().sizeDelta = this.Size;

        this.transform.localPosition = new Vector3(
                    zeroPos.x + posX * cell + Size.x / 2f,
                    zeroPos.y - posY * cell - Size.y / 2f);
        this.OriginPos = this.transform.localPosition;

        this.isEquip = isEquip;
        equip.SetActive(isEquip);

        this.ItemData = itemData;
        this.cell = cell;
        Color imageColor = new Color();
        switch (this.ItemData.ItemID)
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
        float xPos = this.transform.localPosition.x;
        float yPos = this.transform.localPosition.y;
        float xSiz = this.Size.x / 2f;
        float ySiz = this.Size.y / 2f;
        //print(left + " " + right + " " + up + " " + down);
        if (Input.GetMouseButtonUp(0))
        { //위치 조정
            float tempL = ((xPos - xSiz) + (cell / 2f)) % cell;
            xPos -= tempL;

            if (tempL < -(cell / 2f))
                xPos -= cell;

            else if (tempL > (cell / 2f))
                xPos += cell;

            float tempU = ((yPos + ySiz) + (cell / 2f)) % cell;
            yPos -= tempU;
            
            if (tempU < -(cell / 2f))
                yPos -= cell;
            
            else if (tempU > (cell / 2f))
                yPos += cell;
            
            if (
                (xPos - xSiz) < this.zeroPos.x
                || (yPos + ySiz) > this.zeroPos.y
                || (xPos + xSiz) > this.maxPos.x
                || (yPos - ySiz) < this.maxPos.y
                )
            {
                inventoryObject.Inst.ThrowItem(this.gameObject);
            }
            else
            {
                Vector3 temp = new Vector3(xPos, yPos, 0f);
                inventoryObject.Inst.SetItemPos(this.gameObject, temp);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isEquip = !isEquip;
            equip.SetActive(isEquip);
        }
    }
    void Update()
    {   
        if (isHover)
        {
            if (Input.GetMouseButtonDown(0))
            { //좌클릭
                //print("마우스 좌클릭");
                OriginPos = this.transform.localPosition;
            }
            else if (Input.GetMouseButtonDown(1))
            { //우클릭
                //print("마우스 우클릭");
            }
            else
            {//호버
                if (temp == 0)
                {
                    inventoryObject.Inst.ItemHover(this);
                    //print("마우스 호버");
                    temp = 1;
                }
            }
        }
        else
            temp = 0;
    }
}
