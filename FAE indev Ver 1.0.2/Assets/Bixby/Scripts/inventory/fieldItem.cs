using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldItem : MonoBehaviour
{
    public itemData ItemData;
    float angle;
    bool isPlayerClose;
    [SerializeField] List<Mesh> foodMesh;
    [SerializeField] List<Material> foodMaterial;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    void Start()
    {
        
    }
    public void setup(itemData data)
    {
        isPlayerClose = false;
        ItemData = data;
        angle = 0f;
        meshFilter = this.gameObject.GetComponent<MeshFilter>();
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        Vector3 tempPos = this.transform.position;
        tempPos += Vector3.up * 0.5f;
        //tempPos.y = itemData.ySize / 2f;
        this.transform.position = tempPos;
        if (this.ItemData.itemID >= 1000 && this.ItemData.itemID < 2000)
            meshFilter.mesh = foodMesh[this.ItemData.itemID - 1000];
        else if (this.ItemData.itemID > 2000 && this.ItemData.itemID < 3000)
            meshFilter.mesh = foodMesh[this.ItemData.itemID - 2000 + 4];
        else
            this.transform.localScale = new Vector3(ItemData.xSize, ItemData.ySize, 1f);
        meshRenderer.material.color = Color.white;
        switch (ItemData.itemID)
        {
            case 1000:
                meshRenderer.material = foodMaterial[0];
                this.transform.localScale = Vector3.one * 4f;
                this.transform.GetComponent<BoxCollider>().size = Vector3.one * 0.5f;
                break;
            case 1001:
                meshRenderer.material = foodMaterial[0];
                this.transform.localScale = Vector3.one * 4f;
                this.transform.GetComponent<BoxCollider>().size = Vector3.one;
                break;
            case 1002:
                meshRenderer.material = foodMaterial[0];
                this.transform.localScale = Vector3.one * 2f;
                this.transform.GetComponent<BoxCollider>().size = Vector3.one;
                break;
            case 1003:
            case 1004:
            case 2001:
            case 2002:
            case 2003:
            case 2004:
                meshRenderer.material = foodMaterial[1];
                break;
            case 2000:
                meshFilter.mesh = foodMesh[0];
                this.transform.localScale = new Vector3(5f, 3f, 5f);
                this.transform.GetComponent<BoxCollider>().size = Vector3.one * 0.5f;
                meshRenderer.material.color = Color.black;
                break;
            default:
                this.transform.localScale = new Vector3(ItemData.xSize / 4f, ItemData.ySize / 4f, 1 / 4f);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        angle += 60 * Time.deltaTime;
        this.transform.localEulerAngles = new Vector3(0f, angle, 15f);
        if (isPlayerClose)
        {
            if (Input.GetKeyDown(KeyCode.F))
                inventoryObject.Inst.getFieldItem(this.gameObject);
            if (inventoryObject.Inst.FieldFKey.activeSelf)
            {
                var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
                inventoryObject.Inst.FieldFKey.transform.position = wantedPos + Vector3.right * 200f;
            }
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
