using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    // 0. NULL, 1. ���� �� ���̱�, 2. Ƚ�� ���̱� 1.2 ��ĥ�� ������ ����. 3. ��������, 4. ������Ʈ ���� ����
    public int puzzleType; //������ ���� ����
    public bool puzzleState; //������ �����ߴ��� �ƴ��� �Ǵ� 

    public GameObject[] etrigger; //���� �� ���ϰ�
    public GameObject[] pattern;  //���� ������Ʈ
    public GameObject[] connect;  //���� ������Ʈ

    public Transform patternRotation; //���� ���� ���� ������Ʈ �����̼�

    public GameObject treasureBox; //��������

    private void Awake()
    {
        puzzleState = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (puzzleType)
        {
            case 0:
                break;
            case 1:
                //���� �� ���̱� ����
                puzzleState = EelmentTriggerPuzzle();
                PuzzleClear(puzzleType);
                break;
            case 2:

                break;
            case 3:
                //��������
                puzzleState = PatternPuzzle();
                PuzzleClear(puzzleType);
                break;
            case 4:
                //������Ʈ ���� ����
                puzzleState = ConnectPuzzle();
                PuzzleClear(puzzleType);
                break;
            default:
                break;
        }
    }

    //������ Ŭ���� ���� ��
    void PuzzleClear(int type)
    {
        if (puzzleState == true)
        {
            //���� �� ���̱� �϶�
            if (type == 1)
            {
                if (treasureBox != null)
                {
                    //�������� Ȱ��ȭ
                    treasureBox.GetComponent<TreasureBox>().boxState = true;
                }
            }
            //���� �����϶�
            else if (type == 3)
            {
                if (treasureBox != null)
                {
                    //�������� Ȱ��ȭ
                    treasureBox.GetComponent<TreasureBox>().boxState = true;
                }
            }
            else if (type == 4)
            {
                if (treasureBox != null)
                {
                    //�������� Ȱ��ȭ
                    treasureBox.GetComponent<TreasureBox>().boxState = true;
                }
            }
        }
        else if (puzzleState == false)
        {
            //���� �� ���̱� �϶�
            if (type == 1)
            {
                if (treasureBox != null)
                {
                    //�������� Ȱ��ȭ
                    treasureBox.GetComponent<TreasureBox>().boxState = false;
                }
            }
            //���� �����϶�
            else if (type == 3)
            {
                if (treasureBox != null)
                {
                    //�������� Ȱ��ȭ
                    treasureBox.GetComponent<TreasureBox>().boxState = false;
                }
            }
            else if (type == 4)
            {
                if (treasureBox != null)
                {
                    //�������� Ȱ��ȭ
                    treasureBox.GetComponent<TreasureBox>().boxState = false;
                }
            }
        }
    }

    //1�� ����
    bool EelmentTriggerPuzzle()
    {
        for (int i = 0; i < etrigger.Length; i++)
        {
            if (etrigger[i].GetComponent<Etrigger>().state == true)
            {
                continue;
            }
            else
                return false;
        }

        return true;
    }

    //3�� ����
    bool PatternPuzzle()
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i].GetComponent<Pattern>().state == true)
            {
                continue;
            }
            else
                return false;
        }

        return true;
    }


    //4�� ����
    bool ConnectPuzzle()
    {
        for (int i = 0; i < connect.Length; i++)
        {
            if (connect[i].GetComponent<Connect>().state == true)
            {
                continue;
            }
            else
                return false;
        }

        return true;
    }
}
