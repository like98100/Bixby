using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnemyHp : MonoBehaviour
{
    [SerializeField] GameObject hpPrefab;
    [SerializeField] GameObject shieldPrefab;
    public class enemyHp
    {
        public List<GameObject> hpObjects;
        public List<Enemy> enemies;
        public FinalBoss boss;
        public GameObject shieldObject;
        public enemyHp()
        {
            hpObjects = new List<GameObject>();
            enemies = new List<Enemy>();
            boss = null;
            shieldObject = null;
        }
    }
    public static enemyHp EnemyHps;
    private void Awake()
    {
        EnemyHps = new enemyHp();
    }
    void Update()
    {
        if (EnemyHps.boss == null)
        {
            foreach (var item in EnemyHps.hpObjects)
            {
                int index = EnemyHps.hpObjects.IndexOf(item);
                Enemy target = EnemyHps.enemies[index];//EnemyHps.hpObjects.IndexOf(item);

                if (target.State == Enemy.STATE.CHASE && isInScreen(target.gameObject))
                {
                    gaugeSet(false, true, item);
                }
                else
                    item.SetActive(false);
            }
        }
        else
        {
            FinalBoss target = EnemyHps.boss;
            GameObject item = EnemyHps.hpObjects[0];
            GameObject itemShield = EnemyHps.shieldObject;
            if (isInScreen(target.gameObject) && target.Stat.barrier != target.Stat.maxBarrier)
            {
                if (target.Stat.barrier > 0)
                {
                    itemShield.SetActive(true);
                    item.SetActive(false);
                    gaugeSet(true, false, itemShield);
                }
                else
                {
                    if (!item.activeSelf)
                        item.SetActive(true);
                    if (itemShield.activeSelf)
                        itemShield.SetActive(false);
                    gaugeSet(true, true, item);
                }
            }
            else
            {
                item.SetActive(false);
                itemShield.SetActive(false);
            }
        }
    }
    public GameObject getPrefab(bool isHp)
    {
        if (isHp)
            return hpPrefab;
        else
            return shieldPrefab;
    }

    bool isInScreen(GameObject target)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        bool isInScreen = screenPos.z > 0
            && screenPos.x > 0 && screenPos.x < 1920f
            && screenPos.y > 0 && screenPos.y < 1080f;
        return isInScreen;
    }

    void gaugeSet(bool isBoss, bool isHp, GameObject item)
    {
        Enemy targetEnemy = null;
        FinalBoss targetBoss = null;
        if (isBoss)
        {
            targetBoss = EnemyHps.boss;
            item.transform.position = Camera.main.WorldToScreenPoint(targetBoss.gameObject.transform.position + Vector3.up * 10f);
            item.GetComponent<UnityEngine.UI.Slider>().value = isHp ? targetBoss.Stat.hp / targetBoss.Stat.maxHp : targetBoss.Stat.barrier / targetBoss.Stat.maxBarrier;
        }
        else
        {
            if (UI_Control.Inst.OpenedWindow != null)
                if (UI_Control.Inst.OpenedWindow.name == "Map")
                {
                    item.SetActive(false);
                    return;
                }
            targetEnemy = EnemyHps.enemies[EnemyHps.hpObjects.IndexOf(item)];
            item.SetActive(true);
            item.transform.position = Camera.main.WorldToScreenPoint(targetEnemy.gameObject.transform.position + Vector3.up * 5f);
            item.GetComponent<UnityEngine.UI.Slider>().value = targetEnemy.Stat.hp / targetEnemy.Stat.maxHp;
        }

    }
}
