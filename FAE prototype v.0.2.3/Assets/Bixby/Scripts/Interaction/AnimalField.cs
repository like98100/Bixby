using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalField : MonoBehaviour
{
    public GameObject rangeObject;
    BoxCollider rangeCollider;

    public int animalCount = 3; // ���� ����
    public float respawnTime = 5.0f; //������ �ð�

    public GameObject deer; //�罿

    public List<GameObject> objAnimal = new List<GameObject>();

    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < animalCount; i++)
        {
            GameObject test = Instantiate(deer, Return_RandomPosition(), Quaternion.identity);
            objAnimal.Add(test);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in objAnimal)
        {
            if (item.activeSelf == false && item.GetComponent<Deer>().testchack == false)
            {
                //������ �ڷ�ƾ ����
                StartCoroutine(respawn(item));

                item.GetComponent<Deer>().testchack = true;
            }
        }
    }

    IEnumerator respawn(GameObject animal)
    {
        //harvest.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnTime);

        //��ġ ���� �̵�

        animal.transform.position = Return_RandomPosition();
        //animal.GetComponent<deer>().SetSpawnPosition(animal.transform.position);
        animal.GetComponent<Deer>().initialization();
        //animal.GetComponent<deer>().ReSet();

        animal.gameObject.SetActive(true);
    }

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
            respawnPosition = hit.point + new Vector3(0, 1, 0);
        }

        return respawnPosition;
    }
}
