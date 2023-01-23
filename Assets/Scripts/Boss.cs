using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] int stages = 1;
    int curStarge = 1;
    Vector2 IdelPos;
    
    public override void Hit(Vector2 dir, float force = 0, int team = 0)
    {
        GetAnimator().SetTrigger("HIT");
    }
}
