using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    public bool boxState; //���� Ȱ��ȭ ��Ȱ��ȭ ����
    public Material[] mBox; //0.��Ȱ��ȭ, 1.Ȱ��ȭ, ���� ���׸��� ���߿� ���̴��� ����

    public bool open; //���ȴ��� �ƴ���

    public GameObject[] contents; //���빰 ������ ���̽��� ��� ���������� �߿� -> �̺κ��� ���̽� ������ �����͸� �����;� �� �� ����

    public GameObject Player; //�÷��̾� ������Ʈ
    public GameObject coin; //���� ������Ʈ


    itemJsonData itemJsonData;//json������

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "shop");//json�ε�
        Player = GameObject.FindGameObjectWithTag("Player");
        open = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���ڰ� Ȱ��ȭ ���� �� �ϴ°� ���δ�.

        BoxMaterial(); //���̴�or���׸��� ����
        BoxOpen(); //���� ����
    }


    //���� ���� 
    public bool a = true;

    void BoxOpen()
    {
        //������ ���� �� -> ������ �ڵ��� FŰ UI�� ������ ȹ���ϴ°ɷ� ����
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f && boxState)
        {
            a = false;

            //UI ���� fŰ ������ ������
            //fŰ ����
            if (!inventoryObject.Inst.FieldFKey.activeSelf)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
            }


            if (Input.GetKeyDown(KeyCode.F))
            {
                //���ڿ���
                open = true;
                //������ ȹ��, UI?

                //������ ���� contents �� �� ���ڸ� ������ �� ȹ�� �� �� �ִ� ������ ����ֱ� Ȯ����� �̷��� ���߿�
                inventoryObject.Inst.MakeFieldItem(itemJsonData.itemList[0], transform.position);


                ////���� ���� -> �ٸ��ɷ� �ٲ���
                //for (int i = 0; i < 5; i++)
                //{
                //    float RandomX = Random.Range(-3 + transform.position.x, 3 + transform.position.x);
                //    float RandomZ = Random.Range(-3 + transform.position.z, 3 + transform.position.z);

                //    Vector3 RandomPos = new Vector3(RandomX, transform.position.y, RandomZ);

                //    Instantiate(coin, RandomPos, Quaternion.identity);
                //}


                //���� ����? -> �������·� ���α�
                Destroy(gameObject);
            }
        }
        else if (Vector3.Distance(Player.transform.position, transform.position) > 2.0f && boxState && a == false)
        {
            inventoryObject.Inst.FieldFKey.SetActive(false);

            a = true;
        }
    }

    void BoxMaterial()
    {
        if (boxState) //Ȱ��ȭ
        {
            //�������� ���̴� �������� �ٲ���Ѵ�.
            GetComponent<MeshRenderer>().material = mBox[1];
        }
        else if (!boxState) //��Ȱ��ȭ
        {
            //�������� ���̴� �������� �ٲ���Ѵ�.
            GetComponent<MeshRenderer>().material = mBox[0];
        }
    }
}
