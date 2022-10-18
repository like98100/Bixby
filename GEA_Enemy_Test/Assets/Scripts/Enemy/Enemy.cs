using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStatus Stat;
    public EnemyType Type;
    public EnemyElement Element;

    public float EnemySight;
    public float Timer;

    public STATE State;
    public enum STATE
    {
        IDLE,
        PATROL,
        CHASE,
        RUNAWAY,
        SLEEP
    }

    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        Stat = new EnemyStatus();
        Stat = Stat.SetEnemyStatus(Type, Element);

        Timer = 0.0f;

        State = STATE.IDLE;

        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = Vector3.zero;

        findPlayer();

        if(State != STATE.CHASE && State != STATE.RUNAWAY)
            stateChange();

        if(Stat.hp <= 30.0f)
            State = STATE.RUNAWAY;

        died();
    }
    
    private void stateChange()
    {
        Timer += Time.deltaTime;

        if(Timer >= 5.0f)
        {
            if(State == STATE.IDLE)
                State = STATE.PATROL;
            else
                State = STATE.IDLE;
            Timer = 0.0f;
        }
    }

    private void findPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, EnemySight, 
                                                    1, QueryTriggerInteraction.Ignore);

        if(colliders.Length > 0)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].tag == "Player")
                    State = STATE.CHASE;
            }
        }
    }

    private void died()
    {
        if(Stat.hp <= 0.0f)
            Destroy(gameObject);
    }



    public void TakeDamage(float value)
    {
        Stat.hp -= value;
    }

    public void RunToSleepChange()
    {
        State = STATE.SLEEP;
    }

    public void ReCharge()
    {
        if(Stat.hp == Stat.maxHp)
            State = STATE.IDLE;
    }

}
