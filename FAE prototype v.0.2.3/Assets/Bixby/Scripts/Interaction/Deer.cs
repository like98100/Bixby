using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : WeakAnimal
{
    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }

    private void RandomAction()
    {
        //RandomSound();

        int _random = Random.Range(0, 4); // ���, Ǯ���, �θ���, �ȱ�

        if (_random == 0 || _random == 1 || _random == 2)
            Wait();
        //else if (_random == 1)
        //    Eat();
        //else if (_random == 2)
        //    Peek();
        else if (_random == 3)
            TryWalk();
    }

    private void Wait()  // ���
    {
        currentTime = waitTime;
        anim.SetTrigger("idle");
        //Debug.Log("���");
    }

    private void Eat()  // �����
    {
        currentTime = waitTime;
        anim.SetTrigger("idle");
        //Debug.Log("Ǯ ���");
    }

    private void Peek()  // �θ���
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        //Debug.Log("�θ���");
    }
}
