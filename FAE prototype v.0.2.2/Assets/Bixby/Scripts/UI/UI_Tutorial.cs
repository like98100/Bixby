using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
    GameObject canvas;
    List<GameObject> tutorialImages;
    TMPro.TextMeshProUGUI elemeneClearText;
    GameObject openedImage;
    int openedIndex;
    float time;
    bool didText;
    // Start is called before the first frame update
    void Start()
    {
        canvas = this.gameObject.transform.GetChild(0).gameObject;
        tutorialImages = new List<GameObject>();
        elemeneClearText = null;
        openedImage = null;
        openedIndex = 0;
        time = 0;
        didText = false;
        foreach (Transform item in canvas.transform)
        {
            if (item.gameObject.GetComponent<TMPro.TextMeshProUGUI>())
            {
                elemeneClearText = item.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                elemeneClearText.gameObject.SetActive(false);
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
        if (elemeneClearText.gameObject.activeSelf)
        {
            time += Time.deltaTime;
            Color temp = elemeneClearText.color;
            temp.a = (3f - time) / 3f;
            elemeneClearText.color = temp;
            if (temp.a < 0.1f)
            {
                elemeneClearText.gameObject.SetActive(false);
                canvas.SetActive(false);
                time = 0;
                if (QuestObject.manager.GetIndex() == 5)
                    TutoImageSet(1);
                else if(QuestObject.manager.GetIndex() == 10)
                    TutoImageSet(2);
            }
        }
        if (QuestObject.manager.GetIndex() == 5 && QuestObject.manager.GetIsClear()&&!didText)
        {
            ElementGetText(0);
        }
    }

    public void TutoImageSet(int index)//0:����Ʃ�丮��, 1:��Ʃ�丮��, 2:�ƹ�Ÿ����Ʃ�丮��
    {
        UI_Control.Inst.windowSet(canvas);
        openedIndex = index;
        elemeneClearText.gameObject.SetActive(false);
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
        canvas.SetActive(true);
        elemeneClearText.gameObject.SetActive(true);
        switch (index)
        {
            case 0:
                elemeneClearText.text = "<<��>>���� �� ����";
                elemeneClearText.color = ElementControl.FireSkillStartColor;
                break;
            case 1:
                elemeneClearText.text = "<<����>>���� �� ����";
                elemeneClearText.color = ElementControl.IceSkillStartColor;
                break;
            case 2:
                elemeneClearText.text = "<<��>>���� �� ����";
                elemeneClearText.color = ElementControl.WaterSkillStartColor;
                break;
            case 3:
                elemeneClearText.text = "<<����>>���� �� ����";
                elemeneClearText.color = ElementControl.ElectroSkillStartColor;
                break;
            default:
                elemeneClearText.text = "<<���ǵ�������>>���� �� ����";
                elemeneClearText.color = Color.black;
                break;
        }
        didText = true;
    }
}
