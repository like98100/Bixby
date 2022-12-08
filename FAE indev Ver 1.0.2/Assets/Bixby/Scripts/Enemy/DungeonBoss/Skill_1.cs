using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_1 : ElementControl
{
    public float Time_;

    public ElementType element;

    public Material[] mat;

    private CapsuleCollider col;

    // Start is called before the first frame update
    void Start()
    {
        Time_ = 0.0f;
        col = gameObject.GetComponent<CapsuleCollider>();

        Material[] temp;

        if ((int)element == (int)ElementType.ICE)
        {
            temp = gameObject.GetComponent<MeshRenderer>().materials;
            temp[0] = mat[0];
            gameObject.GetComponent<MeshRenderer>().materials = temp;
        }
        else if ((int)element == (int)ElementType.WATER)
        {
            temp = gameObject.GetComponent<MeshRenderer>().materials;
            temp[0] = mat[1];
            gameObject.GetComponent<MeshRenderer>().materials = temp;
        }
        else if ((int)element == (int)ElementType.ELECTRICITY)
        {
            temp = gameObject.GetComponent<MeshRenderer>().materials;
            temp[0] = mat[2];
            gameObject.GetComponent<MeshRenderer>().materials = temp;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Time_ += Time.deltaTime;

        if (Time_ >= 0.75f)
        {
            col.enabled = true;
            transform.localScale += Vector3.up * 2.5f * Time.deltaTime * 5.0f;
        }
        
        if (Time_ >= 2.5f)
            Destroy(this.transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            other.GetComponent<PlayerContorl>().TakeElementHit(10.0f, element);
    }
}
