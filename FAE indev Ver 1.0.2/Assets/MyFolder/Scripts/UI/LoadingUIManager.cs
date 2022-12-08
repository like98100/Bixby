using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUIManager : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image image; // 설명 이미지
    [SerializeField] private Image progressBar;

    [SerializeField] private Sprite[] bgSprites = new Sprite[6];
    [SerializeField] private Sprite[] infoSprites;

    private float timer;

    private void OnEnable()
    {
        progressBar.fillAmount = 0.0f;
        timer = 0.0f;
    }


    private void SetRandomInfo(int index)
    {
        background.sprite = bgSprites[index];

        int random = Random.Range(0, infoSprites.Length - 1);
        image.sprite = infoSprites[random];
    }
    public void SetDungeonInfo(string nextDungeonName)
    {
        switch (nextDungeonName)
        {
            case "FieldScene":
                SetRandomInfo(5);
                break;
            case "FireDungeon":
                SetRandomInfo(0);
                break;
            case "IceDungeon":
                SetRandomInfo(1);
                break;
            case "WaterDungeon":
                SetRandomInfo(2);
                break;
            case "ElectricDungeon":
                SetRandomInfo(3);
                break;
            case "BossDungeon":
                SetRandomInfo(4);
                break;
            default:
                SetRandomInfo(5);
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
}
