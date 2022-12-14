using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    public static QuestObject manager;

    // Awake : 변수 Value 적용
    // Start : 데이터 Load / Create
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);  // 이 오브젝트는 신이 변경되도 제거되지 않는다

        if (manager == null)    // 할당되어 있지 않다면
        {
            manager = this;
        }
        else if(manager != this)        // 현재 할당되어 있는 오브젝트가아니라면
        {
            Destroy(gameObject);        // NAGA
        }

        isClear = false;
        objectIndex = 0;
        questSubIndex = 0;
    }

    questJsonData JsonData;         // Quest Json Data
    bool isClear;                   // Quest isClear bool var
    questData currentQuest;
    int questSubIndex;       // Quest sub Index var;
    int objectIndex;                // Quest Object Index var;
    //GameObject tutorialImage;

    // Start is called before the first frame update
    //public List<Material> NPC_Plane_Marks;//NPC 퀘스트 마크 추가
    List<itemData> items;
    void Start()
    {
        if (json.FileExist(Application.dataPath, "quests"))                                      // Quest 파일이 존재할 시
        {
            JsonData = json.LoadJsonFile<questJsonData>(Application.dataPath, "quests");   // Quest Load
            //Debug.Log("Load Complete : " + JsonData.questList[0].npcName);
        }
        else                                                                                    // Quest 파일이 존재하지 않을 때
        {
            //JsonData = new questJsonData();                // new Class
            //JsonData.questList[0].Log();
            //questJsonData.questList = new List<questData>();    // new List
            //questJsonData.questIndex = -1;                      // Quest index -1로 설정

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
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title") MissionSet();
        //tutorialImage = GameObject.Find("TutorialImage");

        //아이템 데이터
        items = new List<itemData>();
        itemJsonData tempJson = json.LoadJsonFile<itemJsonData>(Application.dataPath, "Harvest");
        foreach (var item in tempJson.itemList)
        {
            items.Add(item);
        }
        tempJson = json.LoadJsonFile<itemJsonData>(Application.dataPath, "cook");//json로드
        foreach (var item in tempJson.itemList)
        {
            items.Add(item);
        }
    }
    int questIndex; //인덱스 확인용 변수

    // Update is called once per frame
    void Update()
    {
        questIndex = JsonData.questIndex;//인덱스 확인용 임시 변수
        //if (JsonData.questIndex == 0 && Input.anyKeyDown)
        //{
        //    tutorialImage.SetActive(false);
        //    UI_Control.Inst.Mission.misssionSet("튜토리얼", "villagerA에게 말을 거시오");
        //}
        // 퀘스트 진행 여부 확인
        switch (currentQuest.questObject[questSubIndex])
        {
            case QuestKind.management:
                //Debug.Log("Developer's Kind");
                //UI_Control.Inst.Mission.misssionSet("튜토리얼", "임시텍스트. 퀘스트 무사 로드.");
                //SetNextQuest();
                break;
            default:
                CheckQuestCount();
                break;
        }
        // 이후 quest text print
    }

    public questJsonData GetQuestData()
    {
        if (JsonData == null) return null;      // Except
        return JsonData;
    }

    public int GetObjectId()
    {
        return currentQuest.objectId[questSubIndex];
    }

    public Vector3 GetPosition()
    {
        //Debug.Log("현재 위치 : " + currentQuest.position[questSubIndex].x);
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
        switch (currentQuest.questObject[questSubIndex])//퀘스트 종류에 따라 인덱스 조정 다르게 처리
        {
            case QuestKind.interactive:
                break;
            case QuestKind.kill:
            case QuestKind.cook:
            case QuestKind.hunt:
                if (objectIndex >= currentQuest.objectCnt[questSubIndex])
                {
                    SetIsClear(true);
                }
                break;
            case QuestKind.management:
            case QuestKind.spot:
                if (objectIndex == currentQuest.objectCnt[questSubIndex])
                {
                    SetIsClear(true);
                }
                break;
            default:
                break;
        }
        //if (objectIndex == currentQuest.objectCnt[questSubIndex])
        //{
        //    SetIsClear(true);
        //}
    }

    //void SetCookCount()                                         // 요리 여부 확인 함수
    //{
    //    if (objectIndex == currentQuest.objectVar)
    //        SetIsClear(true);
    //}

    //void SetHuntCount()                                         // 수렵 카운트 확인 함수
    //{
    //    if (objectIndex == currentQuest.objectVar)
    //        SetIsClear(true);
    //}

    //void SetInteractiveCount()                                  // 상호작용 여부 확인 함수
    //{
    //    if (objectIndex == currentQuest.objectVar)
    //        SetIsClear(true);
    //}

    public void SetNextQuest()                                         // 다음 퀘스트 이동 함수
    {
        JsonData.questIndex += 1;                               // Quest Index++
        isClear = false;
        questSubIndex = 0;
        SetObjectIndex(0);
        if (JsonData.questList.Count <= JsonData.questIndex)
        {
            //GameObject.Find(currentQuest.npcName).GetComponent<NPC>().SetIndex(100);
            UI_Control.Inst.Mission.misssionSet("", "");
            return;
        }
        currentQuest = JsonData.questList[JsonData.questIndex]; // Update Current Quest
        MissionSet();
        //GameObject.Find(currentQuest.npcName).GetComponent<NPC>().SetIndex(GameObject.Find(currentQuest.npcName).GetComponent<NPC>().GetIndex() + 1);
        //NPC 대화 인덱스 조정함수, 조정 위치를 NPC 스크립트 내로 이동
        GameObject.Find("GameManager").GetComponent<SetPositionParticle>().InitializeVariable();    // 파티클 위치 변경
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
            if(questSubIndex != currentQuest.objectId.Count - 1)     // 서브 퀘스트가 마지막이 아닐 때
            {
                if (JsonData.questIndex == 1)
                {
                    switch (questSubIndex)
                    {
                        case 2:
                            GameObject.Find("Tutorial").GetComponent<UI_Tutorial>().TutoImageSet(0);//전투 튜토리얼
                            break;
                        case 3:
                            GameObject.Find("Tutorial").GetComponent<UI_Tutorial>().TutoImageSet(4);//낚시 튜토리얼
                            break;
                        case 4:
                            GameObject.Find("Tutorial").GetComponent<UI_Tutorial>().TutoImageSet(5);//요리 튜토리얼
                            break;
                        default:
                            break;
                    }
                }
                questSubIndex++;
                isClear = false;
                SetObjectIndex(0);                  // 변수 초기화
                if (currentQuest.questObject[questSubIndex] == QuestKind.kill)//중간보스 나오기 전 대화창 위치 수정. 기존의 MissionSet에서 실행할 경우, 2번 나오는 경우가 발생
                {
                    switch (currentQuest.objectId[questSubIndex])
                    {
                        case 3002:
                        case 3003:
                        case 3004:
                            UI_Control.Inst.Speech.setUp("알파", "(이 앞에서 강한 원소의 힘이 느껴진다. 앞으로 가 보자.)");
                            break;
                        default:
                            break;
                    }
                }
                GameObject.Find("GameManager").GetComponent<SetPositionParticle>().InitializeVariable();    // 파티클 위치 변경
            }
            else
            {
                if (currentQuest.npcName == "none")//NPC에게 가지않고 퀘스트가 클리어되는 경우
                    SetNextQuest();
                else
                {
                    string clearNpc = "";
                    switch (currentQuest.npcName)//이름표 조정
                    {
                        case "partnerA":
                            clearNpc = "베타";
                            break;
                        case "partnerB":
                            clearNpc = "델타";
                            break;
                        case "partnerC":
                            clearNpc = "감마";
                            break;
                        default:
                            clearNpc = "지도자";
                            break;
                    }
                    UI_Control.Inst.Mission.misssionSet(JsonData.questIndex % 2 == 0 ? "" : "Quest " + ((JsonData.questIndex + 1) / 2).ToString(), clearNpc + "에게 가시오");
                }
            }
        }
    }
    public string GetNPCName()
    {
        return currentQuest.npcName;
    }

    public void MissionSet()
    {
        if (JsonData == null)
            return;
        if (JsonData.questIndex == 0)
        {
            UI_Control.Inst.Mission.misssionSet("튜토리얼", "지도자에게 말을 거시오");
            return;
        }
        if (isClear)
        {
            SetIsClear(isClear);
            return;
        }
        string missionTitle = "";
        string missionText = "";
        string questPurpose = "";
        questPurpose = " " + currentQuest.objectCnt[questSubIndex].ToString();//공통적 사용부분(목표)
        string questProgress = "(" + objectIndex.ToString() + "/" + questPurpose + ")";//공통적 사용부분(퀘스트 진행상황)
        switch (currentQuest.questObject[questSubIndex])//요리와 상호작용의 경우 여기에서 어떤 요리 혹은 어떤 상호작용인지 설정
        {
            case QuestKind.kill:
                missionText = "적을" + questPurpose + "마리 처치하기" + questProgress;
                switch (currentQuest.objectId[questSubIndex])
                {
                    case 3002:
                        missionText = "얼음 보스를 처치하시오";
                        break;
                    case 3003:
                        missionText = "물 보스를 처치하시오";
                        break;
                    case 3004:
                        missionText = "전기 보스를 처치하시오";
                        break;
                    case 3005:
                        missionText = "최종 보스를 처치하시오";
                        break;
                    default:
                        break;
                }
                break;
            case QuestKind.hunt:
                missionText = "사슴을" + questPurpose + "마리 수렵하시오" + questProgress;
                break;
            case QuestKind.cook:
                missionText = "데이터에 없는 아이템을";
                foreach (var item in items)
                {
                    if (item.itemID == currentQuest.objectId[questSubIndex])
                    {
                        missionText = item.itemName + "을(를)";
                        break;
                    }
                }
                missionText += questPurpose + "개 얻거나 만들기" + questProgress;
                break;
            case QuestKind.interactive:
                break;
            case QuestKind.spot://특정 위치 이동 부분 추가
                missionText = "표시된 위치로 이동하시오.";//위치 지정 필요
                break;
            default:
                break;
        }
        missionTitle = JsonData.questIndex % 2 == 0 ? "" : "Quest " + ((JsonData.questIndex + 1) / 2).ToString();
        UI_Control.Inst.Mission.misssionSet(missionTitle, missionText);
    }
    public int GetIndex()
    {
        if (JsonData == null) return 0;
        else return JsonData.questIndex;
    }
    public void QuestInitialize()
    {
        JsonData.questIndex = 0;
        currentQuest = JsonData.questList[JsonData.questIndex];
        objectIndex = 0;
        questSubIndex = 0;
        SetIsClear(false);
        inventoryObject.Inst.Initialize();
    }
    public QuestKind GetQuestKind()
    {
        return currentQuest.questObject[questSubIndex];
    }

    public void DungeonQuestInit()
    {
        if (currentQuest.objectId.Count != 1)
            questSubIndex = 1;
        SetObjectIndex(0);
    }

    public void DungeonRunaway()
    {
        if (!isClear)
            questSubIndex = 0;
        SetObjectIndex(0);
        LoadingSceneController.Instance.LoadScene("FieldScene");
    }
}
