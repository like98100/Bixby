using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    public float respawnTime = 5.0f; //������ �ð�
    public GameObject field; //������ ���� ������ ������Ʈ

    public HARVESTSTATE state = HARVESTSTATE.NONE;

    public enum HARVESTSTATE
    {
        NONE = 0,
        FRUIT = 1, //����
        GREENONION = 2, //��
        RICE = 3, //��
    };

    //json������
    itemJsonData itemJsonData;
    //������ ������
    itemData harvestData;

    //private MeshRenderer meshTest; //�Ž� ������

    public bool testchack = true; //������Ʈ ���� üũ
    bool isPlayerClose;

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "harvest");//json�ε�
    }

    void Start()
    {
        harvestData = new itemData();

        switch (state)
        {
            case HARVESTSTATE.NONE:
                break;
            case HARVESTSTATE.FRUIT:
                //������ ���� ��ũ��Ʈ
                foreach (var item in itemJsonData.itemList)
                {
                    if (item.itemID == 1000)
                    {
                        harvestData = item;
                    }
                }
                break;
            case HARVESTSTATE.GREENONION:
                //������ ���� ��ũ��Ʈ
                foreach (var item in itemJsonData.itemList)
                {
                    if (item.itemID == 1001)
                    {
                        harvestData = item;
                    }
                }
                break;
            case HARVESTSTATE.RICE:
                //������ ���� ��ũ��Ʈ
                foreach (var item in itemJsonData.itemList)
                {
                    if (item.itemID == 1002)
                    {
                        harvestData = item;
                    }
                }
                break;
            default:
                break;
        }
        isPlayerClose = false;
    }

    private void Update()
    {
        if (isPlayerClose && inventoryObject.Inst.FieldFKey.activeSelf)
        {
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            inventoryObject.Inst.FieldFKey.transform.position = wantedPos + Vector3.right * 200f;
        }
        if (Input.GetKeyDown(KeyCode.F) && isPlayerClose)
        {
            Vector2 tempPos;
            //�� ���� ã��
            tempPos = inventoryObject.Inst.emptyCell(harvestData.xSize, harvestData.ySize);
            inventoryObject.Inst.itemGet(harvestData.xSize, harvestData.ySize, tempPos.x, tempPos.y, harvestData);

            //�κ��丮 �߰� �� ���̽� ����
            inventoryObject.Inst.jsonSave();
            inventoryObject.Inst.FieldFKey.SetActive(false);

            isPlayerClose = false;
            this.gameObject.SetActive(false);
            testchack = false;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //fŰ ����
            if (!inventoryObject.Inst.FieldFKey.activeSelf)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
                isPlayerClose = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //fŰ ����
            inventoryObject.Inst.FieldFKey.SetActive(false);
            isPlayerClose = false;
        }
    }
}
