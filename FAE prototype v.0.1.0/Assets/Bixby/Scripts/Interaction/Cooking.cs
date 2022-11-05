using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooking : MonoBehaviour
{
    //변수이름 바꿔야함
    public Text num;
    public int myNum; //어떤 요리인지

    public GameObject cookPanel;

    public GameObject Player; //플레이어 오브젝트

    public enum COOK
    {
        NONE = 0,
        FRUITJUICE = 1,
        KOREANFOOD = 2,
    }


    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //요리 창 띄우기 -> ui쪽이랑 연동 할 때 오브젝트다.
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f)
        {
            //f키 생성
            if (inventoryObject.Inst.FieldFKey == null)
            {
                inventoryObject.Inst.FieldFKey = Instantiate(inventoryObject.Inst.getObj("KeyF"), GameObject.Find("Canvas").transform);
                var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
                inventoryObject.Inst.FieldFKey.transform.position = wantedPos + Vector3.right * 200f;
            }


            if (Input.GetKeyDown(KeyCode.F))
            {
                //UI창이 켜졌을때
                if (cookPanel.gameObject.activeSelf == true)
                {
                    cookPanel.gameObject.SetActive(false);
                }
                else
                {
                    cookPanel.gameObject.SetActive(true);
                    Destroy(inventoryObject.Inst.FieldFKey);
                    inventoryObject.Inst.FieldFKey = null;
                }
            }
        }
    }

  
    //수정해야함
    public void openCook()
    {
        //요리 UI 해당하는거 추가

        if (myNum == 1)
        {
            num.text = "과일주스";
        }
        else if (myNum == 2)
        {
            num.text = "한식";
        }
    }
}
