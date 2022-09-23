using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemObject : MonoBehaviour
{
    [SerializeField] GameObject image;
    bool isHover;
    Vector3 originPos;
    Rect thisRect;
    [SerializeField] inventoryObject inventory;
    bool isEquip;
    [SerializeField] GameObject equip;
    int width, height;
    private void Start()
    {
        thisRect = this.GetComponent<RectTransform>().rect;
        isEquip = false;
    }
    public void Setup()
    {
        this.transform.localScale = new Vector3(width * 100f, height * 100f, 1f);
    }
    public void Hover(bool hover)
    {
        isHover = hover;
    }
    public void Drag()
    {
        print("�巡��");
        this.transform.position = Input.mousePosition;//Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void Up()
    {
        float left, right, up, down;
        left = this.transform.localPosition.x + thisRect.xMin;
        right = this.transform.localPosition.x + thisRect.xMax;
        up = this.transform.localPosition.y + thisRect.yMax;
        down = this.transform.localPosition.y + thisRect.yMin;
        if (Input.GetMouseButtonUp(0))
        { //��ġ ����

            if ((right <
                (this.transform.parent.GetComponent<RectTransform>().rect.width / -2f) + 100f)
                || (up <
                (this.transform.parent.GetComponent<RectTransform>().rect.height / -2f) + 100f)
                || (left >
                (this.transform.parent.GetComponent<RectTransform>().rect.width / 2f) - 100f)
                || (down >
                (this.transform.parent.GetComponent<RectTransform>().rect.height / 2f) - 100f))
            {
                print("������");
                this.transform.position = originPos;
            }
            else
            {
                float tempL = (left + 50f) % 100f;
                if (tempL < 0)
                {
                    this.transform.position += Vector3.left * tempL;
                    if (tempL < -50)
                        this.transform.position += Vector3.left * 100f;
                }
                else if (tempL > 0)
                {
                    this.transform.position += Vector3.left * tempL;
                    if (tempL > 50)
                        this.transform.position += Vector3.left * -100f;
                }
                float tempU = (up + 50f) % 100f;
                print("y : " + this.transform.localPosition.y + ", tempU : " + tempU);
                if (tempU > 0)
                {
                    this.transform.position -= Vector3.up * tempU;
                    if (tempU > 50)
                        this.transform.position -= Vector3.up * -100f;
                }
                else if (tempU < 0)
                {
                    this.transform.position -= Vector3.up * tempU;
                    if (tempU < -50)
                        this.transform.position -= Vector3.up * 100f;
                }
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
            { //��Ŭ��
                print("���콺 ��Ŭ��");
                originPos = this.transform.position;
            }
            else if (Input.GetMouseButtonDown(1))
            { //��Ŭ��
                print("���콺 ��Ŭ��");
            }
            else
            {//ȣ��
                if (temp == 0)
                {
                    print("���콺 ȣ��");
                    temp = 1;
                }
            }
        }
        else
            temp = 0;
    }
}
