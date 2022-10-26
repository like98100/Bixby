using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamgeable
{
    void TakeHit(float damage);
    // 건드린 부분
    void TakeElementHit(float damage, ElementRule.ElementType element);
}
