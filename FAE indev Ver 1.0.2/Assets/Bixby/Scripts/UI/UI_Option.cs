using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option : MonoBehaviour
{
    private void Awake()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
        {
            cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamControl>();
            mouseSenseSliderX = this.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
            mouseSenseSliderY = this.transform.GetChild(0).GetChild(1).GetComponent<Slider>();
            mouseSenseTextX = this.transform.GetChild(0).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
            mouseSenseTextY = this.transform.GetChild(0).GetChild(3).GetComponent<TMPro.TextMeshProUGUI>();
            BGMSlider = this.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
            SFXSlider = this.transform.GetChild(1).GetChild(1).GetComponent<Slider>();
            BGMText = this.transform.GetChild(1).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
            SFXText = this.transform.GetChild(1).GetChild(3).GetComponent<TMPro.TextMeshProUGUI>();
            Button closeBtn = this.transform.GetChild(this.transform.childCount - 1).GetComponent<Button>();
            closeBtn.onClick.AddListener(() => UI_Control.Inst.windowSet(this.gameObject));
        }
    }
    CamControl cameraControl;
    Slider mouseSenseSliderX;
    Slider mouseSenseSliderY;
    TMPro.TextMeshProUGUI mouseSenseTextX;
    TMPro.TextMeshProUGUI mouseSenseTextY;
    Slider BGMSlider;
    Slider SFXSlider;
    TMPro.TextMeshProUGUI BGMText;
    TMPro.TextMeshProUGUI SFXText;
    float mouseSenseX;
    float mouseSenseY;
    public void Set()
    {
        mouseSenseSliderX.value = 0.2f;
        mouseSenseSliderY.value = 0.2f;
        mouseSenseX = mouseSenseSliderX.value * 99 + 1f;
        mouseSenseY = mouseSenseSliderY.value * 99 + 1f;
        BGMSlider.value = 0.7f;
        SFXSlider.value = 1f;
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
        {
            mouseSenseX = mouseSenseSliderX.value * 99 + 1f;
            mouseSenseY = mouseSenseSliderY.value * 99 + 1f;
            mouseSenseTextX.text = Mathf.FloorToInt(mouseSenseX).ToString();
            mouseSenseTextY.text = Mathf.FloorToInt(mouseSenseY).ToString();
            BGMText.text = Mathf.FloorToInt(BGMSlider.value * 99 + 1).ToString();
            SFXText.text = Mathf.FloorToInt(SFXSlider.value * 99 + 1).ToString();
        }
    }
    public void senseSet(bool isStop)
    {
        cameraControl.mouseSenseX = isStop ? 0 : mouseSenseX * 0.05f;
        cameraControl.mouseSenseY = isStop ? 0 : mouseSenseY * 0.05f;
        SoundManage.instance.SetVolume(true, isStop ? 0 : BGMSlider.value);
        SoundManage.instance.SetVolume(false, isStop ? 0 : SFXSlider.value);
    }

    public void TitleSet(bool start)
    {
        if (start)
            LoadingSceneController.Instance.LoadScene("FieldScene");
        else
            Application.Quit();
    }
    public void toTitle()
    {
        UI_Control.Inst.windowSet(this.gameObject);
        Cursor.lockState = CursorLockMode.None;
        LoadingSceneController.Instance.LoadScene("Title");
        QuestObject.manager.QuestInitialize();
        Time.timeScale = 1f;
    }
}
