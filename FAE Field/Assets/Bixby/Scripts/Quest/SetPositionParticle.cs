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
        positionParticle = GameObject.Find("PositionParticle");   
        initializeVariable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initializeVariable()
    {
        int sum = this.gameObject.GetComponent<QuestObject>().GetObjectId();
        particlePosition = this.gameObject.GetComponent<QuestObject>().GetPosition();
        positionParticle.transform.position = particlePosition;
        Debug.Log("위치 변환 완료");
    }
}
