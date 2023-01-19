using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObj : PhysicsObj
{
    [SerializeField] bool canDamage = true;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (GetRigidbody2D().velocity.magnitude >= 1 && canDamage)
        {
            IDamage idamageable = col.gameObject.GetComponent<IDamage>();
            if (idamageable != null)
            {
                if(idamageable.team != GetTeam())
                {
                    idamageable.Hit( (col.transform.position - transform.position).normalized, 
                    GetRigidbody2D().velocity.magnitude);
                    Hit(Vector2.zero,0);
                }
            }
        }
    }

    public GrabObj ThrowObj(Vector2 dir, int t) {
        team = t;
        transform.SetParent(null);
        GetRigidbody2D().isKinematic = false;
        GetRigidbody2D().velocity = Vector2.zero;
        GetRigidbody2D().AddForce(dir*13, ForceMode2D.Impulse);
        return null;
    }
    public void SetParent(Transform father) {
        GetRigidbody2D().velocity = Vector2.zero;
        GetRigidbody2D().isKinematic = true;
        transform.SetParent(father);
        transform.localPosition = Vector2.zero;
    }
}
