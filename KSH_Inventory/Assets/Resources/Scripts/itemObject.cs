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
    public Vector3 size;
    public Vector3 originPos;
    bool isEquip;
    [SerializeField] GameObject equip;
    public itemData itemData;
    float cell;
    public void Setup(float sizeX, float sizeY, float posX, float posY, bool isEquip, float cell, Vector3 zero, itemData itemData)
    {
        this.zeroPos = zero;
        maxPos = zeroPos + (Vector3.right * inventoryObject.Inst.xSize * cell) + (Vector3.down * inventoryObject.Inst.ySize * cell);

        this.size = new Vector3(sizeX * cell, sizeY * cell, 1f);
        this.GetComponent<RectTransform>().sizeDelta = this.size;

        this.transform.localPosition = new Vector3(
                    zeroPos.x + posX * cell + size.x / 2f,
                    zeroPos.y - posY * cell - size.y / 2f);
        this.originPos = this.transform.localPosition;

        this.isEquip = isEquip;
        equip.SetActive(isEquip);

        this.itemData = itemData;
        this.cell = cell;
        Color imageColor = new Color();
        switch (this.itemData.itemID)
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
        float left, right, up, down;
        left = this.transform.localPosition.x - this.size.x / 2f;
        right = this.transform.localPosition.x + this.size.x / 2f;
        up = this.transform.localPosition.y + this.size.y / 2f;
        down = this.transform.localPosition.y - this.size.y / 2f;
        //print(left + " " + right + " " + up + " " + down);
        if (Input.GetMouseButtonUp(0))
        { //위치 조정
            float tempL = (left + (cell/2f)) % cell;
            left -= tempL;
            right -= tempL;
            if (tempL < -(cell / 2f))
            {
                left -= cell;
                right -= cell;
            }
            else if (tempL > (cell / 2f))
            {
                left += cell;
                right += cell;
            }

            float tempU = (up + (cell / 2f)) % cell;
            up -= tempU;
            down -= tempU;
            if (tempU < -(cell / 2f))
            {
                up -= cell;
                down -= cell;
            }
            else if (tempU > (cell / 2f))
            {
                up += cell;
                down += cell;
            }
            //print(left + " " + right + " " + up + " " + down);
            if (
                left < this.zeroPos.x
                || up > this.zeroPos.y
                || right > this.maxPos.x
                || down < this.maxPos.y
                )
            {
                print("나갔음");
                this.transform.localPosition = this.originPos;
            }
            else
            {
                Vector3 temp = new Vector3((left + right) / 2f, (up + down) / 2f, 0f);
                inventoryObject.Inst.setItemPos(this.gameObject, temp);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isEquip = !isEquip;
            equip.SetActive(isEquip);
        }
    }
    int temp = 0;
    void Update()
    {   
        if (isHover)
        {
            if (Input.GetMouseButtonDown(0))
            { //좌클릭
                //print("마우스 좌클릭");
                originPos = this.transform.localPosition;
            }
            else if (Input.GetMouseButtonDown(1))
            { //우클릭
                //print("마우스 우클릭");
            }
            else
            {//호버
                if (temp == 0)
                {
                    inventoryObject.Inst.itemHover(this);
                    //print("마우스 호버");
                    temp = 1;
                }
            }
        }
        else
            temp = 0;
    }
}
