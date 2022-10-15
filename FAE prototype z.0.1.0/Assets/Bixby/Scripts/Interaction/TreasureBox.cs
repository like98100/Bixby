using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    public bool boxState; //���� Ȱ��ȭ ��Ȱ��ȭ ����
    public Material[] mBox; //0.��Ȱ��ȭ, 1.Ȱ��ȭ, ���� ���׸��� ���߿� ���̴��� ����

    public bool open; //���ȴ��� �ƴ���

    public GameObject[] contents; //���빰 ������ ���̽��� ��� ���������� �߿�

    public GameObject Player; //�÷��̾� ������Ʈ
    public GameObject coin; //���� ������Ʈ

    private void Awake()
    {
        // Player = GameObject.FindGameObjectWithTag("Player");
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

    void BoxOpen()
    {
        //������ ���� ��
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f && boxState)
        {
            //UI ���� fŰ ������ ������

            if (Input.GetKeyDown("g"))
            {
                //���ڿ���
                open = true;
                //������ ȹ��, UI?
                

                for (int i = 0; i < 5; i++)
                {
                    float RandomX = Random.Range(-3 + transform.position.x, 3 + transform.position.x);
                    float RandomZ = Random.Range(-3 + transform.position.z, 3 + transform.position.z);

                    Vector3 RandomPos = new Vector3(RandomX, transform.position.y, RandomZ);

                    Instantiate(coin, RandomPos, Quaternion.identity);
                }


                //���� ����?
                Destroy(gameObject);
            }
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
