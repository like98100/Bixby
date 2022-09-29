using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option : MonoBehaviour
{
    [SerializeField] CamControl Camera;
    [SerializeField] Slider mouseSenseSliderX;
    [SerializeField] Slider mouseSenseSliderY;
    [SerializeField] Text mouseSenseTextX;
    [SerializeField] Text mouseSenseTextY;
    void Start()
    {
        mouseSenseSliderX.value = 0.5f;
        mouseSenseSliderY.value = 0.5f;
    }

    void Update()
    {
        float mouseSenseX = mouseSenseSliderX.value * 100;
        float mouseSenseY = mouseSenseSliderY.value * 100;
        mouseSenseTextX.text = Mathf.FloorToInt(mouseSenseX).ToString();
        mouseSenseTextY.text = Mathf.FloorToInt(mouseSenseY).ToString();
    }
    public void senseSetup()
    {
        Camera.mouseSenseX = Mathf.FloorToInt(Mathf.Pow(10f, mouseSenseSliderX.value * 2f - 1));
        Camera.mouseSenseY = Mathf.FloorToInt(Mathf.Pow(10f, mouseSenseSliderY.value * 2f - 1));
    }
    public void senseStop()
    {
        Camera.mouseSenseX = 0;
        Camera.mouseSenseY = 0;
    }
}
