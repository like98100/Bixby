using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etrigger : MonoBehaviour
{
    //public int element; //������� ����
    //public int nowElement; //���� ����
    public bool state; //���� �ִ��� �ƴ���

    public ElementRule.ElementType element; //������� ����
    public ElementRule.ElementType nowElement; //���� ����

    public Material[] m_element; //���� ���׸��� ���߿� ���̴��� ����ɵ�

    private void Awake()
    {
        //nowElement = 0;
        state = false;
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
                case ElementRule.ElementType.NONE:
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[0];
                    break;
                case ElementRule.ElementType.FIRE:
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[1];
                    break;
                case ElementRule.ElementType.ICE:
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[2];
                    break;
                case ElementRule.ElementType.WATER:
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[3];
                    break;
                case ElementRule.ElementType.ELECTRICITY:
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[4];
                    break;
                case ElementRule.ElementType.NUM:
                    break;
                default:
                    break;
            }

            state = true;
        }
    }

    public void GetElement(ElementRule.ElementType elemet)
    {
        nowElement = elemet;
    }
}
