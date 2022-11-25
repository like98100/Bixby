using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : ElementControl
{
    public float EnableTime = 0.5f;
    public float Damage = 20.0f;

    private float timer = 0.0f;

    void Start()
    {
        //�θ��� Start�� �����ϸ� �ȵȴ�.
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= EnableTime)
        {
            this.GetComponent<Collider>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            setEnemyElement(other.gameObject.GetComponent<Enemy>().Stat.element);
            other.gameObject.GetComponent<Enemy>().TakeElementHit(Damage, MyElement);
        }
    }
}
