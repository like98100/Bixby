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
    public GameObject speechWindow;//��ȭâ
    speechJsonData speechJsonData;
    [SerializeField] string Name;//��ȭ�� �̸�
    void Start()
    {
        speechWindow.SetActive(false);
    }
    public void setUp(string name, int index)//�� �ɾ��� ��
    {
        speechList = new List<string>();//��ȭ ����Ʈ �ʱ�ȭ
        this.Name = name;//��ȭ��� �̸�
        this.talker.text = this.Name;//����ȭ
        if (json.FileExist(Application.dataPath, Name + index.ToString()))//�ش� �̸��� json���� ���� Ȯ��
            speechJsonData = json.LoadJsonFile<speechJsonData>(Application.dataPath, Name + index.ToString());//�ε��ؿ�
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
