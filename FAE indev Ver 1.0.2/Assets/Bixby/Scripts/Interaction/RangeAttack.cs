using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public float Range = 4.0f;
    public GameObject effectObj;
    public float time = 3;




    // Start is called before the first frame update
    void Start()
    {
        //dds = effectObj.GetComponent<MeshRenderer>().materials[0].GetFloat("Vector1_4b313987053c459c8c6d64938e820ea9");
        
        StartCoroutine(Explosion(1.0f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Explosion(float target, float current)
    {
        //������ ���� ����Ʈ Ű��
        effectObj.SetActive(true);
        //float duration = 3.0f;
        float offset = (target - current) / time;
        
        while (current<=target)
        {
            current += offset * Time.deltaTime;
            effectObj.GetComponent<MeshRenderer>().materials[0].SetFloat("Vector1_4b313987053c459c8c6d64938e820ea9", current);

            yield return null;
        }

        current = target;

        //yield return new WaitForSeconds(0.0f);
        //3�� �� ������ ����Ʈ Ű��
        effectObj.SetActive(false);

        //�ݶ��̴� �ݷ� ����
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                        Range, Vector3.up, 0f, LayerMask.GetMask("P"));
        
        //�̰��� �� �ݶ��̴��� ����Ǹ� �÷��̾�� �÷��̾ ���� ����. ���� ������ ���� ����.
        foreach (RaycastHit hitObj in rayHits)
        {
            //�ε����� �Ǵ�.
            //hitObj.transform.GetComponent<move>().Hit();
            //���� �ݶ��̴� �ε��� ������Ʈ�� �ִ� ��ũ��Ʈ�� �о�ͼ� ���ڿ��� ����
        }


    }

    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
