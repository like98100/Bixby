using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusControl : ElementControl, IDamgeable
{
    public const float GravityForce = 30.0f;

    public float MyStartingHealth = 100.0f;
    public float Health;
    public float MyStartingStamina = 100.0f;
    public float Stamina;

    public float attackDamage = 10.0f;

    protected bool isHitted;
    public bool Dead;
    public float DealtDamage;

    public float DashStaminaAmount = 20.0f;
    public float RunStaminaAmount = 10.0f;
    public float SwimStaminaAmount = 10.0f;
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
    public float FireRate = 0.25f;

    protected float rotationSpeed = 45.0f;
    protected float nextFire = 0.0f;

    public float AdditionalDamage = 1.0f; // �ۼ�Ƽ�� �������� ����Ǿ�, �޴� ���� �߰������ ��Ÿ����.
    public float SpeedMultiply = 1.0f; // �ӵ� ���� �����, Ȥ�� �ӵ� ���� ������ ���� ���.

    protected override void Start()
    {
        base.Start();
        Health = MyStartingHealth;
        Stamina = MyStartingStamina;
        MyCurrentSpeed = Speed;
        isDashed = false;
    }

    public virtual void TakeHit(float damage)
    {
        isHitted = true;
        Health -= damage * AdditionalDamage;
        DealtDamage = Mathf.Round(damage * 10) * 0.1f;
        if (Health <= 0 && !Dead)
        {
            die();
        }
    }
    //public virtual void TakeElementHit(float damage, ElementRule.ElementType elementType)
    //{
    //    isHitted = true;
    //    if (elementType != ElementType.NONE && elementType != ElementStack.Peek())
    //    {
    //        this.ElementStack.Push(elementType);
    //    }
    //    Health -= damage * AdditionalDamage;
    //    DealtDamage = Mathf.Round(damage * 10) * 0.1f;
    //    if (Health <= 0 && !Dead)
    //    {
    //        die();
    //    }
    //}

    public virtual void TakeElementHit(float damage, ElementRule.ElementType enemyElement) //���߿� ������ ���ɼ� ����.
    {
        isHitted = true;
        setEnemyElement(enemyElement); // �̷��� EnemyElement�� �ٲ㼭 ���ų� enemyElement�״�� �ᵵ �ɵ�.
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

        Health -= curDamage;
        DealtDamage = Mathf.Round(curDamage * 10) * 0.1f;
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
            Stamina = MyStartingStamina;
        }
    }

    protected virtual void die()
    {
        Dead = true;
        GameObject.Destroy(gameObject);
    }
}