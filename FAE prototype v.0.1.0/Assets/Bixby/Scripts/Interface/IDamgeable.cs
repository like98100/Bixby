using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public interface IDamgeable
{
    void TakeHit(float damage);
    void TakeElementHit(float damage, ElementControl.ElementType elementType);
    void TakeElementHit(float damage, EnemyElement element); //���߿� ������ ���ɼ� ����.
}
