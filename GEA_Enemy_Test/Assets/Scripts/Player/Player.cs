using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 5f;
    private Rigidbody characterRigidbody;

    public GameObject Bullet;

    public float Hp = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
 
        Vector3 velocity = new Vector3(inputX, 0, inputZ);
        velocity *= Speed;
        characterRigidbody.velocity = velocity;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(Bullet, this.transform.position, this.transform.rotation);
        }
    }

    public void TakeDamage(float value)
    {
        Hp -= value;
    }
}
