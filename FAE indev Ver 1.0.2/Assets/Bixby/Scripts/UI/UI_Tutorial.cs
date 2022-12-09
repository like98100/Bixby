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
        if (ElemeneClearText.gameObject.activeSelf)//텍스트 나온 후 튜토리얼 이미지가 나오는 경우
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
                    if (ElemeneClearText.text.Contains("사망"))
                        LoadingSceneController.Instance.ReloadScene();
                    else if (QuestObject.manager.GetIndex() == 5)//불 원소 해방 후
                        TutoImageSet(1);//원소 공격 튜토리얼
                    else if (QuestObject.manager.GetIndex() == 10)//얼음 원소 해방 후
                        TutoImageSet(2);//동료 전환 튜토리얼
                }
            }
        }
        if (QuestObject.manager.GetIndex() == 5 && QuestObject.manager.GetIsClear()&&!didText)//불 던전 퀘스트 클리어 후, 텍스트가 나온 적 없다면
        {
            ElementGetText(0);//불 원소 해방 텍스트
        }
    }

    public void TutoImageSet(int index)//0:전투, 1:상성, 2:아바타 변경, 3:채집사냥, 4:낚시, 5:요리, 6:조작, 7:맵 및 워프포인트
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
                ElemeneClearText.text = "<<불>>원소 힘 개방";
                ElemeneClearText.color = ElementControl.FireSkillStartColor;
                ElemeneClearText.fontSize = 100f;
                didText = true;
                break;
            case 1:
                ElemeneClearText.text = "<<얼음>>원소 힘 개방";
                ElemeneClearText.color = ElementControl.IceSkillStartColor;
                ElemeneClearText.fontSize = 100f;
                break;
            case 2:
                ElemeneClearText.text = "<<물>>원소 힘 개방";
                ElemeneClearText.color = ElementControl.WaterSkillStartColor;
                ElemeneClearText.fontSize = 100f;
                break;
            case 3:
                ElemeneClearText.text = "<<전기>>원소 힘 개방";
                ElemeneClearText.color = ElementControl.ElectroSkillStartColor;
                ElemeneClearText.fontSize = 100f;
                break;
            case 4:
                ElemeneClearText.text = "더이상 나아가는건 위험할 것 같다. 돌아가자.";
                ElemeneClearText.color = Color.black;
                ElemeneClearText.fontSize = 50f;
                break;
            case 5:
                ElemeneClearText.text = "사망하였습니다";
                ElemeneClearText.color = new Color(0.65f, 0.28f, 0.23f);
                ElemeneClearText.fontSize = 100f;
                ElemeneClearText.fontMaterial = fontMaterials[1];
                SoundManage.instance.PlaySFXSound(19, "System"); // 사망 사운드
                break;
            case 6:
                ElemeneClearText.text = "성공하였습니다";
                ElemeneClearText.color = new Color(1f, 1f, 0.5f);
                ElemeneClearText.fontSize = 100f;
                ElemeneClearText.fontMaterial = fontMaterials[1];
                SoundManage.instance.PlaySFXSound(18, "System"); // 성공 사운드
                break;
            case 7:
                ElemeneClearText.text = "가방이 다 찼습니다";
                ElemeneClearText.color = new Color(1f, 1f, 0.5f);
                ElemeneClearText.fontSize = 60f;
                ElemeneClearText.fontMaterial = fontMaterials[1];
                //SoundManage.instance.PlaySFXSound(18, "System"); // 성공 사운드
                break;
            default:
                ElemeneClearText.text = "정해지지 않은 텍스트입니다";
                ElemeneClearText.color = Color.black;
                ElemeneClearText.fontSize = 100f;
                break;
        }

        if (index < 4)   // 4개 원소 출력 시
            SoundManage.instance.PlaySFXSound(10, "System"); // 원소 획득 사운드
    }
}
