using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName; // ������ �̸�
    [SerializeField] protected int hp;  // ������ ü�� -> ���� 2�� ���������� ����

    [SerializeField] protected float walkSpeed;  // �ȱ� �ӷ�
    [SerializeField] protected float runSpeed;  // �޸��� �ӷ�
    [SerializeField] protected float comebackSpeed;  // ���ƿ��¼ӵ�
    protected float applySpeed;

    protected Vector3 destination;  // ������
    [SerializeField] protected Vector3 spawnPosition; //������ ��ġ

    // ���� ����
    protected bool isAction;  // �ൿ ������ �ƴ��� �Ǻ�
    protected bool isWalking; // �ȴ���, �� �ȴ��� �Ǻ�
    protected bool isRunning; // �޸����� �Ǻ�
    protected bool isDead;   // �׾����� �Ǻ�
    public bool isComeBack; //���ƿ����ִ��� �Ǻ�
    public bool testchack = true; //������Ʈ ���� üũ

    [SerializeField] protected float walkTime;  // �ȱ� �ð�
    [SerializeField] protected float waitTime;  // ��� �ð�
    [SerializeField] protected float runTime;  // �ٱ� �ð�
    protected float currentTime;


    // �ʿ��� ������Ʈ
    [SerializeField] protected Animator anim;

    protected NavMeshAgent nav;


    protected GameObject Player;
    itemJsonData itemJsonData;//json������

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "Harvest");//json�ε�
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        currentTime = waitTime;   // ��� ����
        isAction = true;   // ��⵵ �ൿ
        spawnPosition = this.transform.position; //���� ��ġ�� ���� �̰� �ٲ���Ѵ�.
    }

    //������ �ִ��� üũ�ϴ� ����
    public bool checkNear = false;

    // Update is called once per frame
    void Update()
    {
        //���׾�����
        if (!isDead)
        {
            Move();
            ElapseTime();
        }

        //�׾�����
        if (isDead && Vector3.Distance(Player.transform.position, transform.position) <= 2.0f)
        {
            checkNear = true;

            //fŰ ����
            if (!inventoryObject.Inst.FieldFKey.activeSelf)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
            }
        }
        else if (isDead && Vector3.Distance(Player.transform.position, transform.position) > 2.0f && checkNear == true)
        {
            //fŰ ����
            inventoryObject.Inst.FieldFKey.SetActive(false);

            checkNear = false;
        }

        if (checkNear && Input.GetKeyDown(KeyCode.F))
        {
            //������ ȹ��
            animalData = new itemData();
            //������ ���� ��ũ��Ʈ
            animalData.itemID = 2005; animalData.tag = new string[] { "food", "harvest" }; animalData.itemName = "�������";
            animalData.Left = -1; animalData.Up = -1; animalData.xSize = 1; animalData.ySize = 1;
            animalData.isEquip = false; animalData.isSell = false;
            animalData.price = 2; //���߿� ���� ����

            Vector2 tempPos;
            tempPos = inventoryObject.Inst.emptyCell(animalData.xSize, animalData.ySize);
            inventoryObject.Inst.itemGet(animalData.xSize, animalData.ySize, tempPos.x, tempPos.y, animalData);

            //�κ��丮 �߰� �� ���̽� ����
            inventoryObject.Inst.jsonSave();

            //���� ����
            //Destroy(gameObject);
            this.gameObject.SetActive(false);
            testchack = false;

            //fŰ ����
            inventoryObject.Inst.FieldFKey.SetActive(false);

            checkNear = false;
        }
    }

    //������ ������
    itemData animalData;

    protected void Move()
    {
        if (isWalking || isRunning)
            nav.SetDestination(transform.position + destination * 3f);
        else if (isComeBack)
            nav.SetDestination(destination);


        if (isWalking)
        {
            //�̵���������
            float x = Mathf.Clamp(transform.position.x, spawnPosition.x - 10.0f, spawnPosition.x + 10.0f);
            float z = Mathf.Clamp(transform.position.z, spawnPosition.z - 10.0f, spawnPosition.z + 10.0f);

            transform.position = new Vector3(x, transform.position.y, z);
        }
    }

    bool _delay = true; //������ üũ

    protected void ElapseTime()
    {
        if (isAction)
        {
            if (isRunning && _delay)
            {
                StartCoroutine(Delay());
                return;
            }
            else if (isComeBack)
            {
                if (Vector3.Distance(transform.position, spawnPosition) <= 1.0f)
                {
                    isComeBack = false;
                    ReSet();
                    return;
                }
            }
            else if (!isRunning && !isComeBack)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)  // �����ϰ� ���� �ൿ�� ����
                    ReSet();
            }
        }
    }

    protected virtual void ReSet()  // ���� �ൿ �غ�
    {
        isAction = true;

        nav.ResetPath();

        //isWalking = false;
        //anim.SetBool("Walking", isWalking);
        //isRunning = false;
        //anim.SetBool("Running", isRunning);

        isWalking = false;
        isRunning = false;
        isComeBack = false;
        anim.SetBool("Move", false);

        nav.speed = walkSpeed;



        destination.Set(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)); //������ġ
    }

    protected void TryWalk()  // �ȱ�
    {
        currentTime = walkTime;
        isWalking = true;
        //anim.SetBool("Walking", isWalking);
        anim.SetBool("Move", isWalking);
        nav.speed = walkSpeed;
        //Debug.Log("�ȱ�");
    }

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            //PlaySE(sound_Hurt); //->�Ҹ�����
            //anim.SetTrigger("Hurt");
            //Run(_targetPos);
        }
    }

    protected void Dead()
    {
        //PlaySE(sound_Dead); //->�Ҹ�����

        isWalking = false;
        isRunning = false;
        isDead = true;

        anim.SetTrigger("Dead");

        //�ʱ�ȭ�ϰ� ������Ʈ ����
        //������ ȹ��
    }


    private IEnumerator Delay()
    {
        _delay = false;
        yield return new WaitForSeconds(5.0f);
        _delay = true;
        Debug.Log("�̰Ž���");
        ComeBack();

        //yield return new WaitForSeconds(5.0f);

        //��� �߰�
        //ReSet();
        //
    }

    public bool GetComeBack()
    {
        return isComeBack;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public void SetSpawnPosition(Vector3 spawnposition)
    {
        spawnPosition = spawnposition;
    }

    public void ComeBack()
    {
        destination = spawnPosition;
        //currentTime = runTime;
        isWalking = false;
        isRunning = false;
        isComeBack = true;
        nav.speed = comebackSpeed; //���ƿ��¼ӵ�

        anim.SetBool("Move", isRunning);

        hp = 2; //hp �ʱ�ȭ
    }

    //�ʱ�ȭ
    public void initialization()
    {
        isAction = true;  // �ൿ ������ �ƴ��� �Ǻ�
        isWalking = false; // �ȴ���, �� �ȴ��� �Ǻ�
        isRunning = false; // �޸����� �Ǻ�
        isDead = false;   // �׾����� �Ǻ�
        isComeBack = false; //���ƿ����ִ��� �Ǻ�
        currentTime = waitTime;   // ��� ����
        _delay = true;
        hp = 2;
        anim.SetBool("Move", false);
        nav.speed = walkSpeed;
        spawnPosition = this.transform.position; //���� ��ġ�� ���� �̰� �ٲ���Ѵ�.

        destination.Set(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)); //������ġ
    }
}
