using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestField : MonoBehaviour
{
    public GameObject rangeObject;
    BoxCollider rangeCollider;

    public int harvestCount = 3; // ä���� ����
    public float respawnTime = 5.0f; //������ �ð�

    public GameObject harvestObj; //ä���� ������Ʈ ������ �ִ°�
    public List<GameObject> obj = new List<GameObject>(); //�� ��ũ��Ʈ�� ������ ä���� ������Ʈ ����Ʈ

    private void Awake()
    {
        //this.rangeObject.transform.position = new Vector3(this.rangeObject.transform.position.x, 35.0f, this.rangeObject.transform.position.z);
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }

    //������Ʈ�� Y�� ����
    public Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        // �ݶ��̴��� ����� �������� bound.size ���
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;

        RaycastHit hit;

        Vector3 look = transform.TransformDirection(Vector3.down);
        Debug.DrawRay(transform.position, look * 100, Color.red);

        if (Physics.Raycast(respawnPosition, look, out hit, 100, LayerMask.GetMask("Ground")))
        {
            respawnPosition = hit.point;
        }

        return respawnPosition;
    }

    private void Start()
    {
        for (int i = 0; i < harvestCount; i++)
        {
            GameObject instantHarvest = Instantiate(harvestObj, Return_RandomPosition(), Quaternion.identity);
            obj.Add(instantHarvest);
        }
    }

    private void Update()
    {
        foreach (var item in obj)
        {
            if (item.activeSelf == false && item.GetComponent<Harvest>().testchack == false)
            {
                //������ �ڷ�ƾ ����
                StartCoroutine(respawn(item));
                item.GetComponent<Harvest>().testchack = true;
            }
        }
    }

    IEnumerator respawn(GameObject harvest)
    {
        //harvest.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnTime);

        //��ġ ���� �̵�
        harvest.transform.position = Return_RandomPosition();

        harvest.gameObject.SetActive(true);
    }
}
