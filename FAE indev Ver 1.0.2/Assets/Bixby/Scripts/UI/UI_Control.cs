using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Control : MonoBehaviour
{
    public static UI_Control Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title")
            return;
        optionObj = GameObject.Find("Option");
        option = optionObj.GetComponent<UI_Option>();
        inventory = GameObject.Find("Inventory");
        Speech = this.gameObject.GetComponent<Speech>();
        Map = GameObject.Find("Map");
        aimPoint = GameObject.Find("AimPoint");
        Shop = this.gameObject.GetComponent<Shop>();
        Mission = this.gameObject.GetComponent<Mission>();
        EnemyHp = this.gameObject.GetComponent<UI_EnemyHp>();
    }
    GameObject optionObj;
    UI_Option option;
    GameObject inventory;
    public Speech Speech;
    public GameObject Map;
    List<GameObject> windows;
    public GameObject OpenedWindow;
    GameObject aimPoint;
    public Shop Shop;
    public Mission Mission;
    [SerializeField] GameObject damagePrefab;
    public UI_EnemyHp EnemyHp;
    bool isField;
    private float genCoolDown = 0.5f;
    GameObject bossText = null;
    float bossTimer;
    void Start()
    {
        windows = new List<GameObject>();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title")
            return;
        windows.Add(inventory);
        windows.Add(Map);
        windows.Add(Shop.getWindow());
        isField = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FieldScene";
        if (isField)
            windows.Add(GameObject.Find("COOK").transform.GetChild(0).gameObject);
        option.Set();
        Map.SetActive(false);
        OpenedWindow = null;
        Cursor.lockState = CursorLockMode.Locked;
        bossText = GameObject.Find("bossText");
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "BossDungeon")
            bossText.SetActive(false);
        else
        {
            TextOn("불 타입의 경우, 불 덩어리를 쏩니다.\n물 속성 공격으로 끌 수 있습니다.");
            SoundManage.instance.PlaySFXSound(11, "System"); // 보스 힌트 사운드
        }
        bossTimer = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "BossDungeon" ? 0 : 0.1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OptionWindow();
        if (Input.anyKeyDown)
        {
            string temp = Input.inputString;

            switch (temp)
            {
                case "I":
                case "i":
                    windowSet(inventory);
                    break;
                case "M":
                case "m":
                    if (isField)
                        windowSet(Map);
                    break;
                case "X":
                case "x"://퀘스트 스킵
                    QuestObject.manager.SetIsClear(true);
                    break;
                case "p":
                    Speech.setUp("partnerA", "temp");
                    break;
            }
        }
        if (bossText != null)
            bossTimerFunc();


    }
    public void OptionWindow()
    {
        if (!windowClose())
            windowSet(optionObj);
    }
    public void windowSet(GameObject window)//창 열고 닫기
    {
        if (!windows.Contains(window) && window != optionObj)
            windows.Add(window);
        if (!(OpenedWindow == null || OpenedWindow == window))
            return;
        window.SetActive(!window.activeSelf);
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title")
            return;
        option.senseSet(window.activeSelf);
        if (window.activeSelf)
        {
            OpenedWindow = window;
            switch (window.name)
            {
                case "Shop":
                    inventory.SetActive(true);
                    break;
                case "Map":
                    Map.GetComponent<UI_Map>().MapSetUp();
                    break;// 열 때 맵 위치를 플레이어 중점으로 할 것
                case "GameObject":
                    if (window.transform.parent.name == "COOK")
                        window.transform.GetChild(window.transform.childCount - 2).GetComponent<CookingGage>().CookInitialize();
                    break;
            }
            Cursor.lockState = CursorLockMode.None;
            aimPoint.SetActive(false);
            Time.timeScale = 0f;

            if(window.name != "Speech") SoundManage.instance.PlaySFXSound(2, "System"); // 대화창을 제외한 UI 활성화 사운드
        }
        else
        {
            OpenedWindow = null;
            switch (window.name)
            {
                case "Shop":
                    inventory.SetActive(false);
                    inventory.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                case "Inventory":
                    inventory.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                case "Canvas":
                    foreach (Transform item in window.transform)
                    {
                        foreach (Transform item_ in item)
                        {
                            item_.gameObject.SetActive(false);
                        }
                    }
                    break;
                default:
                    break;
            }
            Cursor.lockState = CursorLockMode.Locked;
            aimPoint.SetActive(true);
            Time.timeScale = 1f;

            if (window.name != "Speech") SoundManage.instance.PlaySFXSound(3, "System"); // 대화창을 제외한 UI 비활성화 사운드
        }
    }
    public bool windowClose()//그냥 창 닫기
    {
        bool result = false;
        foreach (var item in windows)
        {
            if (item.activeSelf)
            {
                windowSet(item);
                result = true;
            }
        }
        return result;
    }

    public void damageSet(string content, GameObject subject)
    {
        GameObject temp = Instantiate(damagePrefab, subject.transform);
        temp.GetComponent<TMPro.TextMeshPro>().text = content;
    }

    public void ElementStateGen(GameObject victim, float timming)
    {
        if (victim.GetComponent<ElementControl>().ElementStack.Count != 0 && timming >= 0.5f)
        {
            switch (victim.GetComponent<ElementControl>().ElementStack.Peek())
            {
                case ElementRule.ElementType.FIRE:
                    UI_Control.Inst.damageSet("열기", victim.gameObject);
                    break;
                case ElementRule.ElementType.ICE:
                    UI_Control.Inst.damageSet("냉기", victim.gameObject);
                    break;
                case ElementRule.ElementType.WATER:
                    UI_Control.Inst.damageSet("습기", victim.gameObject);
                    break;
                case ElementRule.ElementType.ELECTRICITY:
                    UI_Control.Inst.damageSet("전기", victim.gameObject);
                    break;
            }
        }
    }

    public void TextOn(string value)
    {
        bossText.SetActive(true);
        bossText.GetComponent<TMPro.TextMeshProUGUI>().text = value;
        bossTimer = 0.1f;
    }

    void bossTimerFunc()
    {
        if (bossText.activeSelf && bossTimer >= 0.1f)
        {
            bossTimer += Time.deltaTime;
            if (bossTimer > 5)
            {
                bossText.SetActive(false);
                bossTimer = 0;
            }
        }
    }
}
