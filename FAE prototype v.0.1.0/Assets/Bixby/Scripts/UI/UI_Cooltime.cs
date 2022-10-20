using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Cooltime : MonoBehaviour
{
    Image coolTimeImage;
    Text coolTimeCount;
    void Start()
    {
        coolTimeImage = this.transform.GetChild(0).GetComponent<Image>();
        coolTimeCount = this.transform.GetChild(1).GetComponent<Text>();
    }
    [SerializeField] float coolTime;//���� �÷��̾�� ���� ������ ��
    [SerializeField] float maxCoolTime;//���� �÷��̾�� ���� ������ ��
    void Update()
    {
        if (coolTime > 0)
            coolTime -= Time.deltaTime;//���� ��Ÿ�� ���̴� �� �Ƹ� �÷��̾�� ���״� ������ ������ �Ŀ��� ����
        coolTimeImage.fillAmount = coolTime / maxCoolTime;
        coolTimeCount.text = coolTime <= 0 ? "" : Mathf.FloorToInt(coolTime + 1).ToString();
    }
}
