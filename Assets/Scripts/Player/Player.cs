using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamage
{
    [SerializeField] int hp = 6, playerTeam;
    [SerializeField] float breakForce = 100;
    int curhp = 6;
    Rigidbody2D rb;
    public int team { get { return playerTeam; } set { playerTeam = value; } }

    private void Start()
    {
        curhp = hp;
        rb = GetComponent<Rigidbody2D>();
    }
    public void KockBack(Vector2 dir, float force)
    {
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(dir * force * (1 - (curhp / hp)), ForceMode2D.Impulse);
    }

    public void Hit(Vector2 dir, float force = 0, int team = 0)
    {
        if(this.team == team)
            return;

        curhp--;

        KockBack(dir, force);

        if (hp <= 0)
            Destroy(gameObject);
    }
}
