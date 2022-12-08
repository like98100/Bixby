using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float DealtDamage = 10.0f;

    private float coolDown = 0.5f; //데미지 틱 간격

    private WaitForSeconds CoolDownWaitForSeconds;
    private bool isCoolDown = false;
    public Coroutine CoolCoroutine;

    public IEnumerator tickCalc()
    {
        isCoolDown = true;
        yield return CoolDownWaitForSeconds;
        isCoolDown = false;
    }

    void Start()
    {
        CoolDownWaitForSeconds = new WaitForSeconds(coolDown);
    }

    void Update()
    {
        if (isCoolDown)
        {
            this.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            this.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<IDamgeable>().TakeHit(DealtDamage);
            CoolCoroutine = StartCoroutine(tickCalc());
        }
    }
}
