using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUIManager : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image image; // ���� �̹���
    [SerializeField] private Text text; // ���� �ؽ�Ʈ
    [SerializeField] private Image progressBar;

    [SerializeField] private Sprite[] bgSprites = new Sprite[6];
    [SerializeField] private Sprite[] infoSprites = new Sprite[10];
    [SerializeField] private string[] str = new string[10];

    private float timer;
    private void Awake()
    {
        //progressBar = this.gameObject.transform.Find("ProgressBar").GetComponent<Image>();
        //image = this.gameObject.transform.Find("Image").GetComponent<Image>();
        //text = this.gameObject.transform.Find("Text").GetComponent<Text>();

        str[0] = "����� ��������� ���������� ü���� �ҰԵ˴ϴ�. \nindex 0";
        str[1] = "���� ���� �����Դϴ�. \nindex 1";
        str[2] = "�� ���� �����Դϴ�. \nindex 2";
        str[3] = "���� ���� �����Դϴ�. \nindex 3";
        str[4] = "���� ���� �����Դϴ�. \nindex 4";
        str[5] = "�ʵ� �����Դϴ�. \nindex 5";
        str[6] = "�ʵ� �����Դϴ�. \nindex 6";
        str[7] = "�ʵ� �����Դϴ�. \nindex 7";
        str[8] = "�ʵ� �����Դϴ�. \nindex 8 ";
        str[9] = "�ʵ� �����Դϴ�. \nindex 9";
    }

    private void OnEnable()
    {
        progressBar.fillAmount = 0.0f;
        timer = 0.0f;
    }
    private void SetInfo(int index)
    {
        if(index < 5)
            background.sprite = bgSprites[index];
        else
            background.sprite = bgSprites[5];

        image.sprite = infoSprites[index];

        text.text = str[index];
    }

    public void SetRandomInfo()
    {
        int index = Random.Range(5, 9);
        SetInfo(index);
    }
    public void SetDungeonInfo(string nextDungeonName)
    {
        switch(nextDungeonName)
        {
            case "FieldScene":
                SetRandomInfo();
                break;
            case "FireDungeon":
                SetInfo(0);
                break;
            case "IceDungeon":
                SetInfo(1);
                break;
            case "WaterDungeon":
                SetInfo(2);
                break;
            case "ElectricDungeon":
                SetInfo(3);
                break;
            case "BossDungeon":
                SetInfo(4);
                break;
            default:
                SetRandomInfo();
                break;
        }
    }
    public bool SetProgressBar(float progress)
    {
        if (progress < 0.9f)
            progressBar.fillAmount = progress;
        else
        {
            timer += Time.unscaledDeltaTime;
            progressBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);

            if (progressBar.fillAmount >= 1.0f)
                return true;
        }
        return false;

    }
    public void InitailizeTimer()
    {
        timer = 0.0f;
    }
}
