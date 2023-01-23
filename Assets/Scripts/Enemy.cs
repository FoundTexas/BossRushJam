using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamage
{
    [SerializeField] int hp = 1;
    Animator anim;
    Vector2 originalPos;
    private void Awake() {
        anim = GetComponent<Animator>();
    }
    public Animator GetAnimator() {return anim;}
    public int team { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public virtual void Hit(Vector2 dir, float force = 0, int team = 0)
    {
        hp--;
    }
}
