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
    bool isExist;
    void Start()
    {
        speechWindow.SetActive(false);
        Button nextSpeech = speechWindow.transform.GetChild(2).GetComponent<Button>();
        nextSpeech.onClick.AddListener(() => speechNext());
        //quest = GameObject.Find("GameManager").GetComponent<QuestObject>();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FieldScene")
            Tutorial = GameObject.Find("Tutorial").GetComponent<UI_Tutorial>();
        isExist = false;
    }
    public void setUp(string name, string content)//�� �ɾ��� ��      // name ��ġ �� isClear == true�� �� text ����
    {
        speechList = new List<string>();//��ȭ ����Ʈ �ʱ�ȭ
        this.talker.text = NameChanger(name);
        isExist = GameObject.Find(this.talker.text) != null;
        if (isExist)//��ȭ��밡 ������ ��� : �ش� ���� ���� ��ȣ�ۿ��Ͽ� ��ȭ�� ���
        {//�ش� ��뿡 �´� json���Ϸ� ��ȭ ����Ʈ�� ����
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
            if (QuestObject.manager.GetNPCName() == GameObject.Find(this.talker.text).GetComponent<NPC>().NpcGetName()
            && (QuestObject.manager.GetIndex() % 2 == 0 || QuestObject.manager.GetQuestKind() == QuestKind.management))
                QuestObject.manager.SetIsClear(true);//����Ʈ ���� �� ��ȭ ����Ʈ �� Ŭ���� ó��
        }
        else
        {//��ȭ��밡 �������� ���� ���, 2��° �Ű��������� ��ȭ ����Ʈ�� ���´�
            speechList.Add(content);
        }
        speechIndex = 0;//��ȭ �ε����� ������
        UI_Control.Inst.windowSet(speechWindow);
        //if (quest.GetNPCName() == this.talker.text
        //    && (quest.GetIndex() % 2 == 0)
        //    )
        //    quest.SetIsClear(true);
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
            if (isExist)//��ȭ��밡 �� ���� ������ ���
            {
                if (QuestObject.manager.GetIsClear()//����Ʈ�� Ŭ���� �Ǿ���
                && GameObject.Find(this.talker.text).GetComponent<NPC>().NpcGetName() == QuestObject.manager.GetNPCName())
                {//��ȭ��밡 Ŭ���� NPC�� ���
                    switch (QuestObject.manager.GetQuestKind())
                    {
                        case QuestKind.kill:
                            break;
                        case QuestKind.cook://�ش� ����Ʈ �� ������ ����Ʈ�� ������ ä�� ����Ʈ�� ���(�����ֽ� ��������)
                            foreach (var item in inventoryObject.Inst.itemObjects)
                            {
                                if (item.GetComponent<itemObject>().ItemData.itemID == QuestObject.manager.GetObjectId())
                                {//�ش� �������� �ִٸ�
                                    inventoryObject.Inst.throwItem(item, false);
                                    break;//�ش� �������� �κ��丮���� �ϳ� ����
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
                    QuestObject.manager.SetNextQuest();//���� ����Ʈ�� �ѱ��
                }
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
            case "��Ÿ":
            case "partnerA":
                return "��Ÿ";
            case "partnerB":
            case "��Ÿ":
                return "��Ÿ";
            case "partnerC":
            case "����":
                return "����";
            case "����":
            case "shop":
                return name;
            default:
                return "������";
        }
    }
}
