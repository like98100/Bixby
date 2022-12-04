using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public static bool isPlayerFishing = false; //�÷��̾ �� � ���� ����Ʈ���� ���ø� �ϴ��� ���°� �ϰ��ǰ� ����ȴ�.

    //���� ������ �� ����
    [SerializeField] protected Slider gageSlider; //������ �����̴�
    [SerializeField] protected Slider minSlider; //�ּҰ� �����̴�
    [SerializeField] protected Slider maxSlider; //�ִ밪 �����̴�

    [SerializeField] protected Text fishingText; //����,����,������ �� ���� �ؽ�Ʈ

    //��ġ�� �������� �ʱ�ȭ ���߿� Awake���� ����
    [SerializeField] protected float min_min = 0.4f; //�ּҰ��� �ּҰ�
    [SerializeField] protected float min_max = 0.7f; //�ּҰ��� �ִ밪
    [SerializeField] protected float sliderInterval = 0.3f; //�ּҰ� �ִ밪 ���� ����
    [SerializeField] protected float resultGageSpeed = 0.1f; //������ �ӵ�
    [SerializeField] protected float fishingTime = 5.0f; //���� �ð�
    [SerializeField] protected float fishingPower = 0.2f; //���� ��

    float dtime;

    [SerializeField] protected bool start = false; //�����ߴ��� �ƴ���
    [SerializeField] protected bool inputF = false; //fŰ �������� �ƴ���
    bool endFishing; //���ð� �������� �ƴ��� �Ǵ�.

    protected GameObject Player; //�÷��̾� ������Ʈ
    itemJsonData itemJsonData;//json������

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "Harvest");//json�ε�
        Player = GameObject.FindGameObjectWithTag("Player");

        gageSlider.gameObject.SetActive(false);
        minSlider.gameObject.SetActive(false);
        maxSlider.gameObject.SetActive(false);
        fishingText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        dtime = Time.deltaTime;
        initialization();
    }

    // Update is called once per frame
    void Update()
    {
        Fishing_Start();

        if (Input.GetKeyDown(KeyCode.Space) && start && inputF && endFishing)
        {
            initialization();
        }
    }

    //�ʱ�ȭ
    void initialization()
    {
        isPlayerFishing = false; //�÷��̾ �������ΰ�? No.
        endFishing = true;
        start = false;
        inputF = false;
        gageSlider.value = 0.5f;

        //UI����
        //���� ���ϴ°� Ǯ��

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            UI_F();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UI_F();
        }
    }

    //���� FŰ ���� �Լ�
    void UI_F()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f && !start && !inputF)
        {
            //fŰ ����
            if (!inventoryObject.Inst.FieldFKey.activeSelf)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
            }
        }
        else if (Vector3.Distance(Player.transform.position, transform.position) > 2.0f || start || inputF)
        {
            //fŰ ����
            inventoryObject.Inst.FieldFKey.SetActive(false);
        }
    }

    //���� �Լ�
    void Fishing_Start()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f && !start && !inputF)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isPlayerFishing = true; //�÷��̾ �������ΰ�? Yes.
                endFishing = false;
                inputF = true;

                gageSlider.gameObject.SetActive(true);
                minSlider.gameObject.SetActive(true);
                maxSlider.gameObject.SetActive(true);
                fishingText.gameObject.SetActive(true);



                StartCoroutine(Check_Start());

                //�ð��Ȱ����ϱ�->windowSet�� case�÷��� ����ؾ� �ҵ���
                //Time.timeScale = 0f;
            }
        }

        if (start)
        {
            //�����Ҷ� ���� ���ϰ� �ϱ�

            //������ ����
            gageSlider.value -= resultGageSpeed * dtime;
            //������ ����
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gageSlider.value += fishingPower;
            }
        }
    }

    int num = 3; //�ð�

    //���� �Ǵ�
    IEnumerator Check_Start()
    {
        //�������� �� �ֱ�
        minSlider.value = Random.Range(min_min, min_max);
        maxSlider.value = minSlider.value + sliderInterval;
        //UI ����

        while (num >= 1 && !start)
        {
            fishingText.text = num.ToString();
            yield return new WaitForSeconds(1.0f);
            num--;
        }
        fishingText.text = "";
        start = true;
        num = 3;
        //���� �ð�
        StartCoroutine(StartFishing());
    }

    //������ ������
    itemData fishData;

    //�ڷ�ƾ ����ؼ� ������ �ð� ����
    IEnumerator StartFishing()
    {
        yield return new WaitForSeconds(fishingTime);

        //������ ȹ��
        if (gageSlider.value >= minSlider.value && gageSlider.value <= maxSlider.value)
        {
            fishingText.text = "success";

            fishData = new itemData();

            foreach (var item in itemJsonData.itemList)
            {
                if (item.itemID == 1003)
                {
                    fishData = item;
                }
            }

            Vector2 tempPos;
            tempPos = inventoryObject.Inst.emptyCell(fishData.xSize, fishData.ySize);
            inventoryObject.Inst.itemGet(fishData.xSize, fishData.ySize, tempPos.x, tempPos.y, fishData);

            //�κ��丮 �߰� �� ���̽� ����
            inventoryObject.Inst.jsonSave();
        }
        else
        {
            fishingText.text = "fail";
        }
        endFishing = true; //�����ٰ� �˷��ֱ�

        yield return new WaitForSeconds(1.0f);

        gageSlider.gameObject.SetActive(false);
        minSlider.gameObject.SetActive(false);
        maxSlider.gameObject.SetActive(false);
        fishingText.gameObject.SetActive(false);

        //�ð��Ȱ����ϱ�->windowSet�� case�÷��� ����ؾ� �ҵ���
        //Time.timeScale = 1f;

        //�ʱ�ȭ
        //start = false; 
        //inputF = false;
        //gageSlider.value = 0.5f;

        //yield return new WaitForSeconds(1.0f);
    }
}
