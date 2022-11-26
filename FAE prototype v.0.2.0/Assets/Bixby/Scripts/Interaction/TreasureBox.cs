using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    public bool boxState; //���� Ȱ��ȭ ��Ȱ��ȭ ����
    public bool open; //���ȴ��� �ƴ��� 
    public bool checkNear; //������ �ִ��� üũ�ϴ� ����

    public int coinCount; //������ ��� ��

    public Material[] mBox; //0.��Ȱ��ȭ, 1.Ȱ��ȭ, ���� ���׸��� ���߿� ���̴��� ����

    public GameObject[] contents; //���빰 ������ ���̽��� ��� ���������� �߿� -> �̺κ��� ���̽� ������ �����͸� �����;� �� �� ����

    public GameObject Player; //�÷��̾� ������Ʈ
    public GameObject coin; //���� ������Ʈ

    private GameObject top; //���� �Ӹ��κ�

    itemJsonData itemJsonData;//json������

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "shop");//json�ε�
        Player = GameObject.FindGameObjectWithTag("Player");
        open = false;

        top = transform.GetChild(0).gameObject;
        Quaternion currentRotation = top.transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���ڰ� Ȱ��ȭ ���� �� �ϴ°� ���δ�.

        //BoxMaterial(); //���̴�or���׸��� ����
        BoxOpen(); //���� ����
    }

    void BoxOpen()
    {
        //������ ���� ��
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f && boxState && !open)
        {
            checkNear = true;

            //fŰ ����
            if (!inventoryObject.Inst.FieldFKey.activeSelf)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.F) && !open)
            {
                //���ڿ���
                open = true;
                //������ ȹ��, UI?

                //������ ���� contents �� �� ���ڸ� ������ �� ȹ�� �� �� �ִ� ������ ����ֱ� Ȯ����� �̷��� ���߿�
                //GameObject TreasureItem = inventoryObject.Inst.MakeFieldItem(itemJsonData.itemList[0], transform.position);
                //TreasureItem.GetComponent<fieldItem>().setup(itemJsonData.itemList[0], true);

                //���� ����
                for (int i = 0; i < coinCount; i++)
                {
                    int RandomX = Random.Range(-40, 40);
                    int RandomY = Random.Range(0, 360);
                    int RandomZ = Random.Range(50, 130);

                    GameObject _coin = Instantiate(coin, transform.position + new Vector3(0, 2, 0), Quaternion.Euler(new Vector3(RandomX, RandomY, RandomZ)));

                    _coin.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse); //���� ���ֱ�
                    _coin.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 5, ForceMode.Impulse); //�����ִ� �������� ���ֱ�
                }

                //�������·� ���α�
                StartCoroutine(BoxAni());

                //fŰ ����
                inventoryObject.Inst.FieldFKey.SetActive(false);
                checkNear = false;
            }
        }
        else if (Vector3.Distance(Player.transform.position, transform.position) > 2.0f && boxState && checkNear == true)
        {
            //fŰ ����
            inventoryObject.Inst.FieldFKey.SetActive(false);

            checkNear = false;
        }
    }

    //����Ʈ Ű�°ɷ� ����
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

    //�ڽ� ������ �� �ڷ�ƾ
    IEnumerator BoxAni()
    {
        for (int i = 0; i < 60; i++)
        {
            top.transform.Rotate(new Vector3(-1, 0, 0));
            yield return null;
        }
    }
}
