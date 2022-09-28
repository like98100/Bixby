using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void Setup(float sizeX, float sizeY, float posX, float posY, bool isEquip, float cell, Vector3 zero, itemData itemData)
    {
        this.zeroPos = zero;
        maxPos = zeroPos + (Vector3.right * inventoryObject.Inst.xSize * cell) + (Vector3.down * inventoryObject.Inst.ySize * cell);

        this.size = new Vector3(sizeX * 100f, sizeY * 100f, 1f);
        this.GetComponent<RectTransform>().sizeDelta = this.size;

        this.originPos = new Vector3(posX, posY, 0f);
        this.transform.localPosition = new Vector3(
                    zeroPos.x + originPos.x * cell + size.x / 2f,
                    zeroPos.y - originPos.y * cell - size.y / 2f);

        this.isEquip = isEquip;
        equip.SetActive(isEquip);

        this.itemData = itemData;
    }
    public void Hover(bool hover)
    {
        isHover = hover;
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
        print(left + " " + right + " " + up + " " + down);
        if (Input.GetMouseButtonUp(0))
        { //위치 조정
            float tempL = (left + 50f) % 100f;
            left -= tempL;
            right -= tempL;
            if (tempL < -50)
            {
                left -= 100;
                right -= 100;
            }
            else if (tempL > 50)
            {
                left += 100;
                right += 100;
            }

            float tempU = (up + 50f) % 100f;
            up -= tempU;
            down -= tempU;
            if (tempU < -50)
            {
                up -= 100;
                down -= 100;
            }
            else if (tempU > 50)
            {
                up += 100;
                down += 100;
            }
            print(left + " " + right + " " + up + " " + down);
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
                    //print("마우스 호버");
                    temp = 1;
                }
            }
        }
        else
            temp = 0;
    }
}
