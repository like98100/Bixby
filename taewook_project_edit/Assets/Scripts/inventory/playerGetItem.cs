using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGetItem : MonoBehaviour
{
    GameObject lessIndexItem;
    void Start()
    {
        lessIndexItem = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (lessIndexItem != null && Input.GetKeyDown(KeyCode.F))
        {
            inventoryObject.Inst.getFieldItem(lessIndexItem);
            lessIndexItem = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (lessIndexItem == null)
            lessIndexItem = other.gameObject;
        else
        {
            lessIndexItem = lessIndexItem.GetComponent<fieldItem>().index < other.GetComponent<fieldItem>().index ? lessIndexItem : other.gameObject;
        }
    }
}
