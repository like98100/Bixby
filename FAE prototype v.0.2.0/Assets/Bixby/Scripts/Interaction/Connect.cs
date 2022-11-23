using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect : MonoBehaviour
{
    public bool state; //���� �ִ��� �ƴ���
    public float moveSpeed; //�����̴� �ӵ�


    public GameObject[] otherConnect; //�ٸ� ���� ������Ʈ

    public Material[] m_connect; //���� ������Ʈ ���׸���


    public Vector3 HitPoint;
    public Vector3 HitNormal;

    public NORMAL nor = NORMAL.NONE;

    public enum NORMAL
    {
        NONE = 0,
        X_minus = 1,
        X_plus = 2,
        Y_minus = 3,
        Y_plus = 4,
        Z_minus = 5,
        Z_plus = 6,
    };


    private void Awake()
    {
        state = false;
        this.gameObject.GetComponent<MeshRenderer>().material = m_connect[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveConnect();
        stateCheck();
    }



    void moveConnect()
    {
        //������ ������
        if (HitNormal != new Vector3(0, 0, 0))
        {
            if (HitNormal.x == -1)
            {
                //���� ����
                nor = NORMAL.X_minus;
                Debug.Log("X-");
                HitNormal = new Vector3(0, 0, 0);
            }
            else if (HitNormal.x == 1)
            {
                //���� ����
                nor = NORMAL.X_plus;
                Debug.Log("X+");
                HitNormal = new Vector3(0, 0, 0);
            }
            else if (HitNormal.y == -1)
            {
                //���� ����
                nor = NORMAL.Y_minus;
                Debug.Log("Y-");
                HitNormal = new Vector3(0, 0, 0);
            }
            else if (HitNormal.y == 1)
            {
                //���� ����
                nor = NORMAL.Y_plus;
                Debug.Log("Y-");
                HitNormal = new Vector3(0, 0, 0);
            }
            else if (HitNormal.z == -1)
            {
                //���� ����
                nor = NORMAL.Z_minus;
                Debug.Log("Z-");
                HitNormal = new Vector3(0, 0, 0);
            }
            else if (HitNormal.z == 1)
            {
                //���� ����
                nor = NORMAL.Z_plus;
                Debug.Log("Z-");
                HitNormal = new Vector3(0, 0, 0);
            }
            else
            {
                Debug.Log("����");
            }
        }


        switch (nor)
        {
            case NORMAL.NONE:
                break;
            case NORMAL.X_minus:
                //���� �ʱ�ȭ
                if (moveTriger == false)
                {
                    StartCoroutine(move(nor));
                }
                nor = NORMAL.NONE;
                break;
            case NORMAL.X_plus:
                //���� �ʱ�ȭ
                if (moveTriger == false)
                {
                    StartCoroutine(move(nor));
                }
                nor = NORMAL.NONE;
                break;
            case NORMAL.Y_minus:
                //���� �ʱ�ȭ
                nor = NORMAL.NONE;
                break;
            case NORMAL.Y_plus:
                //���� �ʱ�ȭ
                nor = NORMAL.NONE;
                break;
            case NORMAL.Z_minus:
                //���� �ʱ�ȭ
                if (moveTriger == false)
                {
                    StartCoroutine(move(nor));
                }
                nor = NORMAL.NONE;
                break;
            case NORMAL.Z_plus:
                //���� �ʱ�ȭ
                if (moveTriger == false)
                {
                    StartCoroutine(move(nor));
                }
                nor = NORMAL.NONE;
                break;
            default:
                break;
        }
    }

    void stateCheck()
    {
        for (int i = 0; i < otherConnect.Length; i++)
        {
            float distance = (otherConnect[i].transform.position - transform.position).magnitude;

            if (distance <= 1.1f)
            {
                this.gameObject.GetComponent<MeshRenderer>().material = m_connect[1];
                state = true;
            }
        }
    }

    //Ÿ��
    public bool moveTriger = false;
    public bool testFlag = true;
    private float blockMoveTime = 1.0f;

    //�ڷ�ƾ
    private IEnumerator move(NORMAL n)
    {
        moveTriger = true;
        float elapsedTime = 0.0f;

        if (state == false)
        {
            if (n == NORMAL.X_minus)
            {
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = currentPosition + Vector3.right;

                while (elapsedTime < blockMoveTime)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / blockMoveTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;

                moveTriger = false;
            }
            else if (n == NORMAL.X_plus)
            {
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = currentPosition + Vector3.left;

                while (elapsedTime < blockMoveTime)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / blockMoveTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;

                moveTriger = false;
            }
            else if (n == NORMAL.Y_minus)
            {
                yield return null;
            }
            else if (n == NORMAL.Y_plus)
            {
                yield return null;
            }
            else if (n == NORMAL.Z_minus)
            {
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = currentPosition + Vector3.forward;

                while (elapsedTime < blockMoveTime)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / blockMoveTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;

                moveTriger = false;
            }
            else if (n == NORMAL.Z_plus)
            {
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = currentPosition + Vector3.back;

                while (elapsedTime < blockMoveTime)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / blockMoveTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;

                moveTriger = false;
            }
        }
    }
    public void GetP(Vector3 hitPiont, Vector3 hitNormal)
    {
        HitPoint = hitPiont;
        HitNormal = hitNormal;
    }
}
