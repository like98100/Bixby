using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStatus
{
    public EnemyType type { get; }
    public int id { get; set; }
    public float maxHp { get; set; }
    public float hp { get; set; }
    public float maxBarrier { get; set; }
    public float barrier { get; set; }
    public float damage { get; set; }
    public float attackRange { get; set; }
    public float attackSpeed { get; set; }
    public float moveSpeed { get; set; }
    public float sight { get; set; }
    public ElementRule.ElementType element { get; set; }

    public EnemyStatus()
    {

    }

    public EnemyStatus(EnemyType type, int id, float maxHp, float maxBarrier, float damage, float attackRange, float attackSpeed, float moveSpeed, float sight, ElementRule.ElementType element)
    {
        this.type = type;
        this.id = id;
        this.maxHp = maxHp;
        hp = maxHp;
        this.maxBarrier = maxBarrier;
        barrier = maxBarrier;
        this.damage = damage;
        this.attackRange = attackRange;
        this.attackSpeed = attackSpeed;
        this.moveSpeed = moveSpeed;
        this.sight = sight;
        this.element = element;
    }

    public EnemyStatus SetEnemyStatus(EnemyType enemyType, ElementRule.ElementType enemyElement)
    {
        EnemyStatus stat = null;
                                // 종류, id, 체력, 쉴드, 대미지, 사정거리, 공격속도, 이동속도, 시야, 속성
        if (enemyType == EnemyType.Melee)
            stat = new EnemyStatus(enemyType, 3001, 100.0f, 50.0f, 10.0f, 2.0f, 5.0f, 5.0f, 15.0f, enemyElement);
        else if (enemyType == EnemyType.Ranged)
            stat = new EnemyStatus(enemyType, 3001, 150.0f, 50.0f, 15.0f, 7.0f, 5.0f, 5.0f, 15.0f, enemyElement);
        else if (enemyType == EnemyType.FinalBoss)
            stat = new EnemyStatus(enemyType, 3002,  500.0f, 50.0f, 30.0f, 5.5f, 2.0f, 10.0f, 100.0f, enemyElement);

        return stat;
    }
}
