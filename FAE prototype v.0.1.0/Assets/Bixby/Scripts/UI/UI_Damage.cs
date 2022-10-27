using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Damage : MonoBehaviour
{
    float timeLast;
    Vector3 basePos;
    float baseY;
    TMPro.TextMeshProUGUI TMP;
    void Start()
    {
        timeLast = 0;
        baseY = basePos.y;
        TMP = this.GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        timeLast += Time.deltaTime;
        basePos.y = baseY + timeLast * 20f;
        this.transform.localPosition = basePos;
        Color tempColor = TMP.color;
        tempColor.a = (3f - timeLast) / 3f;
        TMP.color = tempColor;
        if (TMP.color.a <= 0.1f)
            Destroy(this.gameObject);
    }

    public void setup()
    {
        basePos = this.transform.localPosition;
    }
}
