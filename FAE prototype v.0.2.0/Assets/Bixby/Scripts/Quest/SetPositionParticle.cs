using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionParticle : MonoBehaviour
{
    GameObject positionParticle;
    Vector3 particlePosition;

    // Start is called before the first frame update
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FieldScene")
        {//��ƼŬ ��ü�� ���� ������ if�� �����ص� ����, ���� ���� ��ƼŬ�� ��� �ʵ忡���� �ǵ��� ����
            positionParticle = GameObject.Find("PositionParticle");
            InitializeVariable();
        }
        else
            positionParticle = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeVariable()
    {
        int sum = this.gameObject.GetComponent<QuestObject>().GetObjectId();
        particlePosition = this.gameObject.GetComponent<QuestObject>().GetPosition();
        positionParticle.transform.position = particlePosition;
        Debug.Log("��ġ ��ȯ �Ϸ�");
    }
}
