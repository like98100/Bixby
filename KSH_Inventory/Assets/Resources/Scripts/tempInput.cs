using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempInput : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventory.SetActive(!inventory.activeSelf);
        if (Input.GetKey(KeyCode.W))
            this.transform.position += Vector3.forward * 0.1f;
        if (Input.GetKey(KeyCode.S))
            this.transform.position -= Vector3.forward * 0.1f;
        if (Input.GetKey(KeyCode.A))
            this.transform.Rotate(new Vector3(0f, -100f * Time.deltaTime, 0f));
        if (Input.GetKey(KeyCode.D))
            this.transform.Rotate(new Vector3(0f, 100f * Time.deltaTime, 0f));
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "item")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                inventoryObject.Inst.getFieldItem(other.gameObject);
            }
        }
    }
}
