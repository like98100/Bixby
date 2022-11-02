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
        objectIndex = -1;
    }

    questJsonData JsonData;         // Quest Json Data
    bool isClear;                   // Quest isClear bool var
    questData currentQuest;
    int objectIndex;                // Quest Object Index var;
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

        currentQuest = JsonData.questList[JsonData.questIndex];
    }

    // Update is called once per frame
    void Update()
    {
        // ����Ʈ ���� ���� Ȯ��
        switch (currentQuest.questObject)
        {
            case QuestKind.management:
                Debug.Log("Developer's Kind");
                UI_Control.Inst.Mission.misssionSet("Ʃ�丮��", "�ӽ��ؽ�Ʈ. ����Ʈ ���� �ε�.");
                SetNextQuest();
                break;
            default:
                CheckQuestCount();
                break;
        }
        // ���� quest text print

        
    }

    public int GetObjectIndex()                                 // Get ObjectIndex Var Func
    {
        return objectIndex;
    }


    public void SetObjectIndex(int idx)                         // Set ObjectIndex Var Func
    {
        objectIndex = idx;
    }

    void CheckQuestCount()                                      // Quest Index Check Func
    {
        if (objectIndex == currentQuest.objectVar)
            SetIsClear(true);
    }

    //void SetCookCount()                                         // �丮 ���� Ȯ�� �Լ�
    //{
    //    if (objectIndex == currentQuest.objectVar)
    //        SetIsClear(true);
    //}

    //void SetHuntCount()                                         // ���� ī��Ʈ Ȯ�� �Լ�
    //{
    //    if (objectIndex == currentQuest.objectVar)
    //        SetIsClear(true);
    //}

    //void SetInteractiveCount()                                  // ��ȣ�ۿ� ���� Ȯ�� �Լ�
    //{
    //    if (objectIndex == currentQuest.objectVar)
    //        SetIsClear(true);
    //}

    public void SetNextQuest()                                         // ���� ����Ʈ �̵� �Լ�
    {
        JsonData.questIndex += 1;                               // Quest Index++
        currentQuest = JsonData.questList[JsonData.questIndex]; // Update Current Quest
        SetObjectIndex(-1);
    }

    public bool GetIsClear()
    {
        return isClear;
    }

    public void SetIsClear(bool idx)
    {
        isClear = idx;
        if (idx && currentQuest.npcName == "none")//NPC���� �����ʰ� ����Ʈ�� Ŭ����Ǵ� ���
            SetNextQuest();
    }
    public string GetNPCName()
    {
        return currentQuest.npcName;
    }
}
