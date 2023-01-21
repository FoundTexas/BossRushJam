using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObj : PhysicsObj
{
    [SerializeField] bool canDamage = true;
    [SerializeField] Collider2D col;

    private void Start() {
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (GetRigidbody2D().velocity.magnitude >= 1 && canDamage)
        {
            IDamage idamageable = col.gameObject.GetComponent<IDamage>();
            if (idamageable != null)
            {
                idamageable.Hit( (col.transform.position - transform.position).normalized, 
                GetRigidbody2D().velocity.magnitude, team);
                Hit(Vector2.zero,0,team);
            }
        }
    }

    public GrabObj ThrowObj(Vector2 dir, int t) {
        team = t;
        col.isTrigger = false;
        transform.SetParent(null);
        GetRigidbody2D().isKinematic = false;
        GetRigidbody2D().velocity = Vector2.zero;
        GetRigidbody2D().AddForce(dir*3.5f, ForceMode2D.Impulse);
        return null;
    }
    public void SetParent(Transform father, int t) {
        team = t;
        col.isTrigger = true;
        GetRigidbody2D().velocity = Vector2.zero;
        GetRigidbody2D().isKinematic = true;
        transform.SetParent(father);
        transform.localPosition = Vector2.zero;
    }

    public override void Hit(Vector2 dir, float force, int team )
    {
        this.team = team;

        TakeDamage(dir, force);
    }
}
