using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject Player; //�÷��̾� ������Ʈ
    public float coinSpeed; //���� �ӵ�

    private void Awake()
    {
        // Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.transform.position,transform.position) <= 3.0f)
        {
            transform.LookAt(Player.transform);
            //transform.Translate(coinSpeed * Vector3.forward * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, Player.transform.position, coinSpeed);
        }
    }

    //��ǻ���� ���� ������ �ν��� �ȵ� �� stay�� ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //�κ��丮�� ��� ����
            inventoryObject.Inst.Gold++;
            Destroy(gameObject);
        }
    }
}
