using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    //오브젝트 풀링 사용하기 
    [SerializeField] private GameObject rippleObj;
    private Material material;
    Queue<GameObject> ripples = new Queue<GameObject>();

    static float heightOffset = 0.1f;
    private void Awake()
    {
        material = this.gameObject.GetComponent<MeshRenderer>().material;

        for(int i = 0; i < 4; i++)
        {
            GameObject ripple = Instantiate(rippleObj, transform);
            ripple.SetActive(false);

            ripples.Enqueue(ripple);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(CreateRipple(collision));
    }
    IEnumerator CreateRipple(Collision collision)
    {
        if(ripples.Count == 0)
        {
            GameObject go = Instantiate(rippleObj, transform);
            go.SetActive(false);

            ripples.Enqueue(go);
        }

        GameObject ripple = ripples.Dequeue();
        Material mat = ripple.GetComponent<MeshRenderer>().material;
        mat.SetVector("_SphereCenter", collision.contacts[0].point);
        ripple.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        ripple.SetActive(false);
        ripples.Enqueue(ripple);
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
