using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DamageObject : MonoBehaviour
{
    float timeLast;//���ӵ� �ð�
    Vector3 basePos;
    TMPro.TextMeshPro TMP;
    float lastTime;//������ �ð�
    float upSpeed;//�ö󰡴� �ӵ�
    float height;//���� �� ���س���
    [SerializeField] List<Material> fontMaterials;
    void Start()
    {
        timeLast = 0;
        TMP = this.GetComponent<TMPro.TextMeshPro>();
        height = 4f;
        lastTime = 1f;
        upSpeed = 2.5f;
        int temp;
        if (!int.TryParse(TMP.text, out temp))//������� �ƴ� ���
        {
            TMP.fontSize = 6f;
            TMP.fontMaterial = fontMaterials[0];
            basePos = this.transform.root.position + Vector3.up * 2.5f;
            basePos.y = basePos.y + timeLast * upSpeed;
            this.transform.position = basePos;
            lastTime = 5f;
            this.gameObject.AddComponent<Rigidbody>();
            Vector3 upper = Vector3.up * 100f;
            if (Random.RandomRange(0, 2) == 0)
                upper += GameObject.FindGameObjectWithTag("MainCamera").transform.right * 100f;
            else
                upper += GameObject.FindGameObjectWithTag("MainCamera").transform.right * -100f;
            bool isSynergy = false;
            switch (TMP.text)
            {
                case "����":
                    TMP.color = ElementControl.FireSkillStartColor;
                    break;
                case "�ñ�":
                    TMP.color = ElementControl.IceSkillStartColor;
                    break;
                case "����":
                    TMP.color = ElementControl.WaterSkillStartColor;
                    break;
                case "����":
                    TMP.color = ElementControl.ElectroSkillStartColor;
                    break;
                default:
                    isSynergy = true;
                    break;
            }
            if (isSynergy)
            {
                switch (TMP.text)
                {
                    case "����":
                        TMP.color = ElementControl.IceSkillStartColor;
                        break;
                    case "����":
                        TMP.color = ElementControl.IceSkillStartColor;
                        break;
                    case "����":
                        TMP.color = ElementControl.ElectroSkillStartColor;
                        break;
                    case "����":
                        TMP.color = ElementControl.FireSkillStartColor;
                        break;
                    case "����":
                        TMP.color = ElementControl.ElectroSkillStartColor;
                        break;
                    case "����":
                        TMP.color = ElementControl.WaterSkillStartColor;
                        break;
                    default:
                        break;
                }
                string tmp = TMP.text;
                TMP.text = "<b>" + tmp + "</b>";
                TMP.fontSize = 10f;
                lastTime = 1f;
                this.gameObject.GetComponent<Rigidbody>().useGravity = false;
                upper = Vector3.up * 100;
            }
            this.gameObject.GetComponent<Rigidbody>().AddForce(upper);

        }
        else
        {
            if (this.transform.root.tag == "Player")//�÷��̾ ������� �޴� ���
            {
                TMP.fontSize = 6f;
                upSpeed = 1f;
                switch (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().EnemyElement)
                {
                    case ElementRule.ElementType.FIRE:
                        TMP.color = ElementControl.FireSkillStartColor;
                        break;
                    case ElementRule.ElementType.ICE:
                        TMP.color = ElementControl.IceSkillStartColor;
                        break;
                    case ElementRule.ElementType.WATER:
                        TMP.color = ElementControl.WaterSkillStartColor;
                        break;
                    case ElementRule.ElementType.ELECTRICITY:
                        TMP.color = ElementControl.ElectroSkillStartColor;
                        break;
                    default:
                        TMP.color = Color.yellow;
                        break;
                }
                //if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().IsElectronicShock ||
                //    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().IsTransmission)
                //    TMP.color = ElementControl.ElectroSkillStartColor;
            }
            else//���� ������� �޴� ���
            {
                switch (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().MyElement)
                {
                    case ElementRule.ElementType.FIRE:
                        TMP.color = ElementControl.FireSkillStartColor;
                        break;
                    case ElementRule.ElementType.ICE:
                        TMP.color = ElementControl.IceSkillStartColor;
                        break;
                    case ElementRule.ElementType.WATER:
                        TMP.color = ElementControl.WaterSkillStartColor;
                        break;
                    case ElementRule.ElementType.ELECTRICITY:
                        TMP.color = ElementControl.ElectroSkillStartColor;
                        break;
                    default:
                        TMP.color = Color.yellow;
                        break;
                }
            }
        }
    }
    void Update()
    {
        Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        //camRotate -= Vector3.right * 90f;
        timeLast += Time.deltaTime;
        if (this.gameObject.GetComponent<Rigidbody>() == null)
        {
            basePos = this.transform.root.position + Vector3.up * height;
            basePos.y = basePos.y + timeLast * upSpeed;
            this.transform.position = basePos;
        }
        this.transform.rotation = Quaternion.Euler(camRotate);
        Color tempColor = TMP.color;
        tempColor.a = (lastTime - timeLast) / lastTime;
        TMP.color = tempColor;
        if (TMP.color.a <= 0.1f)
            Destroy(this.gameObject);
    }
}
