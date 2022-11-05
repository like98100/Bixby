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
    GameObject tutorialImage;
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
        tutorialImage = GameObject.Find("TutorialImage");
    }

    // Update is called once per frame
    void Update()
    {
        // ����Ʈ ���� ���� Ȯ��
        switch (currentQuest.questObject)
        {
            case QuestKind.management:
                //Debug.Log("Developer's Kind");
                //UI_Control.Inst.Mission.misssionSet("Ʃ�丮��", "�ӽ��ؽ�Ʈ. ����Ʈ ���� �ε�.");
                //SetNextQuest();
                if (Input.anyKeyDown)
                {
                    tutorialImage.SetActive(false);
                    UI_Control.Inst.Mission.misssionSet("Ʃ�丮��", "villagerA���� ���� �Žÿ�");
                }
                break;
            default:
                CheckQuestCount();
                break;
        }
        // ���� quest text print
        if (Input.GetKeyDown(KeyCode.Tab))//����Ʈ �׽�Ʈ�� �ӽ� �ڵ�
        {
            itemData tempItem = new itemData();
            tempItem.itemID = -2;
            tempItem.xSize = tempItem.ySize = 1f;
            System.Array.Resize(ref tempItem.tag, 2);
            tempItem.itemName = "����";
            tempItem.tag[0] = "food";
            tempItem.tag[1] = "cooked";
            Vector2 itemPos = inventoryObject.Inst.emptyCell(1f, 1f);
            inventoryObject.Inst.itemGet(1f, 1f, itemPos.x, itemPos.y, tempItem);
            SetObjectIndex(tempItem.itemID);
            GameObject.Find(currentQuest.npcName).GetComponent<NPC>().SetIndex(GameObject.Find(currentQuest.npcName).GetComponent<NPC>().GetIndex() + 1);
        }
        
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
        currentQuest = JsonData.questList[JsonData.questIndex]; // Update Current Quest
        isClear = false;
        SetObjectIndex(-1);
        MissionSet();
    }

    public bool GetIsClear()
    {
        return isClear;
    }

    public void SetIsClear(bool idx)
    {
        isClear = idx;
        
        if(idx)
        {
            if (currentQuest.npcName == "none")//NPC���� �����ʰ� ����Ʈ�� Ŭ����Ǵ� ���
                SetNextQuest();
            else
                UI_Control.Inst.Mission.misssionSet(UI_Control.Inst.Mission.GetMissionTitle(), currentQuest.npcName + "���� ���ÿ�");
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
        switch (currentQuest.questObject)//�丮�� ��ȣ�ۿ��� ��� ���⿡�� � �丮 Ȥ�� � ��ȣ�ۿ����� ����
        {
            case QuestKind.kill:
                questPurpose = currentQuest.objectVar.ToString();
                missionText = "����" + questPurpose + "���� óġ�Ͻÿ�";
                break;
            case QuestKind.hunt:
                questPurpose = currentQuest.objectVar.ToString();
                missionText = "����" + questPurpose + "���� ����Ͻÿ�";
                break;
            case QuestKind.cook:
                if (currentQuest.objectVar == -2)
                    questPurpose = "����";
                missionText = questPurpose + "��(��) ����ÿ�";
                break;
            case QuestKind.interactive:
                break;
            default:
                break;
        }
        switch (JsonData.questIndex)
        {
            case 1:
                missionTitle = "Quest 1";
                break;
            case 2:
                missionTitle = "Quest 2";
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
}
