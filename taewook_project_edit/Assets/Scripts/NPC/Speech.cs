using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Speech : MonoBehaviour
{
    [SerializeField] Text talker;//대화상대
    [SerializeField] Text speech;//대화내용
    List<string> speechList;//대화내용 리스트
    int speechIndex;//리스트중 현재 대화내용의 순서
    public GameObject SpeechWindow;//대화창
    SpeechJsonData speechJsonData;
    [SerializeField] string talkerName;//대화자 이름
    void Start()
    {
        SpeechWindow.SetActive(false);
    }
    public void SetUp(string name, int index)//말 걸었을 때
    {
        speechList = new List<string>();//대화 리스트 초기화
        this.talkerName = name;//대화상대 이름
        this.talker.text = this.talkerName;//가시화
        if (Json.FileExist(Application.dataPath, talkerName + index.ToString()))//해당 이름의 json파일 존재 확인
            speechJsonData = Json.LoadJsonFile<SpeechJsonData>(Application.dataPath, this.talkerName + index.ToString());//로드해옴
        foreach (var item in speechJsonData.SpeechDatas)//로드한 json데이터의 speechDatas의 내용을
        {
            speechList.Add(item);//대화 리스트에 넣음
        }
        speechIndex = 0;//대화 인덱스로 쓸것임
        UI_Control.Inst.WindowSet(SpeechWindow);
    }
    void Update()
    {
        if (SpeechWindow.activeSelf)//대화창이 열려있는 동안은
            speech.text = speechList[speechIndex];//대화내용은 리스트 중 해당 인덱스의 내용
    }

    public void SpeechNext()//대화 넘기기 버튼
    {
        if (speechIndex + 1 == speechList.Count)//마지막일때
        {
            speechIndex = 0;//초기화
            UI_Control.Inst.WindowSet(SpeechWindow);
        }
        else
            speechIndex++;//아니면 넘기기
    }
}
