using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    private Vector3 moveSpeed = new Vector3(0.0f, 0.01f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        System.Console.WriteLine("start" + transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.position - moveSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        System.Console.WriteLine("Enter");
        if (collision.gameObject.name == "Water")
        {
            System.Console.WriteLine("in");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Water")
        {
            System.Console.WriteLine("Ãæµ¹.");
            transform.position = transform.position + moveSpeed;
        }
    }
}
