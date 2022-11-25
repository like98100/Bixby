using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBullet : MonoBehaviour, IDamgeable
{
    public Transform Target;
    public LayerMask Mask = -1;

    public Vector3 Vec;
    public float UpdateInterval = 55.0f;
    public float Time_ = 0.0f;
    public float Speed = 25.0f;

    // Start is called before the first frame update
    void Start()
    {
        findPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Time_ += Time.deltaTime;

        if (Time_ < 5.5f)
        {
            
        }
        else if (Time_ >= UpdateInterval)
        {
            Destroy(gameObject);
        }
        else
        {
            Vec = (Target.position - this.transform.position).normalized;

            transform.position += Vec * Time.deltaTime * Speed;
            transform.forward = Vec;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            
            Destroy(gameObject);
        }
    }
    
    private void findPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10000.0f, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {    
            Target = colliders[0].transform;
        }
    }

    public void TakeHit(float damage)
    {
        Destroy(gameObject);
    }
    public void TakeElementHit(float damage, ElementRule.ElementType elementType)
    {
        Destroy(gameObject);
    }
}
