using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStatus
{
    public EnemyType enemyType { get; }
    public float maxHp { get; set; }
    public float hp { get; set; }

    public EnemyStatus()
    {

    }

    public EnemyStatus(EnemyType enemyType, float maxHp)
    {
        this.enemyType = enemyType;
        this.maxHp = maxHp;
        hp = maxHp;
    }

    public EnemyStatus SetEnemyStatus(EnemyType enemyType)
    {
        EnemyStatus stat = null;

        if(enemyType == EnemyType.slime)
            stat = new EnemyStatus(enemyType, 100.0f);
        else if(enemyType == EnemyType.wisp)
            stat = new EnemyStatus(enemyType, 150.0f);

        return stat;
    }
}
