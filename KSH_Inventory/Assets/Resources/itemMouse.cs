using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemMouse : MonoBehaviour
{
    ItemSO itemInfo;
    [SerializeField] GameObject image;
    bool isHover;
    public void Hover(bool hover)
    {
        isHover = hover;
    }
    public void Drag()
    {
        print("드래그");
        this.transform.position = Input.mousePosition;//Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void Up()
    {
        Rect thisRect = this.GetComponent<RectTransform>().rect;
        if (Input.GetMouseButtonUp(0))
        { //위치 조정

            if ((this.transform.position.x + thisRect.xMax <
                this.transform.parent.transform.position.x+this.transform.parent.GetComponent<RectTransform>().rect.xMin)
                ||(this.transform.position.y + thisRect.yMax <
                this.transform.parent.transform.position.y+this.transform.parent.GetComponent<RectTransform>().rect.yMin)
                || (this.transform.position.x + thisRect.xMin >
                this.transform.parent.transform.position.x + this.transform.parent.GetComponent<RectTransform>().rect.xMax)
                || (this.transform.position.y + thisRect.yMin >
                this.transform.parent.transform.position.y + this.transform.parent.GetComponent<RectTransform>().rect.yMax))
            {
                print("나갔음");
            }
            else
            {

            }
        }
        else if (Input.GetMouseButtonUp(1))
            print("아이템 사용");
    }
    void Setup(ItemSO itemSO)
    {
        this.itemInfo = itemSO;
    }
    int temp = 0;
    void Update()
    {   
        if (isHover)
        {
            if (Input.GetMouseButtonDown(0))
            { //좌클릭
                print("마우스 좌클릭");
            }
            else if (Input.GetMouseButtonDown(1))
            { //우클릭
                print("마우스 우클릭");
            }
            else
            {//호버
                if (temp == 0)
                {
                    print("마우스 호버");
                    temp = 1;
                }
            }
        }
        else
            temp = 0;
    }
}
