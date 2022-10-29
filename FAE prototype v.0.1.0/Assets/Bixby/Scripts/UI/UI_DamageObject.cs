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
    void Start()
    {
        timeLast = 0;
        TMP = this.GetComponent<TMPro.TextMeshPro>();
        height = 4f;
        lastTime = 1f;
        upSpeed = 2.5f;
        int temp;
        if (!int.TryParse(TMP.text, out temp))//������� ���
        {
            basePos = this.transform.root.position + Vector3.up * 2.5f;
            basePos.y = basePos.y + timeLast * upSpeed;
            this.transform.position = basePos;
            lastTime = 5f;
            this.gameObject.AddComponent<Rigidbody>();
            float randomX;
            if(Random.RandomRange(0, 2)==0)
                randomX = 1f;
            else
                randomX = -1f;
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(randomX*100f, 100f, 0));
        }
        if (this.transform.root.tag == "Player")//�÷��̾��� ���
        {
            TMP.fontSize = 6f;
            upSpeed = 1f;
        }
        //switch (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().MyElement)
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
