using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldItem : MonoBehaviour
{
    public itemData itemData;
    float angle;
    [SerializeField] GameObject keyF;
    GameObject keyInst;
    public int index;
    void Start()
    {
        this.transform.localScale = new Vector3(itemData.xSize, itemData.ySize, 1f);
        Vector3 tempPos = this.transform.position;
        tempPos.y = itemData.ySize / 2f;
        this.transform.position = tempPos;
    }
    public void setup(itemData data)
    {
        itemData = data;
        angle = 0f;
        keyInst = null;
        this.transform.localScale = new Vector3(itemData.xSize, itemData.ySize, 1f);
        Vector3 tempPos = this.transform.position;
        tempPos.y = itemData.ySize / 2f;
        this.transform.position = tempPos;
        index = inventoryObject.Inst.existFieldItems.Count > 0 ?//인덱스 부여, 리스트가 비었을 경우 0으로, 아닐 경우 기존 리스트 내의 인덱스 +1
            inventoryObject.Inst.existFieldItems[inventoryObject.Inst.existFieldItems.Count - 1].GetComponent<fieldItem>().index + 1 : 0;
        inventoryObject.Inst.existFieldItems.Add(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        angle += 60 * Time.deltaTime;
        this.transform.localEulerAngles = new Vector3(0f, angle, 15f);
    }
    private void OnDestroy()
    {
        Destroy(keyInst);
        keyInst = null;
        inventoryObject.Inst.existFieldItems.Remove(this.gameObject);
        inventoryObject.Inst.SetFieldItemIndex();//인덱스 재설정
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && keyInst == null)
        {
            keyInst = Instantiate(keyF, GameObject.Find("Canvas").transform);
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            keyInst.transform.position = wantedPos + Vector3.right * 200f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag=="Player")
        {
            Destroy(keyInst);
            keyInst = null;
        }
    }
}
