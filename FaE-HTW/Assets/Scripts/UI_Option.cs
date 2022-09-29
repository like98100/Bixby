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
        mouseSenseTextX.text = mouseSenseX.ToString();
        mouseSenseTextY.text = mouseSenseY.ToString();
    }
    public void senseSetup()
    {
        Camera.mouseSenseX = Mathf.Pow(10f, mouseSenseSliderX.value * 2f - 1);
        Camera.mouseSenseY = Mathf.Pow(10f, mouseSenseSliderY.value * 2f - 1);
    }
    public void senseStop()
    {
        Camera.mouseSenseX = 0f;
        Camera.mouseSenseY = 0f;
    }
}
