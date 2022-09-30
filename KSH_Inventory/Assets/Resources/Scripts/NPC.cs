using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] Speech speech;
    [SerializeField] string Name;
    bool playerClose;
    [SerializeField] GameObject keyF;
    GameObject keyInst;
    [SerializeField] int talkIndex;
    void Start()
    {
        playerClose = false;
        talkIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerClose && Input.GetKeyDown(KeyCode.F))
            speech.setUp(Name, talkIndex);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            keyInst = Instantiate(keyF, GameObject.Find("Canvas").transform);
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            keyInst.transform.position = wantedPos + Vector3.right * 200f;
            playerClose = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(keyInst);
            keyInst = null;
            playerClose = false;
        }
    }
}
