using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldItem : MonoBehaviour
{
    public ItemData ItemData;
    float angle;
    [SerializeField] GameObject keyF;
    bool isPlayerClose;
    void Start()
    {
        this.transform.localScale = new Vector3(ItemData.XSize, ItemData.YSize, 1f);
        Vector3 tempPos = this.transform.position;
        tempPos.y = ItemData.YSize / 2f;
        this.transform.position = tempPos;
        isPlayerClose = false;
    }
    public void Setup(ItemData data)
    {
        ItemData = data;
        angle = 0f;
        //keyInst = null;
        this.transform.localScale = new Vector3(ItemData.XSize, ItemData.YSize, 1f);
        Vector3 tempPos = this.transform.position;
        tempPos.y = ItemData.YSize / 2f;
        this.transform.position = tempPos;
    }
    // Update is called once per frame
    void Update()
    {
        angle += 60 * Time.deltaTime;
        this.transform.localEulerAngles = new Vector3(0f, angle, 15f);
        if (isPlayerClose && Input.GetKeyDown(KeyCode.F))
            inventoryObject.Inst.GetFieldItem(this.gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (inventoryObject.Inst.FieldFKey == null)
            {
                isPlayerClose = true;
                inventoryObject.Inst.FieldFKey = Instantiate(keyF, GameObject.Find("Canvas").transform);
                var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
                inventoryObject.Inst.FieldFKey.transform.position = wantedPos + Vector3.right * 200f;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag=="Player")
        {
            Destroy(inventoryObject.Inst.FieldFKey);
            inventoryObject.Inst.FieldFKey = null;
            isPlayerClose = false;
        }
    }
}
