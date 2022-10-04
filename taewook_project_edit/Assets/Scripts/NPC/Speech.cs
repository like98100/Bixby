using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Speech : MonoBehaviour
{
    [SerializeField] Text talker;//��ȭ���
    [SerializeField] Text speech;//��ȭ����
    List<string> speechList;//��ȭ���� ����Ʈ
    int speechIndex;//����Ʈ�� ���� ��ȭ������ ����
    public GameObject SpeechWindow;//��ȭâ
    SpeechJsonData speechJsonData;
    [SerializeField] string talkerName;//��ȭ�� �̸�
    void Start()
    {
        SpeechWindow.SetActive(false);
    }
    public void SetUp(string name, int index)//�� �ɾ��� ��
    {
        speechList = new List<string>();//��ȭ ����Ʈ �ʱ�ȭ
        this.talkerName = name;//��ȭ��� �̸�
        this.talker.text = this.talkerName;//����ȭ
        if (Json.FileExist(Application.dataPath, talkerName + index.ToString()))//�ش� �̸��� json���� ���� Ȯ��
            speechJsonData = Json.LoadJsonFile<SpeechJsonData>(Application.dataPath, this.talkerName + index.ToString());//�ε��ؿ�
        foreach (var item in speechJsonData.SpeechDatas)//�ε��� json�������� speechDatas�� ������
        {
            speechList.Add(item);//��ȭ ����Ʈ�� ����
        }
        speechIndex = 0;//��ȭ �ε����� ������
        UI_Control.Inst.WindowSet(SpeechWindow);
    }
    void Update()
    {
        if (SpeechWindow.activeSelf)//��ȭâ�� �����ִ� ������
            speech.text = speechList[speechIndex];//��ȭ������ ����Ʈ �� �ش� �ε����� ����
    }

    public void SpeechNext()//��ȭ �ѱ�� ��ư
    {
        if (speechIndex + 1 == speechList.Count)//�������϶�
        {
            speechIndex = 0;//�ʱ�ȭ
            UI_Control.Inst.WindowSet(SpeechWindow);
        }
        else
            speechIndex++;//�ƴϸ� �ѱ��
    }
}
