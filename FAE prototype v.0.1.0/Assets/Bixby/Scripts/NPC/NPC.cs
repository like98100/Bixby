using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPC : MonoBehaviour
{
    Speech speech;
    [SerializeField] string npcName;
    bool playerClose;
    GameObject keyInst;
    [SerializeField] int talkIndex;
    GameObject canvasObj;
    GameObject nameObj;
    Text nameUI;
    GameObject notify;
    Shop shop;
    void Start()
    {
        canvasObj = this.transform.GetChild(0).gameObject;
        nameObj = canvasObj.transform.GetChild(0).gameObject;
        speech = UI_Control.Inst.Speech;
        notify = this.transform.GetChild(1).gameObject;
        playerClose = false;
        talkIndex = 0;
        canvasObj.SetActive(true);
        nameUI = nameObj.transform.GetChild(0).GetComponent<Text>();
        nameUI.text = npcName;
        nameObj.SetActive(playerClose);
        shop = UI_Control.Inst.Shop;
        keyInst = Instantiate(inventoryObject.Inst.getObj("KeyF"), this.gameObject.transform.GetChild(0));
        keyInst.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        camRotate -= Vector3.right * 90f;
        notify.transform.rotation = Quaternion.Euler(camRotate);
        if(keyInst.activeSelf)
        {
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            keyInst.transform.position = wantedPos + Vector3.right * 200f;
        }
        if (UI_Control.Inst.OpenedWindow != null)
        {
            if (UI_Control.Inst.OpenedWindow.name == "Map")
                nameObj.SetActive(false);
            else
                nameObj.SetActive(playerClose);
            keyInst.SetActive(false);
        }
        if (nameObj.activeSelf)
            nameObj.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 2f);
        if (playerClose && Input.GetKeyDown(KeyCode.F))
        {
            if (npcName == "shop")
            {
                shop.SetUp();
                nameObj.SetActive(false);
            }
            else
            {
                speech.setUp(npcName, talkIndex);
            }
            keyInst.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !keyInst.activeSelf)
        {
            keyInst.SetActive(true);
            playerClose = true;
            nameObj.SetActive(playerClose);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            keyInst.SetActive(false);
            playerClose = false;
            nameObj.SetActive(playerClose);
        }
    }
}

