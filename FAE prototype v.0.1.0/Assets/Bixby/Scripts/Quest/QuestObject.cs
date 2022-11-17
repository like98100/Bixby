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
        objectIndex = 0;
        questSubIndex = 0;
    }

    questJsonData JsonData;         // Quest Json Data
    bool isClear;                   // Quest isClear bool var
    questData currentQuest;
    int questSubIndex;              // Quest sub Index var;
    int objectIndex;                // Quest Object Index var;
    //GameObject tutorialImage;
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
        MissionSet();
        if (JsonData.questIndex == 0)
            UI_Control.Inst.Mission.misssionSet("Ʃ�丮��", "villagerA���� ���� �Žÿ�");
        //tutorialImage = GameObject.Find("TutorialImage");
    }

    // Update is called once per frame
    void Update()
    {
        //if (JsonData.questIndex == 0 && Input.anyKeyDown)
        //{
        //    tutorialImage.SetActive(false);
        //    UI_Control.Inst.Mission.misssionSet("Ʃ�丮��", "villagerA���� ���� �Žÿ�");
        //}
        // ����Ʈ ���� ���� Ȯ��
        switch (currentQuest.questObject[questSubIndex])
        {
            case QuestKind.management:
                //Debug.Log("Developer's Kind");
                //UI_Control.Inst.Mission.misssionSet("Ʃ�丮��", "�ӽ��ؽ�Ʈ. ����Ʈ ���� �ε�.");
                //SetNextQuest();
                break;
            default:
                CheckQuestCount();
                break;
        }
        // ���� quest text print
        if (Input.GetKeyDown(KeyCode.Tab))//����Ʈ �׽�Ʈ�� �ӽ� �ڵ�
        {
            itemData tempItem = new itemData();
            tempItem.itemID = currentQuest.objectId[questSubIndex];
            tempItem.xSize = tempItem.ySize = 1f;
            System.Array.Resize(ref tempItem.tag, 2);
            tempItem.itemName = "����Ʈ ����";
            tempItem.tag[0] = "food";
            tempItem.tag[1] = "cooked";
            Vector2 itemPos = inventoryObject.Inst.emptyCell(1f, 1f);
            inventoryObject.Inst.itemGet(1f, 1f, itemPos.x, itemPos.y, tempItem);
        }

    }

    public int GetObjectId()
    {
        return currentQuest.objectId[questSubIndex];
    }

    public Vector3 GetPosition()
    {
        Debug.Log("���� ��ġ : " + currentQuest.position[questSubIndex].x);
        return currentQuest.position[questSubIndex];
    }


    public int GetObjectIndex()                                 // Get ObjectIndex Var Func
    {
        return objectIndex;
    }


    public void SetObjectIndex(int idx)                         // Set ObjectIndex Var Func
    {
        objectIndex = idx;
        MissionSet();
    }

    void CheckQuestCount()                                      // Quest Index Check Func
    {
        if (objectIndex == currentQuest.objectCnt[questSubIndex])
        {
            SetIsClear(true);
        }
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
        isClear = false;
        questSubIndex = 0;
        SetObjectIndex(0);
        if (JsonData.questList.Count <= JsonData.questIndex)
        {
            GameObject.Find(currentQuest.npcName).GetComponent<NPC>().SetIndex(100);
            UI_Control.Inst.Mission.misssionSet("", "");
            return;
        }
        currentQuest = JsonData.questList[JsonData.questIndex]; // Update Current Quest
        MissionSet();
        GameObject.Find(currentQuest.npcName).GetComponent<NPC>().SetIndex(GameObject.Find(currentQuest.npcName).GetComponent<NPC>().GetIndex() + 1);
    }

    public bool GetIsClear()
    {
        return isClear;
    }

    public void SetIsClear(bool idx)
    {
        isClear = idx;

        if (idx)
        {
            if(questSubIndex != currentQuest.objectId.Count - 1)     // ���� ����Ʈ�� �������� �ƴ� ��
            {
                questSubIndex++;
                isClear = false;
                SetObjectIndex(0);                  // ���� �ʱ�ȭ
            }
            else
            {
                if (currentQuest.npcName == "none")//NPC���� �����ʰ� ����Ʈ�� Ŭ����Ǵ� ���
                    SetNextQuest();
                else
                    UI_Control.Inst.Mission.misssionSet(UI_Control.Inst.Mission.GetMissionTitle(), currentQuest.npcName + "���� ���ÿ�");
            }
        }
    }
    public string GetNPCName()
    {
        return currentQuest.npcName;
    }

    public void MissionSet()
    {
        string missionTitle = "";
        string missionText = "";
        string questPurpose = "";
        switch (currentQuest.questObject[questSubIndex])//�丮�� ��ȣ�ۿ��� ��� ���⿡�� � �丮 Ȥ�� � ��ȣ�ۿ����� ����
        {
            case QuestKind.kill:
                questPurpose = currentQuest.objectCnt.ToString();
                missionText = "����" + questPurpose + "���� óġ�ϱ�" + "(" + objectIndex.ToString() + "/" + currentQuest.objectCnt.ToString() + ")";
                break;
            case QuestKind.hunt:
                questPurpose = currentQuest.objectCnt.ToString();
                missionText = "����" + questPurpose + "���� ����ϱ�" + "(" + objectIndex.ToString() + "/" + currentQuest.objectCnt.ToString() + ")";
                break;
            case QuestKind.cook:
                if (currentQuest.objectId[questSubIndex] == 2001)
                    questPurpose = "�����ֽ�";
                missionText = questPurpose + "��(��) �����";
                break;
            case QuestKind.interactive:
                break;
            default:
                break;
        }
        switch (JsonData.questIndex)
        {
            case 1:
                missionTitle = "Quest " + JsonData.questIndex.ToString();
                break;
            case 2:
            case 4:
                missionTitle = "";
                break;
            case 3:
                missionTitle = "Quest " + (JsonData.questIndex - 1).ToString();
                break;
            default:
                break;
        }

        UI_Control.Inst.Mission.misssionSet(missionTitle, missionText);
    }
    public int GetIndex()
    {
        return JsonData.questIndex;
    }
    public QuestKind GetQuestKind()
    {
        return currentQuest.questObject[questSubIndex];
    }
}
