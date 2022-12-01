using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Control : MonoBehaviour
{
    public static UI_Control Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
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
    GameObject bossText;
    float bossTimer;
    void Start()
    {
        windows = new List<GameObject>();
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
            TextOn("�� Ÿ���� ���, �� ����� ���ϴ�.\n�� �Ӽ� ���� �������� �������ô�.");
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
                case "x"://����Ʈ ��ŵ
                    QuestObject.manager.SetIsClear(true);
                    break;
                case "p":
                    Speech.setUp("partnerA", "temp");
                    break;
            }
        }
        if (bossText.activeSelf&&bossTimer>=0.1f)
        {
            bossTimer += Time.deltaTime;
            if (bossTimer > 5)
            {
                bossText.SetActive(false);
                bossTimer = 0;
            }
        }
    }
    public void OptionWindow()
    {
        if (!windowClose())
            windowSet(optionObj);
    }
    public void windowSet(GameObject window)//â ���� �ݱ�
    {
        if (!windows.Contains(window) && window != optionObj)
            windows.Add(window);
        if (!(OpenedWindow == null || OpenedWindow == window))
            return;
        window.SetActive(!window.activeSelf);
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
                    break;// �� �� �� ��ġ�� �÷��̾� �������� �� ��
            }
            Cursor.lockState = CursorLockMode.None;
            aimPoint.SetActive(false);
            Time.timeScale = 0f;
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
        }
    }
    public bool windowClose()//�׳� â �ݱ�
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
                    UI_Control.Inst.damageSet("����", victim.gameObject);
                    break;
                case ElementRule.ElementType.ICE:
                    UI_Control.Inst.damageSet("�ñ�", victim.gameObject);
                    break;
                case ElementRule.ElementType.WATER:
                    UI_Control.Inst.damageSet("����", victim.gameObject);
                    break;
                case ElementRule.ElementType.ELECTRICITY:
                    UI_Control.Inst.damageSet("����", victim.gameObject);
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
}
