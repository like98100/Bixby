using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    // Awake : 변수 Value 적용
    // Start : 데이터 Load / Create
    private void Awake()
    {
        isClear = false;
    }

    questJsonData JsonData;    // Quest Json Data
    bool isClear;                   // Quest isClear bool var

    // Start is called before the first frame update
    void Start()
    {
        if (json.FileExist(Application.dataPath, "quests"))                                      // Quest 파일이 존재할 시
        {
            JsonData = json.LoadJsonFile<questJsonData>(Application.dataPath, "quests");   // Quest Load

            Debug.Log("Load Complete : " + JsonData.questList[0].npcName);
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
    }

    // Update is called once per frame
    void Update()
    {
        // 퀘스트 진행 여부 확인
        // enum questObject로 확인할 것
        // 이후 quest text print
    }

    void SetKillCount()                                         // 처치 카운트 계산 함수
    {

    }

    void SetCookCount()                                         // 요리 여부 확인 함수
    {

    }

    void SetHuntCount()                                         // 수렵 카운트 계산 함수
    {

    }

    void SetInteractiveCount()                                  // 상호작용 여부 확인 함수
    {

    }

    void SetNextQuest()                                         // 다음 퀘스트 이동 함수
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
