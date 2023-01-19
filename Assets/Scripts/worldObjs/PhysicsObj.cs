using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsObj : MonoBehaviour, IDamage
{
    [SerializeField] int hp = 1, npcTeam = -1;
    [SerializeField] float breakForce = 100, magnitude;
    [SerializeField] bool inmortal = false;
    int curhp = 1;
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curhp = hp;
    }

    private void Update() {
        magnitude = rb.velocity.magnitude;   
    }

    public Rigidbody2D GetRigidbody2D() { return rb; }
    public int GetHP() { return curhp; }
    public int GetTeam() { return this.team; }

    public void KockBack(Vector2 dir, float force)
    {
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(dir * force * (1 - (curhp / hp)), ForceMode2D.Impulse);
    }

    public int team
    {
        get { return npcTeam; }
        set{ npcTeam = value;}
    }

    public void Hit(Vector2 dir, float force = 0)
    {
        if (!inmortal)
            curhp--;

        KockBack(dir, force);

        if (curhp <= 0)
            Destroy(gameObject);
    }
}
