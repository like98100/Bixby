using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterFrame : MonoBehaviour
{
    PlayerContorl playerContorl;
    [SerializeField] int ableElementCount;//��� ������ �Ӽ� ��, ���� �����Կ� ���� �� ��ũ��Ʈ������ ���� ����
    List<GameObject> avaters;
    void Start()
    {
        //�ν����� ó���Լ�
        playerContorl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>();
        avaters = new List<GameObject>();
        foreach (Transform item in this.transform)
            avaters.Add(item.gameObject);
        SetAvatars();
    }

    void Update()
    {
        if (Input.anyKey)//�ƹ�Ÿ ���� Ű�� ������ ���� ���氡��, �ۺ��̶� �÷��̾� ��Ʈ�ѿ��� �ҷ��� ��
            NowAvater();
    }
    public void SetAvatars()//������ �ƹ�Ÿ�� ����ȭ, �Ұ����� �ƹ�Ÿ �񰡽�ȭ
    {
        float height = avaters[0].GetComponent<RectTransform>().rect.height * 1.5f;
        int i = 0;
        foreach (var item in avaters)
        {
            if (i < ableElementCount)
            {
                item.SetActive(true);
                item.transform.localPosition = Vector3.zero + Vector3.up * (0.5f * ableElementCount - (i + 0.5f)) * height;
            }
            else
                item.SetActive(false);
            i++;
        }
        NowAvater();
    }
    public void NowAvater()//���� �������� �ƹ�Ÿ �׵θ� �� �ٸ��� �ϴ°�. ����ġ ���� �Ӽ����� �� �ٸ��� �� �� �� ����
    {
        foreach (var item in avaters)
        {
            UnityEngine.UI.Image avatarImage = item.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
            UnityEngine.UI.Text avatarText = item.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>();
            if (avaters.IndexOf(item) == ((int)playerContorl.MyElement))
                item.GetComponent<UnityEngine.UI.Image>().color = avatarImage.color = avatarText.color = Color.white;
            else
            {
                avatarImage.color = Color.clear;
                Color tempColor = Color.white; tempColor.a = 0.5f;
                item.GetComponent<UnityEngine.UI.Image>().color = avatarText.color = tempColor;
            }
                
        }
    }
}
