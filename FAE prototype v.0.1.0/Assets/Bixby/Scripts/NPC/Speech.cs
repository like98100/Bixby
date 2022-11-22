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
    QuestObject quest;
    void Start()
    {
        speechWindow.SetActive(false);
        Button nextSpeech = speechWindow.transform.GetChild(2).GetComponent<Button>();
        nextSpeech.onClick.AddListener(() => speechNext());
        quest = GameObject.Find("GameManager").GetComponent<QuestObject>();
    }
    public void setUp(string name, string content)//�� �ɾ��� ��      // name ��ġ �� isClear == true�� �� text ����
    {
        speechList = new List<string>();//��ȭ ����Ʈ �ʱ�ȭ
        this.talker.text = name;//����ȭ
        if (json.FileExist(Application.dataPath, content))//�ش� �̸��� json���� ���� Ȯ��
            speechJsonData = json.LoadJsonFile<speechJsonData>(Application.dataPath, content);//�ε��ؿ�
        foreach (var item in speechJsonData.speechDatas)//�ε��� json�������� speechDatas�� ������
        {
            speechList.Add(item);//��ȭ ����Ʈ�� ����
        }
        speechIndex = 0;//��ȭ �ε����� ������
        UI_Control.Inst.windowSet(speechWindow);
        if (quest.GetNPCName() == this.talker.text
            && (quest.GetIndex()==0||quest.GetIndex()==2)
            )
        {
            quest.SetIsClear(true);
        }
    }
    void Update()
    {
        if (speechWindow.activeSelf)//��ȭâ�� �����ִ� ������
            speech.text = speechList[speechIndex];//��ȭ������ ����Ʈ �� �ش� �ε����� ����
    }

    public void speechNext()//��ȭ �ѱ�� ��ư             // name ��ġ �� isClear == true�� �� questindex ����
    {
        if (speechIndex + 1 == speechList.Count)//�������϶�
        {
            if (quest.GetIsClear()
                && this.talker.text == quest.GetNPCName())
            {
                //GameObject.Find(this.talker.text).GetComponent<NPC>().SetIndex(GameObject.Find(this.talker.text).GetComponent<NPC>().GetIndex() + 1);
                switch (quest.GetQuestKind())
                {
                    case QuestKind.kill:
                        break;
                    case QuestKind.cook:
                        foreach (var item in inventoryObject.Inst.itemObjects)
                        {
                            if (item.GetComponent<itemObject>().ItemData.itemID == quest.GetObjectId())
                            {
                                inventoryObject.Inst.throwItem(item, false);
                                break;
                            }
                        }
                        break;
                    case QuestKind.hunt:
                        break;
                    case QuestKind.interactive:
                        break;
                    case QuestKind.spot:
                        break;
                }
                quest.SetNextQuest();
            }
            speechIndex = 0;//�ʱ�ȭ
            UI_Control.Inst.windowSet(speechWindow);
        }
        else
            speechIndex++;//�ƴϸ� �ѱ��
    }
}
