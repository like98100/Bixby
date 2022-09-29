using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldItem : MonoBehaviour
{
    public itemData itemData;
    float angle;
    [SerializeField] GameObject keyF;
    GameObject keyInst;
    void Start()
    {
        
    }
    public void setup(itemData data)
    {
        itemData = data;
        angle = 0f;
        keyInst = null;
    }
    // Update is called once per frame
    void Update()
    {
        angle += 60 * Time.deltaTime;
        this.transform.localEulerAngles = new Vector3(0f, angle, 15f);
    }
    private void OnDestroy()
    {
        Destroy(keyInst);
        keyInst = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            keyInst = Instantiate(keyF, GameObject.Find("Canvas").transform);
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            keyInst.transform.position = wantedPos + Vector3.right * 200f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag=="Player")
        {
            Destroy(keyInst);
            keyInst = null;
        }
    }
}
