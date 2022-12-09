using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEne : ElementControl
{
    public GameObject ObjRef;
    public bool isCharged;
    public ElementType myEle;
    public float myDmg;

    public float Time_;

    void Start()
    {
        if (isCharged)
            transform.localScale += new Vector3(1.0f, 1.0f, 1.0f);

        Vector3 myVec = ObjRef.transform.position;
        myVec += Vector3.up*0.5f;

        Time_ = 0.0f;

        transform.LookAt(myVec);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * 0.2f);

        Time_ += Time.deltaTime;

        if (Time_ > 5.0f)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (isCharged)
                other.gameObject.GetComponent<PlayerContorl>().TakeElementHit(myDmg*2.0f, myEle);
            else
                other.gameObject.GetComponent<PlayerContorl>().TakeElementHit(myDmg, myEle);
            Destroy(gameObject);
        }
    }
}
