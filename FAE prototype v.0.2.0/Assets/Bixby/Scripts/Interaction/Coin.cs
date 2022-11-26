using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] protected float coinSpeed; //ȹ���ϴ� ���� �ӵ�
    [SerializeField] protected float coinPower; //���� ������ �� ���ο� �ִ� ��
    [SerializeField] protected float cognitiveRange;//������� ����

    [SerializeField] protected int acheiveGold; //ȹ�� ���

    bool act; //Ȱ��ȭ �Ȱ��� �ƴ���

    float angle;

    GameObject Player; //�÷��̾� ������Ʈ

    Rigidbody rd;
    public Collider coinCollider;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rd = GetComponent<Rigidbody>();

        act = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ȸ��
        angle += 90 * Time.deltaTime;
        this.transform.localEulerAngles = new Vector3(0f, angle, 90f);

        //��������� �Ծ��
        if (Vector3.Distance(Player.transform.position, transform.position) <= cognitiveRange && act)
        {
            //�߷�����
            this.rd.useGravity = false;
            this.rd.isKinematic = true;
            //����ϰ� �����
            coinCollider.isTrigger = true;

            transform.LookAt(Player.transform.localPosition);
            //transform.Translate(coinSpeed * Vector3.forward * Time.deltaTime);

            transform.position = Vector3.Slerp(transform.position, Player.transform.position + new Vector3(0, 1, 0), coinSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            act = true;
            coinCollider.isTrigger = false;
        }

        if (other.tag == "Player" && act == true)
        {
            //�κ��丮�� ��� ����
            inventoryObject.Inst.Gold += acheiveGold;
            //���̽� ����
            inventoryObject.Inst.jsonSave();
            Destroy(gameObject);
        }
    }
}
