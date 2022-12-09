using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
    GameObject canvas;
    List<GameObject> tutorialImages;
    public TMPro.TextMeshProUGUI ElemeneClearText;
    GameObject openedImage;
    int openedIndex;
    float time;
    bool didText;
    bool didTutorial;
    bool aValue;
    [SerializeField] Material[] fontMaterials;
    // Start is called before the first frame update
    void Start()
    {
        canvas = this.gameObject.transform.GetChild(0).gameObject;
        tutorialImages = new List<GameObject>();
        ElemeneClearText = null;
        openedImage = null;
        openedIndex = 0;
        time = 0;
        didText = false;
        didTutorial = false;
        aValue = false;
        foreach (Transform item in canvas.transform)
        {
            if (item.gameObject.GetComponent<TMPro.TextMeshProUGUI>())
            {
                ElemeneClearText = item.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                ElemeneClearText.gameObject.SetActive(false);
            }
            else
            {
                tutorialImages.Add(item.gameObject);
                foreach (Transform item_ in item)
                {
                    item_.gameObject.SetActive(false);
                }
            }
        }
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ElemeneClearText.gameObject.activeSelf)//�ؽ�Ʈ ���� �� Ʃ�丮�� �̹����� ������ ���
        {
            time += Time.deltaTime;
            Color temp = ElemeneClearText.color;
            temp.a = aValue ? time / 1f : (3f - time) / 3f;
            if (aValue && temp.a >= 1)
            {
                aValue = false;
                time = 0;
            }
            ElemeneClearText.color = temp;
            if (temp.a < 0.1f && !aValue)
            {
                ElemeneClearText.gameObject.SetActive(false);
                canvas.SetActive(false);
                time = 0;
                if (ElemeneClearText.fontSize == 100)
                {
                    if (ElemeneClearText.text.Contains("���"))
                        LoadingSceneController.Instance.ReloadScene();
                    else if (QuestObject.manager.GetIndex() == 5)//�� ���� �ع� ��
                        TutoImageSet(1);//���� ���� Ʃ�丮��
                    else if (QuestObject.manager.GetIndex() == 10)//���� ���� �ع� ��
                        TutoImageSet(2);//���� ��ȯ Ʃ�丮��
                }
            }
        }
        if (QuestObject.manager.GetIndex() == 5 && QuestObject.manager.GetIsClear()&&!didText)//�� ���� ����Ʈ Ŭ���� ��, �ؽ�Ʈ�� ���� �� ���ٸ�
        {
            ElementGetText(0);//�� ���� �ع� �ؽ�Ʈ
        }
    }

    public void TutoImageSet(int index)//0:����, 1:��, 2:�ƹ�Ÿ ����, 3:ä�����, 4:����, 5:�丮, 6:����, 7:�� �� ��������Ʈ
    {
        if (index == 0 && didTutorial)
            return;
        UI_Control.Inst.windowSet(canvas);
        openedIndex = index;
        ElemeneClearText.gameObject.SetActive(false);
        foreach (var item in tutorialImages)
        {
            foreach (Transform item_ in item.transform)
            {
                item_.gameObject.SetActive(false);
            }
            item.SetActive(false);
        }
        tutorialImages[openedIndex].SetActive(true);
        tutorialImages[openedIndex].transform.GetChild(0).gameObject.SetActive(true);
        openedImage = tutorialImages[openedIndex].transform.GetChild(0).gameObject;
        if (index == 0)
            didTutorial = true;
    }
    public void NextImage()
    {
        int imageIndex = openedImage.transform.GetSiblingIndex();
        if (tutorialImages[openedIndex].transform.childCount-1 == imageIndex)
        {
            tutorialImages[openedIndex].transform.GetChild(imageIndex).gameObject.SetActive(false);
            tutorialImages[openedIndex].SetActive(false);
            if (canvas.activeSelf)
                UI_Control.Inst.windowSet(canvas);
            if (openedIndex == 4)
                TutoImageSet(7);
        }
        else
        {
            tutorialImages[openedIndex].transform.GetChild(imageIndex).gameObject.SetActive(false);
            tutorialImages[openedIndex].transform.GetChild(imageIndex+1).gameObject.SetActive(true);
            openedImage = tutorialImages[openedIndex].transform.GetChild(imageIndex + 1).gameObject;
        }
    }

    public void ElementGetText(int index)
    {
        aValue = true;
        canvas.SetActive(true);
        ElemeneClearText.gameObject.SetActive(true);
        ElemeneClearText.fontMaterial = fontMaterials[0];
        switch (index)
        {
            case 0:
                ElemeneClearText.text = "<<��>>���� �� ����";
                ElemeneClearText.color = ElementControl.FireSkillStartColor;
                ElemeneClearText.fontSize = 100f;
                didText = true;
                break;
            case 1:
                ElemeneClearText.text = "<<����>>���� �� ����";
                ElemeneClearText.color = ElementControl.IceSkillStartColor;
                ElemeneClearText.fontSize = 100f;
                break;
            case 2:
                ElemeneClearText.text = "<<��>>���� �� ����";
                ElemeneClearText.color = ElementControl.WaterSkillStartColor;
                ElemeneClearText.fontSize = 100f;
                break;
            case 3:
                ElemeneClearText.text = "<<����>>���� �� ����";
                ElemeneClearText.color = ElementControl.ElectroSkillStartColor;
                ElemeneClearText.fontSize = 100f;
                break;
            case 4:
                ElemeneClearText.text = "���̻� ���ư��°� ������ �� ����. ���ư���.";
                ElemeneClearText.color = Color.black;
                ElemeneClearText.fontSize = 50f;
                break;
            case 5:
                ElemeneClearText.text = "����Ͽ����ϴ�";
                ElemeneClearText.color = new Color(0.65f, 0.28f, 0.23f);
                ElemeneClearText.fontSize = 100f;
                ElemeneClearText.fontMaterial = fontMaterials[1];
                SoundManage.instance.PlaySFXSound(19, "System"); // ��� ����
                break;
            case 6:
                ElemeneClearText.text = "�����Ͽ����ϴ�";
                ElemeneClearText.color = new Color(1f, 1f, 0.5f);
                ElemeneClearText.fontSize = 100f;
                ElemeneClearText.fontMaterial = fontMaterials[1];
                SoundManage.instance.PlaySFXSound(18, "System"); // ���� ����
                break;
            case 7:
                ElemeneClearText.text = "������ �� á���ϴ�";
                ElemeneClearText.color = new Color(1f, 1f, 0.5f);
                ElemeneClearText.fontSize = 60f;
                ElemeneClearText.fontMaterial = fontMaterials[1];
                //SoundManage.instance.PlaySFXSound(18, "System"); // ���� ����
                break;
            default:
                ElemeneClearText.text = "�������� ���� �ؽ�Ʈ�Դϴ�";
                ElemeneClearText.color = Color.black;
                ElemeneClearText.fontSize = 100f;
                break;
        }

        if (index < 4)   // 4�� ���� ��� ��
            SoundManage.instance.PlaySFXSound(10, "System"); // ���� ȹ�� ����
    }
}
