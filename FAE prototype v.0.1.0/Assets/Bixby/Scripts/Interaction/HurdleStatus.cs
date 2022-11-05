using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleStatus : MonoBehaviour
{
    public float HurdleDamage = 10.0f;
    public float moveSpeed; //�����̴� ��ֹ� �ӵ�
    public float downPlayerSpeed; //�÷��̾� �پ��� �ӵ�

    public List<GameObject> traget;
    public int nowTraget = 0; //���� ��ȣ
    public bool isMove = false; //���� �����̰� �ִ���

    public List<GameObject> nearobj;


    public HURDLE_STATE State = HURDLE_STATE.NONE;


    public enum HURDLE_STATE
    {
        NONE = -1, //�ƹ����� �ƴѰ�
        HURDLE = 0, //��ֹ�
        SLOW_HURDLE = 1, //�������� ��ֹ�
        NEAR_HURDLE = 2, //������ ���� �����Ǵ� ��ֹ�
        MOVE_HURDLE = 3  //�����̴���ֹ�
    }


    // Update is called once per frame
    void Update()
    {
        if (State == HURDLE_STATE.MOVE_HURDLE)
        {
            move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�÷��̾�� ������ ������
        if (other.gameObject.tag == "Player" && State == HURDLE_STATE.HURDLE)
        {
            //�̺κ��� ��ĥ�� �ٸ� �Լ� ����ؼ� ����
            other.GetComponent<PlayerStatusControl>().Health -= HurdleDamage;
        }

        //�÷��̾� ���ο�
        if (other.gameObject.tag == "Player" && State == HURDLE_STATE.SLOW_HURDLE)
        {
            //�̺κ��� ��ĥ�� �ٸ� �Լ� ����ؼ� ����
            other.GetComponent<PlayerStatusControl>().MyCurrentSpeed -= downPlayerSpeed;
        }
    }

    //���� ���� �ȿ� ������ ������ ���� �� ����� ������Ʈ ����
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && State == HURDLE_STATE.NEAR_HURDLE)
        {
            for (int i = 0; i < nearobj.Count; i++)
            {
                nearobj[i].gameObject.SetActive(true);
            }
        }
    }

    //�������°� ����
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && State == HURDLE_STATE.SLOW_HURDLE)
        {
            other.GetComponent<PlayerStatusControl>().MyCurrentSpeed += downPlayerSpeed;
        }

        if (other.gameObject.tag == "Player" && State == HURDLE_STATE.NEAR_HURDLE)
        {
            for (int i = 0; i < nearobj.Count; i++)
            {
                nearobj[i].gameObject.SetActive(false);
            }
        }
    }

    private void move()
    {
        //���� Ÿ���� �������� ������Ʈ�� �������ϰ� ��������
        if (transform.position == traget[nowTraget].transform.position)
        {
            nowTraget = (nowTraget + 1) % traget.Count;
        }

        //��������
        transform.position = Vector3.MoveTowards(transform.position, traget[nowTraget].transform.position, moveSpeed);
    }
}
