using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusControl : CombatStatus, IDamgeable
{
    public const float GravityForce = 30.0f;

    public float MyStartingHealth = 100.0f;
    public float Health;
    public float MyStartingStamina = 100.0f;
    public float Stamina;
    public float ElementGauge = 0.0f; //원소게이지

    public float ElementGaugeChargeAmount = 25.0f; //원소게이지 차지 량

    public float AttackDamage = 15.0f;
    public float ChargedAttackDamage = 25.0f;
    public float UltDamage = 50.0f;

    protected bool isHitted;
    public bool Dead;
    public float DealtDamage;

    public float DashStaminaAmount = 20.0f;
    public float RunStaminaAmount = 10.0f;
    public float SwimStaminaAmount = 5.0f;
    public float ChargeAttackStaminaAmount = 10.0f;

    protected bool isDashed;
    public float DashDistance = 5.0f;

    public float Speed = 5.0f;
    public float SwimSpeed = 4.0f;
    public float RunSpeed = 10.0f;
    public float DashSpeed = 20.0f;
    public float MyCurrentSpeed;
    public float JumpPower = 5.0f;

    public float ShootDistance = 50f;
    public float SwitchToChargeTime = 2.0f;

    protected float rotationSpeed = 45.0f;
    protected float nextFire = 0.0f;

    protected bool isSkillCoolDown = false;
    public float SkillCoolDown = 10.0f;
    public float RemainTimeForSkill = 0; //UI에 쿨타임 시간을 제공할 부분.

    protected bool isUltimateSkillCoolDown = false;
    public float UltimateSkillCoolDown = 15.0f;
    public float RemainTimeForUltSkill = 0; //UI에 쿨타임 시간을 제공할 부분.

    protected IEnumerator skillCoolDownCalc()
    {
        this.RemainTimeForSkill = SkillCoolDown;
        this.isSkillCoolDown = true;
        while(RemainTimeForSkill > 0)
        {
            RemainTimeForSkill--;
            yield return new WaitForSeconds(1.0f);
        }
        this.RemainTimeForSkill = 0;
        this.isSkillCoolDown = false;
    }
    protected IEnumerator ultSkillCoolDownCalc()
    {
        this.RemainTimeForUltSkill = UltimateSkillCoolDown;
        this.isUltimateSkillCoolDown = true;
        while (RemainTimeForUltSkill > 0)
        {
            RemainTimeForUltSkill--;
            yield return new WaitForSeconds(1.0f);
        }
        this.RemainTimeForUltSkill = 0;
        this.isUltimateSkillCoolDown = false;
    }

    protected override void Start()
    {
        base.Start();
        this.Dead = false;

        if (QuestObject.manager.GetIndex() < 5 
            || (QuestObject.manager.GetIndex() == 5 && !QuestObject.manager.GetIsClear()))  // 미개방 상태
        {
            this.MyElement = ElementType.NONE;
        }
        else
        {
            this.mySkillStartColor = FireSkillStartColor;
            this.mySkillEndColor = FireSkillEndColor;
            this.MyElement = ElementType.FIRE;
        }

        this.EnemyElement = ElementType.NONE;

        this.Health = MyStartingHealth;
        this.Stamina = MyStartingStamina;
        this.MyCurrentSpeed = Speed;
        this.isDashed = false;
    }

    public virtual void TakeHit(float damage)
    {
        this.isHitted = true;
        Health -= damage * AdditionalDamage;
        UI_Control.Inst.damageSet((damage * AdditionalDamage).ToString(), GameObject.FindGameObjectWithTag("Player"));//대미지 UI 추가 코드
        this.DealtDamage = Mathf.Round(damage * 10) * 0.1f;

        // 피격 사운드 넣기
        SoundManage.instance.PlaySFXSound(9, "Player");
        if (Health <= 0 && !Dead)
        {
            die();
        }
    }

    public virtual void TakeElementHit(float damage, ElementRule.ElementType enemyElement) //나중에 삭제될 가능성 있음.
    {
        this.isHitted = true;
        setEnemyElement(enemyElement); // 이렇게 EnemyElement로 바꿔서 쓰거나 enemyElement그대로 써도 될듯.
        float curDamage = attackedOnNormal(damage);

        if (EnemyElement != ElementType.NONE)
        {
            if (ElementStack.Count != 0)
            {
                if (EnemyElement != this.ElementStack.Peek())
                {
                    this.ElementStack.Push(EnemyElement);
                    checkIsPopTime();
                }
            }
            else
            {
                this.ElementStack.Push(EnemyElement);
                checkIsPopTime();
            }
        }

        Health -= curDamage * AdditionalDamage;
        UI_Control.Inst.damageSet((curDamage * AdditionalDamage).ToString(), GameObject.FindGameObjectWithTag("Player"));//대미지 UI 추가 코드
        this.DealtDamage = Mathf.Round(curDamage * 10) * 0.1f;

        // 피격 사운드 넣기
        SoundManage.instance.PlaySFXSound(9, "Player");
        if (Health <= 0 && !Dead)
        {
            die();
        }
    }

    public virtual void StaminaUse(float amount)
    {
        Stamina -= amount;
    }

    public virtual void StaminaTickUse(float amount)
    {
        Stamina -= Time.deltaTime * amount;
    }

    public virtual void StaminaRegerenate()
    {
        if(Stamina != MyStartingStamina)
        {
            Stamina += Time.deltaTime * 10f;
        }

        if(Stamina > MyStartingStamina)
        {
            this.Stamina = MyStartingStamina;
        }
    }

    protected virtual void die()
    {
        this.gameObject.GetComponent<PlayerContorl>().NextState = PlayerContorl.STATE.DEAD;
        this.gameObject.GetComponent<PlayerContorl>().PrevState = this.gameObject.GetComponent<PlayerContorl>().State;
        //GameObject.Destroy(gameObject);
    }
}