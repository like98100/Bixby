using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    //������Ʈ Ǯ�� ����ϱ� 
    [SerializeField] private GameObject rippleObj;
    private Material material;

    static float heightOffset = 0.1f;
    private void Awake()
    {
        material = this.gameObject.GetComponent<MeshRenderer>().material;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Test"))
        {
            //������Ʈ Ǯ�� ����ϰų� SetActive ����ؼ� �¿��� �ϱ�
            GameObject ripple = Instantiate(rippleObj, transform);
            Material mat = ripple.GetComponent<MeshRenderer>().material;
            mat.SetVector("_SphereCenter", collision.contacts[0].point);
            Destroy(ripple, 0.5f);
        }
    }

    public void SetActive(bool _bool,Color color)
    {
        this.gameObject.SetActive(_bool);
        this.SetSize();
        this.SetColor(color);
    }
    private void SetSize()
    {
        float height = this.gameObject.transform.parent.GetComponent<BoxCollider>().size.y + heightOffset;
        this.gameObject.transform.localScale = new Vector3(height, height, height);
    }
    private void SetColor(Color color)
    {
        material.SetColor("_Color", color);
    }
}
