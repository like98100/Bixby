using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyGroup;
    [SerializeField] private GameObject[] doors;
    private bool[] isOpen;

    private void Start()
    {
        isOpen = new bool[doors.Length];
    }
    void Update()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (enemyGroup[i].transform.childCount == 0 && !isOpen[i])
            {
                int length = doors[i].transform.childCount;
                if (length > 0)
                {
                    for (int j = 0; j < length; j++)
                    {
                        StartCoroutine(doors[i].transform.GetChild(j).GetComponent<Dissolve>().Act(doors[i].transform.GetChild(j).gameObject));
                    }                    
                    isOpen[i] = true;
                }
                else
                {
                    StartCoroutine(doors[i].GetComponent<Dissolve>().Act(doors[i]));
                    isOpen[i] = true;
                }
            }
        }
    }
}
