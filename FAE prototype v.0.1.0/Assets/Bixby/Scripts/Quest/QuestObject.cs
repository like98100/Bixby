using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    // Awake : ���� Value ����
    // Start : ������ Load / Create
    private void Awake()
    {
        isClear = false;
    }

    questJsonData JsonData;    // Quest Json Data
    bool isClear;                   // Quest isClear bool var

    // Start is called before the first frame update
    void Start()
    {
        if (json.FileExist(Application.dataPath, "quests"))                                      // Quest ������ ������ ��
        {
            JsonData = json.LoadJsonFile<questJsonData>(Application.dataPath, "quests");   // Quest Load

            Debug.Log("Load Complete : " + JsonData.questList[0].npcName);
        }
        else                                                                                    // Quest ������ �������� ���� ��
        {
            //JsonData = new questJsonData();                // new Class
            //JsonData.questList[0].Log();
            //questJsonData.questList = new List<questData>();    // new List
            //questJsonData.questIndex = -1;                      // Quest index -1�� ����

            questJsonData temp = new questJsonData();

            //foreach(var quest in JsonData.questList)
            //{
            //    temp.questList.Add(quest);
            //}


            string questStr = json.ObjectToJson(temp);        // ToString

            Debug.Log(questStr);

            json.CreateJsonFile(Application.dataPath, "quests", questStr);  // Create Json FIle
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ����Ʈ ���� ���� Ȯ��
        // enum questObject�� Ȯ���� ��
        // ���� quest text print
    }

    void SetKillCount()                                         // óġ ī��Ʈ ��� �Լ�
    {

    }

    void SetCookCount()                                         // �丮 ���� Ȯ�� �Լ�
    {

    }

    void SetHuntCount()                                         // ���� ī��Ʈ ��� �Լ�
    {

    }

    void SetInteractiveCount()                                  // ��ȣ�ۿ� ���� Ȯ�� �Լ�
    {

    }

    void SetNextQuest()                                         // ���� ����Ʈ �̵� �Լ�
    {

    }

    public bool GetIsClear()
    {
        return isClear;
    }

    public void SetIsClear(bool idx)
    {
        isClear = idx;
    }
}
