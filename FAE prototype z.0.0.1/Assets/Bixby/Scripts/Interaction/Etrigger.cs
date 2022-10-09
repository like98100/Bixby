using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etrigger : MonoBehaviour
{
    public int element; //������� ����
    public int nowElement; //���� ����
    public bool state; //���� �ִ��� �ƴ���

    public Material[] m_element; //���� ���׸��� ���߿� ���̴��� ����ɵ�

    private void Awake()
    {
        nowElement = 0;
        state = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //����ü ������ �־�� �ҰŰ���.
        if (other.tag == "projectile")
        {
            switch (other.GetComponent<Projectile>().projectileState)
            {
                case 0:
                    nowElement = 0;
                    break;
                case 1:
                    nowElement = 1;
                    break;
                case 2:
                    nowElement = 2;
                    break;
                case 3:
                    nowElement = 3;
                    break;
                case 4:
                    nowElement = 4;
                    break;
                default:
                    break;
            }
        }
        //TriggerState();
    }

    // Update is called once per frame
    void Update()
    {
        //������Ʈ���� ���� Ʈ�����ʿ� �־����
        TriggerState();
    }

    public void TriggerState()
    {
        //������ҿ� ������� ���Ұ� ���� ��
        if (element == nowElement)
        {
            switch (element)
            {
                case 0:
                    //null
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[0];
                    break;
                case 1:
                    //��
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[1];
                    break;
                case 2:
                    //��
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[2];
                    break;
                case 3:
                    //����
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[3];
                    break;
                case 4:
                    //����
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[4];
                    break;
                default:
                    break;
            }

            state = true;
        }
    }
}
