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

    public GameObject shield;

    public NavMeshAgent MyAgent;
    public Animator Anim;
    public Rigidbody rigid;
    public SphereCollider col;

    public float DealtDamage;
    public bool isBarriered;

    public float SkillCooldown;

    // Start is called before the first frame update
    void Start()
    {   
        Stat = new EnemyStatus();
        Stat = Stat.SetEnemyStatus(EnemyType.FinalBoss, ElementType.FIRE);

        this.MyElement = Stat.element;
        isBarriered = true;

        SkillCooldown = 0.0f;

        MyAgent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();

        shield.SetActive(true);
        col.enabled = true;

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
        //rigid.AddForce(Vector3.down * 50.0f);
        MyAgent.speed = Stat.moveSpeed * SpeedMultiply;
        findPlayer(Stat.sight);

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
        if ((int)Stat.element == (int)ElementType.FIRE)
        {
            Stat.element = ElementType.ICE;
            this.MyElement = Stat.element;
        }
        else if ((int)Stat.element == (int)ElementType.ICE)
        {
            Stat.element = ElementType.WATER;
            this.MyElement = Stat.element;
        }
        else if ((int)Stat.element == (int)ElementType.WATER)
        {
            Stat.element = ElementType.ELECTRICITY;
            this.MyElement = Stat.element;
        }
        SetBarrier();
        SkillCooldown = 0.0f;
    }

    public void SetBarrier()
    {
        Stat.barrier = Stat.maxBarrier;
        isBarriered = true;
        shield.SetActive(true);
        col.enabled = true;
    }

    public virtual void TakeHit(float damage)
    {
        Stat.hp -= damage * AdditionalDamage;
        UI_Control.Inst.damageSet((damage * AdditionalDamage).ToString(), this.gameObject);//대미지 UI 추가 코드
        DealtDamage = Mathf.Round(damage * 10) * 0.1f;
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

        if (!isBarriered)
            Stat.hp -= curDamage * AdditionalDamage;
        else
            Stat.barrier -= curDamage * AdditionalDamage;

        UI_Control.Inst.damageSet((curDamage * AdditionalDamage).ToString(), this.gameObject);//����� UI �߰� �ڵ�
        DealtDamage = Mathf.Round(curDamage * 10) * 0.1f;

        if (Stat.barrier <= 0.0f)
        {
            isBarriered = false;
            shield.SetActive(false);
            col.enabled = false;
        }
    }

}
