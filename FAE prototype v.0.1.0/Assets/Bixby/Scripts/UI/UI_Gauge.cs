using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gauge : MonoBehaviour
{
    Slider hp;
    Slider stamina;
    PlayerContorl playerControl;
    GameObject staminaBack;
    float staminaBackAmount;
    float timeTack;
    Slider attackCharge;
    CamControl cameraControl;
    Vector3 staminaOriginPos;
    GameObject staminaObj;
    public class enemyHp
    {
        public List<GameObject> hpObjects;
        public List<Enemy> enemies;
        public enemyHp()
        {
            hpObjects = new List<GameObject>();
            enemies = new List<Enemy>();
        }
    }
    public static enemyHp EnemyHps;
    void Start()
    {
        staminaObj = this.transform.GetChild(1).gameObject;
        hp = this.transform.GetChild(0).GetComponent<Slider>();
        stamina = staminaObj.GetComponent<Slider>();
        attackCharge = this.transform.GetChild(2).GetComponent<Slider>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>();
        staminaBack = staminaObj.transform.GetChild(1).gameObject;
        cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamControl>();
        staminaBackAmount = playerControl.MyStartingStamina;
        timeTack = 0f;
        staminaOriginPos = staminaObj.transform.position;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyHps = new enemyHp();
        foreach (var item in enemies)
        {
            EnemyHps.hpObjects.Add(Instantiate(hp.gameObject, this.transform.parent));
            EnemyHps.enemies.Add(item.GetComponent<Enemy>());
            EnemyHps.hpObjects[EnemyHps.hpObjects.Count - 1].transform.position =
            Camera.main.WorldToScreenPoint(item.transform.position + Vector3.up * 1.3f);
            EnemyHps.hpObjects[EnemyHps.hpObjects.Count - 1].transform.localScale = Vector3.one * 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        staminaObj.SetActive(stamina.value != 1);
        staminaObj.transform.position = cameraControl.step == CamControl.STATE.AIM ? staminaOriginPos :
            Vector3.Lerp(staminaObj.transform.position, Camera.main.WorldToScreenPoint(playerControl.gameObject.transform.position + Vector3.up * 1.3f) + Vector3.right * 200f, Time.deltaTime * 15.0f);

        staminaObj.transform.localScale = cameraControl.step == CamControl.STATE.AIM ? Vector3.one : new Vector3(0.7f, 1, 1);
        attackCharge.gameObject.SetActive(playerControl.State == PlayerContorl.STATE.ATTACK && cameraControl.step == CamControl.STATE.AIM);
        if (attackCharge.gameObject.activeSelf)
            attackCharge.value = playerControl.StateTimer / playerControl.SwitchToChargeTime;
        timeTack += Time.deltaTime;
        hp.value = playerControl.Health / playerControl.MyStartingHealth;
        stamina.value = playerControl.Stamina / playerControl.MyStartingStamina;
        if (timeTack > 0.5f)
        {
            staminaBackAmount = Mathf.Abs(staminaBackAmount - playerControl.Stamina) < 1 ? playerControl.Stamina : Mathf.Lerp(playerControl.Stamina, staminaBackAmount, 0.5f);
            staminaBack.GetComponent<RectTransform>().sizeDelta = new Vector2(staminaBackAmount / playerControl.MyStartingStamina * 200f, 0);
            timeTack = 0f;
        }
        foreach (var item in EnemyHps.hpObjects)
        {
            if (EnemyHps.enemies[EnemyHps.hpObjects.IndexOf(item)].gameObject == null)
                continue;
            item.GetComponent<Slider>().value = EnemyHps.enemies[EnemyHps.hpObjects.IndexOf(item)].Stat.hp / EnemyHps.enemies[EnemyHps.hpObjects.IndexOf(item)].Stat.maxHp;
            item.transform.position = Camera.main.WorldToScreenPoint(EnemyHps.enemies[EnemyHps.hpObjects.IndexOf(item)].gameObject.transform.position + Vector3.up * 1.3f);
        }
    }
}