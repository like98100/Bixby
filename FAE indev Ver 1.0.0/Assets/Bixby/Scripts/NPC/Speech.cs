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
    //QuestObject quest;
    public List<Material> NPC_Plane_Marks;//NPC ����Ʈ ��ũ �߰�\
    public UI_Tutorial Tutorial;
    void Start()
    {
        speechWindow.SetActive(false);
        Button nextSpeech = speechWindow.transform.GetChild(2).GetComponent<Button>();
        nextSpeech.onClick.AddListener(() => speechNext());
        //quest = GameObject.Find("GameManager").GetComponent<QuestObject>();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FieldScene")
            Tutorial = GameObject.Find("Tutorial").GetComponent<UI_Tutorial>();
    }
    public void setUp(string name, string content)//�� �ɾ��� ��      // name ��ġ �� isClear == true�� �� text ����
    {
        speechList = new List<string>();//��ȭ ����Ʈ �ʱ�ȭ
        this.talker.text = NameChanger(name);
        if (json.FileExist(Application.dataPath, content))//�ش� �̸��� json���� ���� Ȯ��
            speechJsonData = json.LoadJsonFile<speechJsonData>(Application.dataPath, content);//�ε��ؿ�
        else 
        {
            speechJsonData = json.LoadJsonFile<speechJsonData>(Application.dataPath, name + "Temp");//�� ����Ʈ ��ȭ ��ũ��Ʈ �ε�
        }
        foreach (var item in speechJsonData.speechDatas)//�ε��� json�������� speechDatas�� ������
        {
            speechList.Add(item);//��ȭ ����Ʈ�� ����
        }
        speechIndex = 0;//��ȭ �ε����� ������
        UI_Control.Inst.windowSet(speechWindow);
        //if (quest.GetNPCName() == this.talker.text
        //    && (quest.GetIndex() % 2 == 0)
        //    )
        //    quest.SetIsClear(true);

        if (QuestObject.manager.GetNPCName() == GameObject.Find(this.talker.text).GetComponent<NPC>().NpcGetName()
            && (QuestObject.manager.GetIndex() % 2 == 0 || QuestObject.manager.GetQuestKind() == QuestKind.management)
    )
            QuestObject.manager.SetIsClear(true);
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
            //if (quest.GetIsClear()
            //    && this.talker.text == quest.GetNPCName())
            if (QuestObject.manager.GetIsClear()
                && GameObject.Find(this.talker.text).GetComponent<NPC>().NpcGetName() == QuestObject.manager.GetNPCName())
            {
                //GameObject.Find(this.talker.text).GetComponent<NPC>().SetIndex(GameObject.Find(this.talker.text).GetComponent<NPC>().GetIndex() + 1);
                
                //switch (quest.GetQuestKind())
                switch (QuestObject.manager.GetQuestKind())
                {
                    case QuestKind.kill:
                        break;
                    case QuestKind.cook:
                        foreach (var item in inventoryObject.Inst.itemObjects)
                        {
                            //if (item.GetComponent<itemObject>().ItemData.itemID == quest.GetObjectId())
                            if (item.GetComponent<itemObject>().ItemData.itemID == QuestObject.manager.GetObjectId())
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
                //quest.SetNextQuest();
                QuestObject.manager.SetNextQuest();
            }
            speechIndex = 0;//�ʱ�ȭ
            UI_Control.Inst.windowSet(speechWindow);
            if (QuestObject.manager.GetQuestKind()== QuestKind.management)
            {
                QuestObject.manager.SetIsClear(true);
            }
            switch (QuestObject.manager.GetIndex())
            {
                case 3:
                    Tutorial.TutoImageSet(0);
                    break;
                case 10:
                    Tutorial.ElementGetText(1);
                    break;
                case 14:
                    Tutorial.ElementGetText(2);
                    break;
                case 18:
                    Tutorial.ElementGetText(3);
                    break;
                default:
                    break;
            }
        }
        else
            speechIndex++;//�ƴϸ� �ѱ��
    }
    public string NameChanger(string name)
    {
        switch (name)//�̸�ǥ ����
        {
            case "partnerA":
                return "��Ÿ";
            case "partnerB":
                return "��Ÿ";
            case "partnerC":
                return "����";
            case "shop":
                return name;
            default:
                return "������";
        }
    }
}
