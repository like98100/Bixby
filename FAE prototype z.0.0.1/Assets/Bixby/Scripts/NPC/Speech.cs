using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Speech : MonoBehaviour
{
    private void Awake()
    {
        speechWindow = GameObject.Find("Speech");
        talker = speechWindow.transform.GetChild(1).GetComponent<Text>();
        speech = speechWindow.transform.GetChild(0).GetComponent<Text>();
    }
    Text talker;//��ȭ���
    Text speech;//��ȭ����
    List<string> speechList;//��ȭ���� ����Ʈ
    int speechIndex;//����Ʈ�� ���� ��ȭ������ ����
    GameObject speechWindow;//��ȭâ
    speechJsonData speechJsonData;
    void Start()
    {
        speechWindow.SetActive(false);
    }
    public void setUp(string name, int index)//�� �ɾ��� ��
    {
        speechList = new List<string>();//��ȭ ����Ʈ �ʱ�ȭ
        this.talker.text = name;//����ȭ
        if (json.FileExist(Application.dataPath, name + index.ToString()))//�ش� �̸��� json���� ���� Ȯ��
            speechJsonData = json.LoadJsonFile<speechJsonData>(Application.dataPath, name + index.ToString());//�ε��ؿ�
        foreach (var item in speechJsonData.speechDatas)//�ε��� json�������� speechDatas�� ������
        {
            speechList.Add(item);//��ȭ ����Ʈ�� ����
        }
        speechIndex = 0;//��ȭ �ε����� ������
        UI_Control.Inst.windowSet(speechWindow);
    }
    void Update()
    {
        if (speechWindow.activeSelf)//��ȭâ�� �����ִ� ������
            speech.text = speechList[speechIndex];//��ȭ������ ����Ʈ �� �ش� �ε����� ����
    }

    public void speechNext()//��ȭ �ѱ�� ��ư
    {
        if (speechIndex + 1 == speechList.Count)//�������϶�
        {
            speechIndex = 0;//�ʱ�ȭ
            UI_Control.Inst.windowSet(speechWindow);
        }
        else
            speechIndex++;//�ƴϸ� �ѱ��
    }
}
