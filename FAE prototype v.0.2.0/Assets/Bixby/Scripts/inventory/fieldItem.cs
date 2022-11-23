using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldItem : MonoBehaviour
{
    public itemData ItemData;
    float angle;
    bool isPlayerClose;
    void Start()
    {
        this.transform.localScale = new Vector3(ItemData.xSize, ItemData.ySize, 1f);
        Vector3 tempPos = this.transform.position;
        //tempPos.y = itemData.ySize / 2f;
        this.transform.position = tempPos;
        isPlayerClose = false;
    }
    public void setup(itemData data)
    {
        ItemData = data;
        angle = 0f;
        //keyInst = null;
        this.transform.localScale = new Vector3(ItemData.xSize, ItemData.ySize, 1f);
        Vector3 tempPos = this.transform.position;
        //tempPos.y = itemData.ySize / 2f;
        this.transform.position = tempPos;
    }
    // Update is called once per frame
    void Update()
    {
        angle += 60 * Time.deltaTime;
        this.transform.localEulerAngles = new Vector3(0f, angle, 15f);
        if (isPlayerClose && Input.GetKeyDown(KeyCode.F))
            inventoryObject.Inst.getFieldItem(this.gameObject);
        if (isPlayerClose && inventoryObject.Inst.FieldFKey.activeSelf)
        {
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            inventoryObject.Inst.FieldFKey.transform.position = wantedPos + Vector3.right * 200f;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!inventoryObject.Inst.FieldFKey.activeSelf)
            {
                isPlayerClose = true;
                inventoryObject.Inst.FieldFKey.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag=="Player")
        {
            inventoryObject.Inst.FieldFKey.SetActive(false);
            isPlayerClose = false;
        }
    }
}
