using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FinalBoss : CombatStatus, IDamgeable
{
    public EnemyStatus Stat;

    public LayerMask Mask = -1;
    public LayerMask Ground = -1;
    public GameObject Target;
    public GameObject FireBall;
    public GameObject WaterBall;
    public GameObject WaterBallSpawner;

    //public GameObject shield;
    public Material[] mat;

    public NavMeshAgent MyAgent;
    public Animator Anim;
    public Rigidbody rigid;
    public SphereCollider col;

    public float DealtDamage;
    public bool isAttacked;
    public bool elecAttack;
    public bool isHitted;

    public float SkillCooldown;

    private Shield shield;
    [SerializeField] private bool setShield;

    // Start is called before the first frame update
    void Start()
    {   
        Stat = new EnemyStatus();
        Stat = Stat.SetEnemyStatus(EnemyType.FinalBoss, ElementType.FIRE);

        this.MyElement = Stat.element;
        isAttacked = false;
        isHitted = false;

        SkillCooldown = 0.0f;

        MyAgent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();

        shield = this.gameObject.transform.Find("Shield").GetComponent<Shield>();
        shield.SetActive(true);
        setShield = true;

        //게이지 생성
        UI_EnemyHp.EnemyHps.hpObjects.Add(Instantiate(UI_Control.Inst.EnemyHp.getPrefab(true), GameObject.Find("UI").transform.GetChild(1)));
        UI_EnemyHp.EnemyHps.hpObjects[0].SetActive(false);
        UI_EnemyHp.EnemyHps.shieldObject = Instantiate(UI_Control.Inst.EnemyHp.getPrefab(false), GameObject.Find("UI").transform.GetChild(1));
        UI_EnemyHp.EnemyHps.shieldObject.SetActive(false);
        UI_EnemyHp.EnemyHps.boss = this;
    }

    void FixedUpdate()
    {
        if (SkillCooldown > 0.0f)
            SkillCooldown -= Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        //rigid.AddForce(Vector3.down * 50.0f);
        MyAgent.speed = Stat.moveSpeed * SpeedMultiply;
        findPlayer(Stat.sight);

        // if (isAttacked)
        //     MyAgent.isStopped = true;
        // else
        //     MyAgent.isStopped = false;

        if (elecAttack && !isHitted)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Stat.attackRange*1.5f, 
                                                    Mask, QueryTriggerInteraction.Ignore);

            if (colliders.Length > 0)
            {    
                setEnemyElement(colliders[0].GetComponent<PlayerContorl>().MyElement);
                colliders[0].GetComponent<PlayerContorl>().TakeElementHit(Stat.damage, Stat.element);
                isHitted = true;
            }
        }

    }

    private void findPlayer(float sight)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sight, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {    
            Target = colliders[0].gameObject;
        }
    }

    public void ChangePhase()
    {
        Material[] temp;

        if ((int)Stat.element == (int)ElementType.FIRE)
        {
            Stat.element = ElementType.ICE;
            this.MyElement = Stat.element;
            
            temp = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials;
            temp[1] = mat[0];
            transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials = temp;
        }
        else if ((int)Stat.element == (int)ElementType.ICE)
        {
            Stat.element = ElementType.WATER;
            this.MyElement = Stat.element;
            
            temp = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials;
            temp[1] = mat[1];
            transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials = temp;
        }
        else if ((int)Stat.element == (int)ElementType.WATER)
        {
            Stat.element = ElementType.ELECTRICITY;
            this.MyElement = Stat.element;

            temp = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials;
            temp[1] = mat[2];
            transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials = temp;
        }
        SetBarrier();
        SkillCooldown = 0.0f;
    }

    public void SetBarrier()
    {
        Stat.barrier = Stat.maxBarrier;
        setShield = true;
        shield.SetActive(true);
        //col.enabled = true;
    }

    public virtual void TakeHit(float damage)
    {
        if (!setShield)
            Stat.hp -= damage * AdditionalDamage;
        else
            Stat.barrier -= damage * AdditionalDamage;
        UI_Control.Inst.damageSet((damage * AdditionalDamage).ToString(), this.gameObject);//대미지 UI 추가 코드
        DealtDamage = Mathf.Round(damage * 10) * 0.1f;
        
        if (Stat.barrier <= 0.0f)
        {
            setShield = false;
            shield.SetActive(false);
            col.enabled = false;
        }
    }

    public virtual void TakeElementHit(float damage, ElementRule.ElementType enemyElement) //���߿� ������ ���ɼ� ����.
    {
        setEnemyElement(enemyElement); // �̷��� EnemyElement�� �ٲ㼭 ���ų� enemyElement�״�� �ᵵ �ɵ�.
        float curDamage = attackedOnNormal(damage);

        if (EnemyElement != ElementType.NONE)
        {
            if (ElementStack.Count != 0)
            {
                if (EnemyElement != this.ElementStack.Peek())
                {
                    Debug.Log("Pushed!");
                    this.ElementStack.Push(EnemyElement);
                    checkIsPopTime();
                }
            }
            else
            {
                Debug.Log("Pushed!");
                this.ElementStack.Push(EnemyElement);
                checkIsPopTime();
            }
        }

        
        Stat.hp -= curDamage * AdditionalDamage;
        UI_Control.Inst.damageSet((curDamage * AdditionalDamage).ToString(), this.gameObject);//����� UI �߰� �ڵ�
        DealtDamage = Mathf.Round(curDamage * 10) * 0.1f;
    }

    public void EndAttack()
    {
        isAttacked = false;
    }

    public void NormalAttackAreaCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position+Vector3.forward*1.5f, Stat.attackRange, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {    
            setEnemyElement(colliders[0].GetComponent<PlayerContorl>().MyElement);
            colliders[0].GetComponent<PlayerContorl>().TakeElementHit(Stat.damage, Stat.element);
        }
    }

    public void DashAttackAreaCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Stat.attackRange*1.5f, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {    
            setEnemyElement(colliders[0].GetComponent<PlayerContorl>().MyElement);
            colliders[0].GetComponent<PlayerContorl>().TakeElementHit(Stat.damage, Stat.element);
        }
    }

    public void IceSkillAttackAreaCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Stat.attackRange*3.0f, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {    
            setEnemyElement(colliders[0].GetComponent<PlayerContorl>().MyElement);
            colliders[0].GetComponent<PlayerContorl>().TakeElementHit(Stat.damage, Stat.element);
        }
    }

    public void IceSkillAniInit()
    {
        Anim.SetFloat("AnimSpeed", 1.4f);
    }

}
