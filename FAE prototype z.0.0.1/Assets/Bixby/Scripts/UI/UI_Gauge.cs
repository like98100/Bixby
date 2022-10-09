using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gauge : MonoBehaviour
{
    Slider hp;
    Slider stamina;
    PlayerContorl player;
    Image staminaBack;
    float staminaBackAmount;
    float timeTack;
    Slider attackCharge;
    CamControl cameraControl;
    void Start()
    {
        hp = this.transform.GetChild(0).GetComponent<Slider>();
        stamina = this.transform.GetChild(1).GetComponent<Slider>();
        attackCharge = this.transform.GetChild(2).GetComponent<Slider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>();
        staminaBack = this.transform.GetChild(1).GetChild(1).GetComponent<Image>();
        cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamControl>();
        staminaBackAmount = player.MyStartingStamina;
        timeTack = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.State == PlayerContorl.STATE.ATTACK && cameraControl.step == CamControl.STATE.AIM)
        {
            attackCharge.gameObject.SetActive(true);
            attackCharge.value = player.StateTimer / 2f;
        }
        else
            attackCharge.gameObject.SetActive(false);
        timeTack += Time.deltaTime;
        hp.value = player.Health / player.MyStartingHealth;
        stamina.value = player.Stamina / player.MyStartingStamina;
        if (timeTack > 0.5f)
        {
            staminaBackAmount = Mathf.Abs(staminaBackAmount - player.Stamina) < 1 ? player.Stamina : Mathf.Lerp(player.Stamina, staminaBackAmount, 0.5f);
            staminaBack.fillAmount = staminaBackAmount / player.MyStartingStamina;
            timeTack = 0f;
        }
    }
}
