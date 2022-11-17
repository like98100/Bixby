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



    //������ ������
    itemData harvestData;

    private MeshRenderer meshTest; //�Ž� ������

    void Start()
    {
        harvestData = new itemData();

        switch (state)
        {
            case HARVESTSTATE.NONE:
                break;
            case HARVESTSTATE.FRUIT:
                //������ ���� ��ũ��Ʈ
                harvestData.itemID = 1000; harvestData.tag = new string[] { "food", "harvest" }; harvestData.itemName = "����";
                harvestData.Left = -1; harvestData.Up = -1; harvestData.xSize = 1; harvestData.ySize = 1;
                harvestData.isEquip = false; harvestData.isSell = false;
                harvestData.price = 1; //���߿� ���� ����
                break;
            case HARVESTSTATE.GREENONION:
                //������ ���� ��ũ��Ʈ
                harvestData.itemID = 1001; harvestData.tag = new string[] { "food", "harvest" }; harvestData.itemName = "��";
                harvestData.Left = -1; harvestData.Up = -1; harvestData.xSize = 1; harvestData.ySize = 1;
                harvestData.isEquip = false; harvestData.isSell = false;
                harvestData.price = 1; //���߿� ���� ����
                break;
            case HARVESTSTATE.RICE:
                //������ ���� ��ũ��Ʈ
                harvestData.itemID = 1002; harvestData.tag = new string[] { "food", "harvest" }; harvestData.itemName = "��";
                harvestData.Left = -1; harvestData.Up = -1; harvestData.xSize = 1; harvestData.ySize = 1;
                harvestData.isEquip = false; harvestData.isSell = false;
                harvestData.price = 1; //���߿� ���� ����
                break;
            default:
                break;
        }

        


        meshTest = gameObject.GetComponent<MeshRenderer>();
    }


    //���ÿ� ȹ���ϴ°� �����ؾ��� -> ���� ������ �ִ� �� �� ���� ��������?
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);

        if (other.gameObject.tag == "Player")
        {
            //Debug.Log(other.name);

            //fŰ ����
            if (!inventoryObject.Inst.FieldFKey.activeSelf && this.meshTest.enabled == true)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
            }

            

            if (Input.GetKeyDown(KeyCode.F) && meshTest.enabled == true)
            {
                //Debug.Log("cccc");
                this.meshTest.enabled = false;

                StartCoroutine(respawn());
                //������ ȹ�� -> �κ��丮�� ����

                Vector2 tempPos;
                //�� ���� ã��
                tempPos = inventoryObject.Inst.emptyCell(harvestData.xSize, harvestData.ySize);
                inventoryObject.Inst.itemGet(harvestData.xSize, harvestData.ySize, tempPos.x, tempPos.y, harvestData);

                //�κ��丮 �߰� �� ���̽� ����
                inventoryObject.Inst.jsonSave();
                inventoryObject.Inst.FieldFKey.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inventoryObject.Inst.FieldFKey.SetActive(false);
        }
    }


    IEnumerator respawn()
    {
        //gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnTime);

        //��ġ ���� �̵�
        transform.position = field.GetComponent<HarvestField>().Return_RandomPosition();

        //gameObject.SetActive(true);
        this.meshTest.enabled = true;

    }
}
